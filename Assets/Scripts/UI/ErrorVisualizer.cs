using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorVisualizer : MonoBehaviour
{
	public Image img;
	public Text text;

	private const float speed = 0.8f;
	
	// Update is called once per frame
	void Update ()
	{
		Color c = gameObject.GetComponent<Image>().color;
		Color cText = text.color;
		float a = c.a;
		
		if (c.a <= 0)
		{
			gameObject.GetComponent<Image>().color = new Color(c.r, c.g, c.b, 1f);		
			img.color = new Color(c.r, c.g, c.b, 1f);
			text.color = new Color(cText.r, cText.g, cText.b, 1f);
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.GetComponent<Image>().color = new Color(c.r, c.g, c.b, a - speed * Time.deltaTime);
			img.color = new Color(c.r, c.g, c.b, a - speed * Time.deltaTime);
			text.color = new Color(cText.r, cText.g, cText.b, a - speed * Time.deltaTime);
		}
	}
}
