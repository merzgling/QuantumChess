using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject MainMenu;
    
    public GameObject CreateGame;
    public GameObject FindGame;
    public GameObject HowToPlay;
    public GameObject Credits;

    public void OnCreateButtonPressed()
    {
        
    }
    
    public void OnFindGameButtonPressed()
    {
        
    }
    
    public void OnCreditsButtonPressed()
    {
        
    }
    
    public void OnExitButtonPressed()
    {
        Application.Quit();
    }

    public void OnChangeColorButtonPressed(GameObject gameObject)
    {
        /*
        if (gameObject.GetComponent<Image>().color == new Color(0, 0, 0))
            gameObject.GetComponent<Image>().color = new Color(255, 255, 255);
        else
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0);*/
    }

    public void SetActiveMainMenu()
    {
        MainMenu.SetActive(true);
    }
}
