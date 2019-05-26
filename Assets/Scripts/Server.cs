using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : NetworkBehaviour
{
    public void set(GameObject piece, GameObject field)
    {
        if (!isServer)
            return;

        GameObject gc = GameObject.Find("Game controller");
        float rand = Random.value;
        gc.GetComponent<GameController>().RpcCommonMoveRequest(piece, field, rand);
    }
    
    public void setQuantum(GameObject piece, GameObject field, GameObject field2)
    {
        if (!isServer)
            return;

        GameObject gc = GameObject.Find("Game controller");
        gc.GetComponent<GameController>().RpcQuantumMoveRequest(piece, field, field2);
    }

    public void setSurrender(int c)
    {
        if (!isServer)
            return;

        GameObject gc = GameObject.Find("Game controller");
        float rand = Random.value;
        gc.GetComponent<GameController>().RpcSurrender(c, rand);
    }
}
