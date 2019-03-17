using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPosition
{
    public Dictionary<Field, Piece> state = new Dictionary<Field, Piece>(); 

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
    }

    public void StandPiece(Piece p, Field f)
    {
        if (state.ContainsKey(f))
            state[f] = p;
        else
            state.Add(f, p);
    }
}
