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

    protected void Start()
    {
        float a = transform.localPosition.x, b = transform.localPosition.z;
        float a1 = Game.Board.x_component.x, a2 = Game.Board.y_component.x;
        float b1 = Game.Board.x_component.y, b2 = Game.Board.y_component.y;

        float new_x = (a * b2 - a2 * b) / (a1 * b2 - a2 * b1), new_y = (a1 * b - a * b1) / (a1 * b2 - a2 * b1);

        position = Game.Board.Map((int)(new_x + 0.5f), (int)(new_y + 0.5f));
        position.piece = this;
        movingController = gameObject.GetComponent<MovingContoller>();
    }
}
