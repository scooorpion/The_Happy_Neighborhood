using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Animator UIAnimator;
    public GameObject WaitingPanel;
    public GameObject ServerFullPanel;
    public GameObject GameLoadingPanel;

    

    void Awake()
    {
        Initialazation();
    }

    #region Initialazation(bool ToReset = false)
    /// <summary>
    /// Initialazation values for starting the game
    /// </summary>
    /// <param name="ToReset">If is true, it is for reseting after disconnecting not first initialazation</param>
    public void Initialazation(bool ToReset = false)
    {
        if (ToReset)
        {
            UIAnimator.SetBool("Default", true);
            UIAnimator.SetBool("serverFull", false);
            UIAnimator.SetBool("waiting", false);
            UIAnimator.SetBool("loadGame", false);
        }

        WaitingPanel.SetActive(false);
        ServerFullPanel.SetActive(false);
        GameLoadingPanel.SetActive(false);
    }
    #endregion

    #region ShowGameLoading()
    /// <summary>
    /// Show animation for loading game
    /// </summary>
    public void ShowGameLoading()
    {
        GameLoadingPanel.SetActive(true);
        UIAnimator.SetBool("loadGame", true);
    }
    #endregion

    #region ShowWaitingAnimation()
    /// <summary>
    /// Show animation for waiting for other client
    /// </summary>
    public void ShowWaitingAnimation()
    {
        WaitingPanel.SetActive(true);
        UIAnimator.SetBool("waiting", true);
    }
    #endregion

    #region ShowRoomIsFull()
    /// <summary>
    /// Show animation to tell server is full
    /// </summary>
    public void ShowRoomIsFull()
    {
        ServerFullPanel.SetActive(true);
        UIAnimator.SetBool("serverFull", true);
    }
    #endregion



}
