using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController_net : NetworkBehaviour
{
    [Command]
    public void CmdSet(GameObject go1, GameObject go2)
    {
        GameObject.Find("Server").GetComponent<Server>().set(go1, go2);
    }

    private void Start()
    {
        GameObject.Find("Game controller").GetComponent<GameController>().playerController = this;
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
