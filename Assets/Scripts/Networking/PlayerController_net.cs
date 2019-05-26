using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController_net : NetworkBehaviour
{
    [SyncVar]
    public Color color;
    [SyncVar]
    public string name;
    
    [Command]
    public void CmdSet(GameObject go1, GameObject go2)
    {
        GameObject.Find("Server").GetComponent<Server>().set(go1, go2);
    }

    [Command]
    public void CmdSetQuantum(GameObject go1, GameObject go2, GameObject go3)
    {
        GameObject.Find("Server").GetComponent<Server>().setQuantum(go1, go2, go3);
    }

    [Command]
    public void CmdSetSurrender(int c)
    {
        GameObject.Find("Server").GetComponent<Server>().setSurrender(c);
    }
    
    public void Initialize()
    {
        var gameController = GameObject.Find("Game controller");
        var gameControllerComponent = gameController.GetComponent<GameController>();
        
        if (!isLocalPlayer)
            return;

        gameControllerComponent.numberOfLocalPlayers++;
        gameControllerComponent.playerController = this;
        gameControllerComponent.colorOfPlayer = ((color == Color.black) ? (Colore.Black) : (Colore.White));

        if (gameControllerComponent.numberOfLocalPlayers > 1)
        {
            Game.GameOption.IsMultiplayer = false;
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                //CmdSet(GameObject.Find("PawnLight (3)"), GameObject.Find("field 3 2"));
            }
        }
    }
}
