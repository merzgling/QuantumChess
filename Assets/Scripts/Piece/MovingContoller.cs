using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingContoller : MonoBehaviour
{
    protected Board board;
    protected Piece piece;

    List<Field> findOutBittenFields(SuperPosition sp)
    {
        List<Field> bittenFields = new List<Field>();
        foreach (Piece p in sp.state.Values)
        {
            if (Game.Diplomate.get_state(piece.color, p.color) == DiplomateState.Enemy)
            {
                List<Field> bit = p.movingController.getBites(sp);
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

    protected bool canMove(SuperPosition sp, Field field)
    {
        if (field == null)
            return false;

        if (sp.state[field] == null)
            return true;
        else
            if (Game.Diplomate.get_state(field.piece.color, piece.color) == DiplomateState.Enemy)
            return true;
        else return false;
    }

    protected bool canMove(SuperPosition sp, int x, int y)
    {
        return canMove(sp, board.Map(x, y));
    }

    protected bool canBite(SuperPosition sp, Field field)
    {
        if (field)
            if (sp.state[field])
            {
                if (Game.Diplomate.get_state(sp.state[field].color, piece.color) == DiplomateState.Enemy)
                    return true;
            }
        return false;
    }

    protected bool canBite(SuperPosition sp, int x, int y)
    {
        return canBite(sp, board.Map(x, y));
    }

    protected bool occupied(SuperPosition sp, Field field)
    {
        if (field == null)
            return false;
        else
            if (sp.state[field] == null)
                return false;
            else
                return true;
    }

    protected bool occupied(SuperPosition sp, int x, int y)
    {
        return occupied(sp, board.Map(x, y));
    }

    public bool isBitten(SuperPosition sp, Field field)
    {
        List <Field> bittenFields = findOutBittenFields(sp);
        bool result = bittenFields.Contains(field);
        return result;
    }

    public bool isBitten(SuperPosition sp)
    {
        return isBitten(sp, piece.position);
    }

    public bool isBitten(SuperPosition sp, int x, int y)
    {
        return isBitten(sp, board.Map(x, y));
    }

    // return all fields piece can move
    public List<Field> getMoves(SuperPosition sp)
    {
        List<Field> result = getTransfers(sp);
        foreach (Rule rule in board.rule)
        {
            result = rule.onGettingMoves(piece, result);
        }

        return result;
    }

    protected abstract List<Field> getTransfers(SuperPosition sp);

    //return all fields piece can bite
    public abstract List<Field> getBites(SuperPosition sp);

    // actions must be done with moving piece
    public virtual void OnMove()
    {
        
    }

    public virtual bool IsMovingCorrect(SuperPosition sp, Field f)
    {
        return getMoves(sp).Contains(f);
    }
}