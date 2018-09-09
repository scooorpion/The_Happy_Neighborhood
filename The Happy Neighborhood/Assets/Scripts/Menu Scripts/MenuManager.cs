using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static string UserNamePlayerPrefs = "UserName";
    public GameObject PopupPanelForName;
    public GameObject MainMenuPanel;
    public GameObject OptionPanel;
    public GameObject TutorialPanel;
    public GameObject[] TutorialPages;
    public GameObject CreditPanel;

    public GameObject Cloud;
    public GameObject Wall;
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        Initialization();
        CheckNameForFirstTime();

    }

    void Initialization()
    {
        TutorialPanel.SetActive(false);
        OptionPanel.SetActive(false);
        
        PopupPanelForName.SetActive(false);
        CreditPanel.SetActive(false);
        for (int i = 0; i < TutorialPages.Length; i++)
        {
            TutorialPages[i].SetActive(false);
        }

    }

    public void CheckNameForFirstTime()
    {

        if (PlayerPrefs.HasKey(UserNamePlayerPrefs))
        {
            MainMenuPanel.SetActive(true);
            PopupPanelForName.SetActive(false);
        }
        else
        {
            PopupPanelForName.SetActive(true);
        }
    }
}
