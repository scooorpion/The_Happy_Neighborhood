using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region CharactersType Enum
public enum CharactersType
{
    Empty,
    NoramlGuy,
    DoubleGuys,
    TripleGuys,
    RedGuy,
    RedNoBlueGuy,
    BlueGuy,
    BlueNoYellow,
    PurpuleGuy,
    PurpleNoRedGuy,
    OldGuy,
    PenthouseGuy,
    TwoHouseGuy,
    ThreeHouseLGuy,
    FourHouseGuy,
    Ghost,
    FamilyTwoGuys,
    Animal,
    GuyWithAnimal,
    GuyNeedParking,
    GuyNeedGarden,
    Baby,
    GhostCatcher,
    Gangster,
    Wizard
}
#endregion

#region HouseCellsType Enum
public enum HouseCellsType
{
    BannedTile,
    EmptyTile,
    BlueTile,
    RedTile,
    PurpleTile,
    YellowTile,
    OldBlueTile,
    OldRedTile,
    OldPurpleTile,
    OldYellowTile,
    PentHouse,
    Parking,
    Terrace,
    Garden,
}
#endregion

public class GameManager : MonoBehaviour {

    public Animator UIAnimator;
    public GameObject WaitingPanel;
    public GameObject ServerFullPanel;
    public GameObject GameLoadingPanel;

    public GameObject MyBoard;
    public GameObject EnemyBoard;

    private BoardGenerator myBoardGenerator;
    private BoardGenerator myEnemyBoardGenerator;


    private



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
            UIAnimator.SetBool("serverFull", false);
            UIAnimator.SetBool("waiting", false);
            UIAnimator.SetBool("loadGame", false);
        }
        SetDiactiveUIBeginingWaitingPanel();
        MyBoard.SetActive(false);
        EnemyBoard.SetActive(false);
        myBoardGenerator = MyBoard.GetComponent<BoardGenerator>();
        myEnemyBoardGenerator = EnemyBoard.GetComponent<BoardGenerator>();


    }
    #endregion

    #region SetDiactiveUIBeginingWaitingPanel()
    /// <summary>
    /// DeActive 3 Panels in UI which show pre-game state at begining(wain,server full,loading)
    /// </summary>
    public void SetDiactiveUIBeginingWaitingPanel()
    {
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
        StartGame();
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

    #region StartGame()
    public void StartGame()
    {
        MyBoard.SetActive(true);
        EnemyBoard.SetActive(true);
    }
    #endregion

    # region SetUserNames(string MyName,string EnemyName)
    /// <summary>
    /// Set User Name and Enemy Name in UI TextBox
    /// </summary>
    /// <param name="MyName"></param>
    /// <param name="EnemyName"></param>
    public void SetUserNames(string MyName,string EnemyName)
    {
        myBoardGenerator.UserNameTextBox.text = MyName;
        myEnemyBoardGenerator.UserNameTextBox.text = EnemyName;
    }
    #endregion


}
