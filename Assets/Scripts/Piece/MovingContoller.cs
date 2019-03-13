using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingContoller : MonoBehaviour
{
    protected Board board;
    protected Piece piece;

    List<Field> findOutBittenFields()
    {
        List<Field> bittenFields = new List<Field>();
        foreach (Piece p in board.piece)
        {
            if (Game.Diplomate.get_state(piece.color, p.color) == DiplomateState.Enemy)
            {
                List<Field> bit = p.movingController.getBites();
                foreach (Field f in bit)
                {
                    bittenFields.Add(f);
                }
            }
        }

        return bittenFields;
    }

    void Awake()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
        piece = gameObject.GetComponent<Piece>();
    }

    protected bool canMove(Field field)
    {
        if (field == null)
            return false;

        if (field.piece == null)
            return true;
        else
            if (Game.Diplomate.get_state(field.piece.color, piece.color) == DiplomateState.Enemy)
            return true;
        else return false;
    }

    protected bool canMove(int x, int y)
    {
        return canMove(board.Map(x, y));
    }

    protected bool canBite(Field field)
    {
        if (field)
            if (field.piece)
            {
                if (Game.Diplomate.get_state(field.piece.color, piece.color) == DiplomateState.Enemy)
                    return true;
            }
        return false;
    }

    protected bool canBite(int x, int y)
    {
        return canMove(board.Map(x, y));
    }

    protected bool occupied(Field field)
    {
        if (field == null)
            return false;
        else
            if (field.piece == null)
                return false;
            else
                return true;
    }

    protected bool occupied(int x, int y)
    {
        return occupied(board.Map(x, y));
    }

    public bool isBitten(Field field)
    {
        List <Field> bittenFields = findOutBittenFields();
        bool result = bittenFields.Contains(field);
        return result;
    }

    public bool isBitten()
    {
        return isBitten(piece.position);
    }

    public bool isBitten(int x, int y)
    {
        return isBitten(board.Map(x, y));
    }

    // return all fields piece can move
    public List<Field> getMoves()
    {
        List<Field> result = getTransfers();
        foreach (Rule rule in board.rule)
        {
            result = rule.onGettingMoves(piece, result);
        }

        return result;
    }
    
    // return all fields piece can move
    protected abstract List<Field> getTransfers();

    //return all fields piece can bite
    public abstract List<Field> getBites();

    // actions must be done with moving piece
    public virtual void OnMove()
    {
        
    }
}
