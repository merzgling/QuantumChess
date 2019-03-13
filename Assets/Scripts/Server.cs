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
        gc.GetComponent<GameController>().RpcMoveRequest(piece, field);
    }
}
