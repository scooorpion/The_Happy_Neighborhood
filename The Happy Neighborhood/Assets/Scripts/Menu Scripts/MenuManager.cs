using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static string UserNamePlayerPrefs = "UserName";
    public GameObject PopupPanelForName;
    public GameObject MainMenuPanel;
    public GameObject OptionPanel;

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        CheckNameForFirstTime();

    }

    public void CheckNameForFirstTime()
    {
        if (PlayerPrefs.HasKey(UserNamePlayerPrefs))
        {
            PopupPanelForName.SetActive(false);
            MainMenuPanel.SetActive(true);
            OptionPanel.SetActive(false);
        }
        else
        {
            PopupPanelForName.SetActive(true);
            MainMenuPanel.SetActive(false);
            OptionPanel.SetActive(false);

        }
    }
}
