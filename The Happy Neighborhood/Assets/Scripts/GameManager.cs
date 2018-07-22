using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject WaitingPanel;
    public GameObject RoomIsFullPanel;
    public GameObject StartTheGamePanel;
    public Text UserNameText;

	// Use this for initialization
	void Start ()
    {
        WaitingPanel.SetActive(false);
        RoomIsFullPanel.SetActive(false);
        StartTheGamePanel.SetActive(false);

    }

    public void ShowWaitingRoom()
    {
        WaitingPanel.SetActive(true);
        RoomIsFullPanel.SetActive(false);
        StartTheGamePanel.SetActive(false);
    }

    public void ShowRoomIsFull()
    {
        WaitingPanel.SetActive(false);
        RoomIsFullPanel.SetActive(true);
        StartTheGamePanel.SetActive(false);
    }

    public void StartTheGame()
    {
        WaitingPanel.SetActive(false);
        RoomIsFullPanel.SetActive(false);
        StartTheGamePanel.SetActive(true);
        UserNameText.text =  PlayerPrefs.GetString(MenuManager.UserNamePlayerPrefs);
    }

}
