using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Colore { None, White, Black, Blue, Red, Green }

public class Piece : MonoBehaviour
{
    public string figureName;
    public Field position;
    public Colore color;

    [HideInInspector]
    public MovingContoller movingController;
    public Transformation transformation;
}
