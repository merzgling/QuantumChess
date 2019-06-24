using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlOpener : MonoBehaviour 
{
	public void OnCreditPressed()
	{
		Application.OpenURL("https://twitter.com/normtaksoftware");
	}

	public void OnHowToPlayPressed()
	{
		Application.OpenURL("https://habr.com/ru/post/402355/");
	}
}
