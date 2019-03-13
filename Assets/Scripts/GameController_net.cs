using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController_net : NetworkBehaviour
{


    /*
    [SerializeField]
    protected Piece pickedUpPiece;
    List<Field> movesOfPickedUpPiece;
    [SerializeField]
    protected Board board;
    [SerializeField]
    List<Player> turnSequance;
    int currentTurn;
    // Use this for initialization
    void Awake()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
    }

    void visualMoves()
    {
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
        if (movesOfPickedUpPiece != null)
            foreach (Field field in movesOfPickedUpPiece)
            {
                field.transform.GetChild(0).gameObject.SetActive(false);
                field.transform.GetChild(1).gameObject.SetActive(false);
            }
    }

    void pickUpPiece(GameObject pickedUpObject)
    {
        unVisualMoves();
        pickedUpPiece = pickedUpObject.GetComponent<Piece>();
        movesOfPickedUpPiece = pickedUpPiece.movingController.getMoves();
        visualMoves();
    }

    void unPickUpPiece()
    {
        unVisualMoves();
        pickedUpPiece = null;
        movesOfPickedUpPiece = null;
    }

    public void pickUp(GameObject pickedUpObject)
    {
        // pick up piece
        if (pickedUpObject.GetComponent<Piece>())
        {
            if (pickedUpObject.GetComponent<Piece>().color == turnSequance[currentTurn].color)
                pickUpPiece(pickedUpObject);
            else
                moveRequest(pickedUpPiece, pickedUpObject.GetComponent<Field>());
        }

        // pick up field
        if (pickedUpPiece)
        {
            if (pickedUpObject.GetComponent<Field>())
                moveRequest(pickedUpPiece, pickedUpObject.GetComponent<Field>());
            if (pickedUpObject.GetComponent<Piece>())
                if (Game.Diplomate.get_state(pickedUpObject.GetComponent<Piece>().color, pickedUpPiece.color) == DiplomateState.Enemy)
                    if (!moveRequest(pickedUpPiece, pickedUpObject.GetComponent<Piece>().position))
                        unPickUpPiece();
        }
    }
    [ServerCallback]
    protected bool moveRequest(Piece piece, Field field)
    {
        bool result = board.moveRequest(piece, field);
        if (result)
            nextTurn();
        else
            Debug.LogError("invalid move!");

        return result;
    }

    void nextTurn()
    {
        currentTurn++;
        currentTurn %= turnSequance.Count;
        unPickUpPiece();
    }
    */
}
