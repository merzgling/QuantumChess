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
}
