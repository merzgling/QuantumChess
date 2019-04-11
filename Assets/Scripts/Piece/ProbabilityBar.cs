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
        if (1 - probability < 0.0000001)
            bar.SetActive(false);
        else
            bar.SetActive(true);
        
        valueObject.transform.localPosition = new Vector3(0.5f - (float)(probability / 2), 0);
        valueObject.transform.localScale = new Vector3((float)(probability + 0.001f), 1, 1);
    }
}
