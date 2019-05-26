using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
    public int numberOfLocalPlayers = 0;
    public Colore colorOfPlayer;

    private InGameUIController UIController;
    
    [SerializeField] private State _currentState;
    private BoardVisualizer _visualizer;

    [SerializeField] GameObject pickedUpPiece;
    [SerializeField] GameObject pickedUpField;

    List<Field> movesOfPickedUpPiece;
    [SerializeField] Board board;
    [SerializeField] List<Player> turnSequance;
    int currentTurn;

    protected Server server;
    public PlayerController_net playerController;

    public List<PlayerController_net> networkPlayers = new List<PlayerController_net>();

    void Awake()
    {
        UIController = GameObject.Find("InGameUI").GetComponent<InGameUIController>();
        if (Game.GameOption.IsMultiplayer && isServer)
            server = GameObject.Find("Server").GetComponent<Server>();
        board = GameObject.Find("Board").GetComponent<Board>();
        _visualizer = GameObject.Find("Visualizer").GetComponent<BoardVisualizer>();
    }


    void Start()
    {
        if (Game.GameOption.IsMultiplayer)
        {
            int i = 0;
            foreach (var v in GameObject.FindObjectsOfType<PlayerController_net>())
            {
                networkPlayers.Add(v.GetComponent<PlayerController_net>());
                networkPlayers[i].Initialize();
                i++;
            }
        }

        if (colorOfPlayer == Colore.Black && Game.GameOption.IsMultiplayer)
        {
            var CC = GameObject.Find("Camera controller").GetComponent<CameraController>();
            CC.WhiteCamera.SetActive(false);
            CC.BlackCamera.SetActive(true);
        }
    }

    public void RegisterNewPlayer(PlayerController_net pc)
    {
        playerController = pc;
    }

    void ChangeStateToNothingPicked()
    {
        _visualizer.UnPickPiece();
        pickedUpPiece = null;
        pickedUpField = null;
        _currentState = State.NothingPicked;
    }
    
    void ChangeStateToPiecePickedOnce(Piece piece)
    {
        _visualizer.SetPickedPiece(piece);
        pickedUpPiece = piece.gameObject;
        _currentState = State.PiecePickedOnce;
    }
    
    void ChangeStateToPiecePickedDouble(Piece piece)
    {
        _visualizer.SetDoublePickedPiece(piece);
        pickedUpPiece = piece.gameObject;
        _currentState = State.PiecePickedDouble;
    }
    
    void ChangeStateToPiecePickedDoubleWithField(GameObject field)
    {
        _visualizer.SetPickedField(field);
        pickedUpField = field;
        _currentState = State.PiecePickedDoubleWithField;
    }

    void pickUpPiece(GameObject pickedUpObject)
    {
        Piece piece = pickedUpObject.GetComponent<Piece>();
        if (pickedUpPiece == pickedUpObject)
            if (_currentState == State.PiecePickedDouble || (_currentState == State.PiecePickedOnce && !Game.GameOption.IsQuantum))
                ChangeStateToNothingPicked();
            else
                ChangeStateToPiecePickedDouble(piece);
        else
            ChangeStateToPiecePickedOnce(piece);
    }

    void unPickUpPiece()
    {
        ChangeStateToNothingPicked();
    }
    
    public void pickUp(GameObject pickedUpObject)
    {
        // pick up piece
        if (pickedUpObject.GetComponent<Piece>())
        {
            Piece piece = pickedUpObject.GetComponent<Piece>();
            if (piece.color == turnSequance[currentTurn].color && (!Game.GameOption.IsMultiplayer || piece.color == colorOfPlayer))
                pickUpPiece(pickedUpObject);
            else
            {
                pickedUpField = piece.position.gameObject;
                if (_currentState == State.PiecePickedOnce)
                    CmdSentRequestCommonMove(pickedUpPiece, pickedUpField);
                else if (_currentState == State.PiecePickedDouble)
                    Debug.LogError("Cant eat in quantum move");//CmdSentRequestQuantumMove(pickedUpPiece, pickedUpField, null);
            }
        }

        // pick up field
        if (pickedUpObject.GetComponent<Field>())
        {
            if (_currentState == State.PiecePickedOnce)
                CmdSentRequestCommonMove(pickedUpPiece, pickedUpObject);
            else if (_currentState == State.PiecePickedDouble)
                ChangeStateToPiecePickedDoubleWithField(pickedUpObject);
            else if (_currentState == State.PiecePickedDoubleWithField)
                CmdSentRequestQuantumMove(pickedUpPiece, pickedUpField, pickedUpObject);
        }
    }
    
    void CmdSentRequestCommonMove(GameObject pickedUpPiece, GameObject pickedUpField)
    {
        if (Game.GameOption.IsMultiplayer)
            playerController.CmdSet(pickedUpPiece.GetComponent<Piece>().position.gameObject, pickedUpField);
        else
            CommonMoveRequest(pickedUpPiece.GetComponent<Piece>().position, pickedUpField.GetComponent<Field>(), Random.value);
    }
    
    void CmdSentRequestQuantumMove(GameObject pickedUpPiece, GameObject pickedUpField1, GameObject pickedUpField2)
    {
        if (Game.GameOption.IsMultiplayer)
            playerController.CmdSetQuantum(pickedUpPiece.GetComponent<Piece>().position.gameObject, pickedUpField1, pickedUpField2);
        else
            QuantumMoveRequest(pickedUpPiece.GetComponent<Piece>(), pickedUpField1.GetComponent<Field>(), pickedUpField2.GetComponent<Field>());
    }

    void CmdSentRequestSurrender(int c)
    {
        playerController.CmdSetSurrender(c);
    }
    
    [ClientRpc]
    public void RpcCommonMoveRequest(GameObject pickedUpPiece, GameObject pickedUpField, float randomValue)
    {
        CommonMoveRequest(pickedUpPiece.GetComponent<Field>(), pickedUpField.GetComponent<Field>(), randomValue);
    }
    
    [ClientRpc]
    public void RpcQuantumMoveRequest(GameObject pickedUpPiece, GameObject pickedUpField, GameObject pickedUpField2)
    {
        QuantumMoveRequest(pickedUpPiece.GetComponent<Field>().piece, pickedUpField.GetComponent<Field>(),  pickedUpField2.GetComponent<Field>());
    }

    [ClientRpc]
    public void RpcSurrender(int c, float randomValue)
    {
        SurrendeRequest(c, randomValue);
    }

    public void CommonMoveRequest(Field piece, Field field, float randomValue)
    {
        bool result = board.CommonMoveRequest(piece.piece, field, randomValue);
        if (result)
            nextTurn();
        else
            UIController.SayInvalidCommonMove();
    }

    public void QuantumMoveRequest(Piece piece, Field field1, Field field2)
    {
        if (board.QuantumMoveRequest(piece, field1, field2))
            nextTurn();
        else
            UIController.SayInvalidQuantumMove();
    }

    public void SurrendeRequest(int c, float randomValue)
    {
        Surrender(c, randomValue);
        int indexWinPlayer = board.EndGame(randomValue);
        GameOver(indexWinPlayer);
    }

    void nextTurn()
    {
        currentTurn++;
        currentTurn %= turnSequance.Count;

        if (currentTurn == 0)
            UIController.SetWhiteTurn();
        else
            UIController.SetBlackTurn();
        
        unPickUpPiece();
    }

    public void OnSurrender()
    {
        if (turnSequance[currentTurn].color == colorOfPlayer || !Game.GameOption.IsMultiplayer)
            CmdSentRequestSurrender(currentTurn);
    }

    public void Surrender(int color, float randomNamber)
    {
        Colore c = Colore.White;
        if (color == 1)
            c = Colore.Black;
        board.Surrender(c);
    }

    public void GameOver(int playerIndex)
    {
        if (playerIndex == 0)
            UIController.OnWhiteWin();
        else
            UIController.OnBlackWin();
    }
    
    enum State
    {
        NothingPicked,
        PiecePickedOnce,
        PiecePickedDouble,
        PiecePickedDoubleWithField
    }
}
