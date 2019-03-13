using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClickableObject : MonoBehaviour
{
    void OnMouseDown()
    {
        Game.GameController.pickUp(gameObject);
    }
}
