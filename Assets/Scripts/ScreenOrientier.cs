﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOrientier : MonoBehaviour 
{
	void Start () 
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
}
