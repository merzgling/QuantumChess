using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rule : MonoBehaviour
{
    [SerializeField]
    protected string discription;

    protected Board board;

    void Start()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
    }

    // action happents after piece moved
    public virtual void afterMoving(Field from, Piece piece, Field to)
    {

    }

    // action happents before piece moved
    public virtual void beforeMoving(Piece piece, Field field)
    {

    }

    // additional limitation for moves
    public virtual List<Field> onGettingMoves(Piece piece, List<Field> inFields)
    {
        return inFields;
    }
}
