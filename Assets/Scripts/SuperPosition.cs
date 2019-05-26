using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuperPosition
{
    public Dictionary<Field, Piece> state = new Dictionary<Field, Piece>();
    public List<KeyValuePair<Field, Piece>> debug;

    public Piece GetPiece(Field f)
    {
        debug = state.ToList();
        if (state.ContainsKey(f))
            return state[f];
        return null;
    }
    
    public void DoMovement(Field f1, Field f2)
    {
        if (state.ContainsKey(f1))
        {
            if (state.ContainsKey(f2))
                state[f2] = state[f1];
            else
                state.Add(f2, state[f1]);

            state.Remove(f1);
        }
        debug = state.ToList();
    }

    public void StandPiece(Piece p, Field f)
    {
        if (state.ContainsKey(f))
            state[f] = p;
        else
            state.Add(f, p);
        debug = state.ToList();
    }

    public void emptyField(Field f)
    {
        if (state.ContainsKey(f))
            state.Remove(f);
        debug = state.ToList();
    }

    public SuperPosition Clone()
    {
        SuperPosition newSuperposition = new SuperPosition();
        foreach (var s in state)
        {
            newSuperposition.state.Add(s.Key, s.Value);
        }
        return newSuperposition;
    }

    public bool IsGameOpen()
    {
        bool whiteKing = false;
        bool blackKing = false;
        foreach (var st in state)
        {
            if (st.Value.figureName == "King" && st.Value.color == Colore.White)
                whiteKing = true;
            
            if (st.Value.figureName == "King" && st.Value.color == Colore.Black)
                blackKing = true;
        }

        if (whiteKing && blackKing)
            return true;
        else
            return false;
    }

    public void DestroyKing(Colore c)
    {
        Field king = null;
        foreach (var st in state)
        {
            if (st.Value.figureName == "King" && st.Value.color == c)
                king = st.Key;
        }

        if (king)
            emptyField(king);
    }

    public bool HaveKing(Colore c)
    {
        bool result = false;
        foreach (var st in state)
        {
            if (st.Value.figureName == "King" && st.Value.color == c)
                result = true;
        }

        return result;
    }
}
