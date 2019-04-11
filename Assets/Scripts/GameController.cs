using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
    public bool isNetworkGame;
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

    void Awake()
    {
        if (isNetworkGame)
            server = GameObject.Find("Server").GetComponent<Server>();
        board = GameObject.Find("Board").GetComponent<Board>();
        _visualizer = GameObject.Find("Visualizer").GetComponent<BoardVisualizer>();
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
            if (piece.color == turnSequance[currentTurn].color)
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
        if (isNetworkGame)
            playerController.CmdSet(pickedUpPiece, pickedUpField);
        else
            CommonMoveRequest(pickedUpPiece.GetComponent<Piece>(), pickedUpField.GetComponent<Field>());
    }
    
    void CmdSentRequestQuantumMove(GameObject pickedUpPiece, GameObject pickedUpField1, GameObject pickedUpField2)
    {
        if (isNetworkGame)
            playerController.CmdSet(pickedUpPiece, pickedUpField);
        else
            QuantumMoveRequest(pickedUpPiece.GetComponent<Piece>(), pickedUpField1.GetComponent<Field>(), pickedUpField2.GetComponent<Field>());
    }
    
    [ClientRpc]
    public void RpcMoveRequest(GameObject pickedUpPiece, GameObject pickedUpField)
    {
        CommonMoveRequest(pickedUpPiece.GetComponent<Piece>(), pickedUpField.GetComponent<Field>());
    }

    public void CommonMoveRequest(Piece piece, Field field)
    {
        bool result = board.CommonMoveRequest(piece, field);
        if (result)
            nextTurn();
        else
            Debug.LogError("invalid common move!");
    }

    public void QuantumMoveRequest(Piece piece, Field field1, Field field2)
    {
        if (board.QuantumMoveRequest(piece, field1, field2))
            nextTurn();
        else
            Debug.LogError("Invalid quantum move");
    }

    void nextTurn()
    {
        currentTurn++;
        currentTurn %= turnSequance.Count;
        unPickUpPiece();
    }
    
    enum State
    {
        NothingPicked,
        PiecePickedOnce,
        PiecePickedDouble,
        PiecePickedDoubleWithField
    }
}
