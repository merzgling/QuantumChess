using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public enum Colore { None, White, Black, Blue, Red, Green }

public class Piece : MonoBehaviour
{
    public string figureName;
    public Field position;
    public Colore color;

    public float probability;
    public ProbabilityBar probabilityBar;

    [HideInInspector]
    public MovingContoller movingController;
    public Transformation transformation;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            if (t.gameObject.GetComponent<ProbabilityBar>())
                probabilityBar = t.gameObject.GetComponent<ProbabilityBar>();
        }
    }
}
