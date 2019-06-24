using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject WhiteWinPanel;
    public GameObject BlackWinPanel;
    public GameObject WhiteTurnImage;
    public GameObject BlackTurnImage;

    public GameObject InvalidCommonMovePanel;
    public GameObject InvalidQuantumMovePanel;
    
    public void OnPauseButtonPressed()
    {
        PauseMenu.SetActive(true);
    }

    public void OnResumeButtonPressed()
    {
        PauseMenu.SetActive(false);
    }
    
    public void OnExitButtonPressed()
    {
        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().addPlayerButton.SetActive(true);
        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().GoBackButton();
        GameObject.Find("Menu UI").GetComponent<UIController>().SetActiveMainMenu(true);
        //Application.LoadLevel(0);
    }

    public void OnSurrenderButtonPressed()
    {
        GameObject.Find("Game controller").GetComponent<GameController>().OnSurrender();
    }

    public void OnWhiteWin()
    {
        WhiteWinPanel.SetActive(true);
    }

    public void OnBlackWin()
    {
        BlackWinPanel.SetActive(true);
    }

    public void SetWhiteTurn()
    {
        WhiteTurnImage.SetActive(true);
        BlackTurnImage.SetActive(false);
    }

    public void SetBlackTurn()
    {
        WhiteTurnImage.SetActive(false);
        BlackTurnImage.SetActive(true);
    }

    public void SayInvalidCommonMove()
    {
        Debug.LogError("invalid common move!");
        InvalidCommonMovePanel.SetActive(true);
    }

    public void SayInvalidQuantumMove()
    {
        Debug.LogError("Invalid quantum move");
        InvalidQuantumMovePanel.SetActive(true);
    }
}
