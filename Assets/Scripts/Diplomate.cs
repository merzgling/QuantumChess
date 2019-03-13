using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiplomateState : byte { None, Enemy, Ally }


[System.Serializable]
public class DiplomateReference
{
    public Colore side1;
    public Colore side2;
    public DiplomateState state;
}
 
public class Diplomate : MonoBehaviour
{
    [SerializeField]
    protected List<DiplomateReference> states;
    
    

    public DiplomateState get_state(Colore c1, Colore c2)
    {
        foreach (DiplomateReference state in states)
        {
            if (state.side1 == c1 && state.side2 == c2 || state.side1 == c2 && state.side2 == c1)
                return state.state;
        }

        return DiplomateState.None;
    }
}
