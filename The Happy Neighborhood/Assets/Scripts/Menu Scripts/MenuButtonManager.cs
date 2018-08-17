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
    private SoundManager soundManager;

    private void Start()
    {
        menuManager = GetComponent<MenuManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    #region StartGame()
    /// <summary>
    /// Start the game
    /// </summary>
    public void StartGame()
    {
        soundManager.SFX_MenuButtonPlay();
        StopAllCoroutines();
        StartCoroutine(DelayLoadScene(1, 0.4f));
    }
    #endregion

    #region SaveNameBtn()
    /// <summary>
    /// Save Name In the Pop-Up panel at the begining of game
    /// </summary>
    public void SaveNameBtn()
    {
        soundManager.SFX_MenuButtonPlay();


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
            soundManager.SFX_ErrorPlay();
        }
    }
    #endregion

    #region ShowOptionPanelBtn()
    /// <summary>
    /// Button To show Option Panel
    /// </summary>
    public void ShowOptionPanelBtn()
    {
        soundManager.SFX_MenuButtonPlay();
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


    public void ShowTutorial()
    {
        soundManager.SFX_MenuButtonPlay();
        menuManager.OptionPanel.SetActive(false);
        menuManager.TutorialPanel.SetActive(true);
    }

    public void OKToHideTutorial()
    {
        soundManager.SFX_MenuButtonPlay();
        menuManager.OptionPanel.SetActive(false);
        menuManager.PopupPanelForName.SetActive(false);
        menuManager.TutorialPanel.SetActive(false);

        menuManager.MainMenuPanel.SetActive(true);

    }


    #region ExitGameBtn()
    /// <summary>
    /// Exit the game
    /// </summary>
    public void ExitGameBtn()
    {
        soundManager.SFX_ErrorPlay();
        StopAllCoroutines();
        StartCoroutine(DelayExit(0.4f));
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
            soundManager.SFX_MenuButtonPlay();
            PlayerPrefs.SetString(MenuManager.UserNamePlayerPrefs, UserName);
            return true;
        }
        else
        {
            soundManager.SFX_ErrorPlay();
            NameEditWarningText.text = "WARNING: Wrong Input!";
            return false;
        }
    }
    #endregion



    #region DelayLoadScene(int SceneIndex, float WaitingTime) [Coroutine]
    /// <summary>
    /// Load a scene after given delay [use for playing SFX befor starting the game]
    /// </summary>
    /// <param name="SceneIndex"></param>
    /// <param name="WaitingTime"></param>
    /// <returns></returns>
    IEnumerator DelayLoadScene(int SceneIndex, float WaitingTime)
    {
        yield return new WaitForSeconds(WaitingTime);
        SceneManager.LoadScene(SceneIndex);
    }
    #endregion

    #region DelayExit(float WaitingTime)  [Coroutine]
    /// <summary>
    /// Exit the game after given delay [use for playing SFX befor exiting the game]
    /// </summary>
    /// <param name="WaitingTime"></param>
    /// <returns></returns>
    IEnumerator DelayExit(float WaitingTime)
    {
        yield return new WaitForSeconds(WaitingTime);
        Application.Quit();
    }
    #endregion
}
