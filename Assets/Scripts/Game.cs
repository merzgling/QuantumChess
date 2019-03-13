using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    private static GameController gameController;
    public static GameController GameController
    {
        get
        {
            if (gameController == null)
                gameController = GameObject.Find("Game controller").GetComponent<GameController>();

            return gameController;
        }
    }

    private static Diplomate diplomate;
    public static Diplomate Diplomate
    {
        get
        {
            if (diplomate == null)
                diplomate = GameObject.Find("Diplomate").GetComponent<Diplomate>();
            return diplomate;
        }
    }

    private static Board board;
    public static Board Board
    {
        get
        {
            if (board == null)
                board = GameObject.Find("Board").GetComponent<Board>();

            return board;
        }
    }
}
