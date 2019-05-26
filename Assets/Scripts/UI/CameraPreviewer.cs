using System.Collections;
using System.Collections.Generic;
using Tanks.UI;
using UnityEngine;

public class CameraPreviewer : MonoBehaviour 
{
	// Use this for initialization
	void Start ()
	{
		GameObject.Find("Prewywer").GetComponent<Prewywer>().setCamera(gameObject.GetComponent<Camera>());
	}
}
