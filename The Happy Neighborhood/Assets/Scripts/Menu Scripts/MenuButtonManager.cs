using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonManager : MonoBehaviour
{

    public InputField NameSaveInput;
    public InputField NameEditInput;
    public Text NameSaveWarningText;
    public Text NameEditWarningText;

    private MenuManager menuManager;


    private void Start()
    {
        menuManager = GetComponent<MenuManager>();
    }

    #region StartGame()
    /// <summary>
    /// Start the game
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    #endregion

    #region SaveNameBtn()
    /// <summary>
    /// Save Name In the Pop-Up panel at the begining of game
    /// </summary>
    public void SaveNameBtn()
    {
        string UserName = "";

        UserName = NameSaveInput.text.Trim();

        if (UserName != "")
        {
            PlayerPrefs.SetString(MenuManager.UserNamePlayerPrefs, UserName);
            GetComponent<MenuManager>().CheckNameForFirstTime();
        }
        else
        {
            NameSaveWarningText.text = "WARNING: This field can't be empty";
        }
    }
    #endregion

    #region ShowOptionPanelBtn()
    /// <summary>
    /// Button To show Option Panel
    /// </summary>
    public void ShowOptionPanelBtn()
    {
        menuManager.MainMenuPanel.SetActive(false);
        menuManager.OptionPanel.SetActive(true);
        NameEditInput.text = PlayerPrefs.GetString(MenuManager.UserNamePlayerPrefs);
    }
    #endregion

    #region SaveAndBackToMainMenubtn()
    /// <summary>
    /// Button that back to main menu
    /// </summary>
    public void SaveAndBackToMainMenubtn()
    {
        if(EditName())
        {
            menuManager.MainMenuPanel.SetActive(true);
            menuManager.OptionPanel.SetActive(false);
        }
    }
    #endregion

    #region ExitGameBtn()
    /// <summary>
    /// Exit the game
    /// </summary>
    public void ExitGameBtn()
    {
        Application.Quit();
    }
    #endregion

    #region EditName()
    /// <summary>
    /// Edit Name and save in Option Panel
    /// </summary>
    public bool EditName()
    {
        string UserName = "";

        UserName = NameEditInput.text.Trim();

        if (UserName != "")
        {
            PlayerPrefs.SetString(MenuManager.UserNamePlayerPrefs, UserName);
            return true;
        }
        else
        {
            NameEditWarningText.text = "WARNING: Wrong Input!";
            return false;
        }
    }
    #endregion

}
