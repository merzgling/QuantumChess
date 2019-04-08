using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ProbabilityBar : MonoBehaviour
{
    public GameObject bar;
    public GameObject valueObject;

    public void Awake()
    {
        bar.SetActive(false);
    }

    public void SetProbability(float probability)
    {
        valueObject.transform.localPosition = new Vector3((float)(probability / 2), 0);
        valueObject.transform.localScale = new Vector3((float)(probability + 0.001f), 1, 1);
    }
}
