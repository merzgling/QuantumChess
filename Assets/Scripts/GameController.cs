using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
    public bool isNetworkGame;
    [SerializeField] private State _currentState;
    private BoardVisualizer _visualizer;

    [SerializeField] bool doublePicked;
    [SerializeField] GameObject pickedUpPiece;
    [SerializeField] GameObject pickedUpField;

    List<Field> movesOfPickedUpPiece;
    [SerializeField] Board board;
    [SerializeField] List<Player> turnSequance;
    int currentTurn;


    protected Server server;
    public PlayerController_net playerController;


    // Use this for initialization
    void Awake()
    {
        if (isNetworkGame)
            server = GameObject.Find("Server").GetComponent<Server>();
        board = GameObject.Find("Board").GetComponent<Board>();
    }

    void visualMoves()
    {
        if (!Game.GameOption.IsQuantum)
            if (movesOfPickedUpPiece != null)
                foreach (Field field in movesOfPickedUpPiece)
                {
                    if (field.piece)
                        field.transform.GetChild(0).gameObject.SetActive(true);
                    else
                        field.transform.GetChild(1).gameObject.SetActive(true);
                }
    }

    void unVisualMoves()
    {
        if (!Game.GameOption.IsQuantum)
            if (movesOfPickedUpPiece != null)
                foreach (Field field in movesOfPickedUpPiece)
                {
                    field.transform.GetChild(0).gameObject.SetActive(false);
                    field.transform.GetChild(1).gameObject.SetActive(false);
                }
    }

    void ChangeStateToNothingPicked()
    {
        
    }
    
    void ChangeStateToPiecePickedOnce()
    {
        
    }
    
    void ChangeStateToPiecePickedDouble()
    {
        
    }

    void pickUpPiece(GameObject pickedUpObject)
    {
        unVisualMoves();
        if (pickedUpPiece == pickedUpObject)
            if (doublePicked)
            {
                unPickUpPiece();
            }
            else
                doublePicked = true;
            
        doublePicked = true;
        pickedUpPiece = pickedUpObject;
        Piece piece = pickedUpPiece.GetComponent<Piece>();
        movesOfPickedUpPiece = piece.movingController.getMoves(board.superPosition[0]);
        visualMoves();
    }

    void unPickUpPiece()
    {
        unVisualMoves();
        pickedUpPiece = null;
        movesOfPickedUpPiece = null;
    }
    
    void CmdSentRequest(GameObject pickedUpPiece, GameObject pickedUpField)
    {
        if (isNetworkGame)
            playerController.CmdSet(pickedUpPiece, pickedUpField);
        else
            CommonMoveRequest(pickedUpPiece.GetComponent<Piece>(), pickedUpField.GetComponent<Field>());
    }
    
    public void pickUp(GameObject pickedUpObject)
    {
        // pick up piece
        if (pickedUpObject.GetComponent<Piece>())
        {
            if (pickedUpObject.GetComponent<Piece>().color == turnSequance[currentTurn].color)
                pickUpPiece(pickedUpObject);
            else
            {
                pickedUpField = pickedUpObject;
                CmdSentRequest(pickedUpPiece, pickedUpField);
            }
        }

        // pick up field
        if (pickedUpPiece)
        {
            if (pickedUpObject.GetComponent<Field>())
            {
                pickedUpField = pickedUpObject;
                CmdSentRequest(pickedUpPiece, pickedUpField);
            }
            if (pickedUpObject.GetComponent<Piece>())
                if (Game.Diplomate.get_state(pickedUpObject.GetComponent<Piece>().color, pickedUpPiece.GetComponent<Piece>().color) == DiplomateState.Enemy)
                {
                    pickedUpField = pickedUpObject.GetComponent<Piece>().position.gameObject;
                    CmdSentRequest(pickedUpPiece, pickedUpField);
                }
            
        }
    }
    
    [ClientRpc]
    public void RpcMoveRequest(GameObject pickedUpPiece, GameObject pickedUpField)
    {
        CommonMoveRequest(pickedUpPiece.GetComponent<Piece>(), pickedUpField.GetComponent<Field>());
    }

    public void CommonMoveRequest(Piece piece, Field field)
    {
        bool result = board.moveRequest(piece, field);
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
        PiecePickedDouble
    }
}
