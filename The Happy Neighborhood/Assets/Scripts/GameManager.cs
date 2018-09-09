using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    TwoHouseGuy_Up,
    TwoHouseGuy_Down,
    ThreeHouseLGuy,
    ThreeHouseLGuy_Up,
    ThreeHouseLGuy_Center,
    ThreeHouseLGuy_Down_Right,
    FourHouseGuy,
    FourHouseGuy_Up_Left,
    FourHouseGuy_Up_Right,
    FourHouseGuy_Down_Left,
    FourHouseGuy_Down_Right,
    Ghost,
    FamilyTwoGuys,
    Animal,
    GuyWithAnimal,
    GuyNeedParking,
    GuyNeedGarden,
    Baby,
    GhostCatcher,
    Gangster,
    Wizard,

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

public class GameManager : MonoBehaviour
{
    public Animator UIAnimator;
    public GameObject WaitingPanel;
    public GameObject ServerFullPanel;
    public GameObject ErrorMessagePanel;
    public GameObject GameLoadingPanel;
    public GameObject ConnectionLostPanel;
    public GameObject CharacterDeck;
    public GameObject HouseDeck;
    public GameObject NetworkHudBtns;

    public Text ConnctionLostPlayer;
    public GameObject FinishedPanel;

    public Image WinnerIconReference;
    public Sprite LoserIcon;
    public Text WinnerLableTxt;
    public Text WitchWinnerText;

    public GameObject WinnerName;
    public GameObject WinnerScore;
    public GameObject WinnerPpoint;
    public GameObject WinnerNpoint;
    public GameObject LoserName;
    public GameObject LoserScore;
    public GameObject LoserPpoint;
    public GameObject LoserNpoint;
    public GameObject DeathPoint;

    //public GameObject DeathPlacePrefab;

    public GameObject DeathPlaceParent;
    public GameObject[] DeathCharPlaces;
    public Sprite DefaultDeathSprite;

    public GameObject MyBoard;
    public GameObject EnemyBoard;

    public Sprite[] ErrorMessageSprites;

    private BoardGenerator myBoardGenerator;
    private BoardGenerator myEnemyBoardGenerator;
    private SoundManager soundManager;
    private NetworkButtonManager networkButtonManager;

    private static int[] LeftEdge = new int[] { 0, 7, 14, 21, 28, 35, 42 };
    private static int[] RightEdge = new int[] { 6, 13, 20, 27, 34, 41, 48 };

    Vector2 GhostOriginPoint;
    Vector2 GhostDestinPoint;

    GameObject GhostCardToFly;
    public GameObject GhostPrefab;
    public Transform MyGhostParent;
    public Transform EnemyGhostParent;

    private static int  DeathNumber = 0;
    public static string enemyName;



    [SerializeField]
    public SpriteReference spriteReference;

    private void Awake()
    {
        Initialazation();
    }

    #region VisibleCursor(float waitingTime): Wait To Show Cursor at the begining
    IEnumerator VisibleCursor(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        Cursor.visible = true;
    }
    #endregion

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

            UIAnimator.SetBool("Find_Waiting", false);
            UIAnimator.SetBool("Create_waiting", false);

            UIAnimator.SetBool("loadGame", false);
        }
        StartCoroutine(VisibleCursor(0.4f));
        ConnectionLostPanel.SetActive(false);
        SetDiactiveUIBeginingWaitingPanel();
        CharacterDeck.SetActive(false);
        HouseDeck.SetActive(false);
        MyBoard.SetActive(false);
        EnemyBoard.SetActive(false);
        myBoardGenerator = MyBoard.GetComponent<BoardGenerator>();
        myEnemyBoardGenerator = EnemyBoard.GetComponent<BoardGenerator>();
        networkButtonManager = GameObject.FindObjectOfType<NetworkButtonManager>();
        myBoardGenerator.TurnPanel.SetActive(false);
        myEnemyBoardGenerator.TurnPanel.SetActive(false);
        myBoardGenerator.GhostPanel.SetActive(false);
        myEnemyBoardGenerator.GhostPanel.SetActive(false);
        ErrorMessagePanel.SetActive(false);
        NetworkHudBtns.SetActive(true);
        FinishedPanel.SetActive(false);
        DeathPlaceParent.SetActive(false);
        soundManager = FindObjectOfType<SoundManager>();
        spriteReference.FirstHouseSpriteRandomInitialization();
        WinnerLableTxt.text = "The Winner";
        WitchWinnerText.text = "";

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
       // GameLoadingPanel.SetActive(true);
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
        //WaitingPanel.SetActive(true);
        //UIAnimator.SetBool("waiting", true);
        print("Loadiiing");

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
        CharacterDeck.SetActive(true);
        HouseDeck.SetActive(true);
        DeathPlaceParent.SetActive(true);
    }
    #endregion

    # region SetMyName(string MyName)
    /// <summary>
    /// Set User Name  in UI TextBox
    /// </summary>
    /// <param name="MyName"></param>
    public void SetMyName(string MyName)
    {
        myBoardGenerator.UserNameTextBox.text = MyName;
        myBoardGenerator.UserNameTextBox.fontStyle = FontStyle.Bold;
    }
    #endregion

    #region SetEnemyName(string EnemyName)
    /// <summary>
    /// Set Enemy Name in UI TextBox
    /// </summary>
    /// <param name="EnemyName"></param>
    public void SetEnemyName(string EnemyName)
    {
        myEnemyBoardGenerator.UserNameTextBox.text = EnemyName;
        myEnemyBoardGenerator.UserNameTextBox.fontStyle = FontStyle.Normal;

    }
    #endregion

    public void DisableDecks()
    {
        for (int i = 0; i < 4; i++)
        {
            if (HouseDeckManager.HousesCardsDeckPickable[i])
                HouseDeckManager.HousesCardsDeckPickable[i].GetComponent<Button>().interactable = false;


            if (CharDeckManager.CharacterCardsDeckPickable[i])
                CharDeckManager.CharacterCardsDeckPickable[i].GetComponent<Button>().interactable = false;
        }

        myBoardGenerator.GhostPanel.GetComponent<Button>().interactable = false;
    }

    public void EnableDecks()
    {
        for (int i = 0; i < 4; i++)
        {
            if (HouseDeckManager.HousesCardsDeckPickable[i])
                HouseDeckManager.HousesCardsDeckPickable[i].GetComponent<Button>().interactable = true;

            if (CharDeckManager.CharacterCardsDeckPickable[i])
                CharDeckManager.CharacterCardsDeckPickable[i].GetComponent<Button>().interactable = true;
        }

        myBoardGenerator.GhostPanel.GetComponent<Button>().interactable = true;


    }

    public void HighlightPlayerNameWhoHasTheTurn(bool IsItMyTurn)
    {
        if (IsItMyTurn)
        {
            //myBoardGenerator.UserNameTextBox.color = Color.black;
            //myBoardGenerator.scoreText.color = Color.black;
            //myBoardGenerator.ScoreLable.color = Color.black;

            //myEnemyBoardGenerator.UserNameTextBox.color = Color.gray;
            //myEnemyBoardGenerator.scoreText.color = Color.gray;
            //myEnemyBoardGenerator.ScoreLable.color = Color.gray;

            myBoardGenerator.TurnPanel.SetActive(true);
            myEnemyBoardGenerator.TurnPanel.SetActive(false);
        }
        else
        {
            //myBoardGenerator.UserNameTextBox.color = Color.gray;
            //myBoardGenerator.scoreText.color = Color.gray;
            //myBoardGenerator.ScoreLable.color = Color.gray;

            //myEnemyBoardGenerator.UserNameTextBox.color = Color.black;
            //myEnemyBoardGenerator.scoreText.color = Color.black;
            //myEnemyBoardGenerator.ScoreLable.color = Color.black;

            myBoardGenerator.TurnPanel.SetActive(false);
            myEnemyBoardGenerator.TurnPanel.SetActive(true);

        }
    }

    public void UpdatePlayersScore(int Score, bool IsItMyTurn)
    {
        if (IsItMyTurn)
        {
            myBoardGenerator.scoreText.text = Score.ToString();
        }
        else
        {
            myEnemyBoardGenerator.scoreText.text = Score.ToString();
        }
    }

    // New Functions: 

    public void ShowDeathChar(CharactersType[] DeadCharacter, int DeadCharsNumber)
    {
        PlayerConnection myConn = GameObject.FindGameObjectWithTag("MyConnection").GetComponent<PlayerConnection>();

        DeathNumber = DeadCharsNumber;

        soundManager.SFX_DeathPlay();

        UIAnimator.SetInteger("DeathNumber", DeadCharsNumber);

        for (int i = 0; i < 5; i++)
        {
            DeathCharPlaces[i].GetComponent<Image>().sprite = DefaultDeathSprite;
        }

        for (int i = 0; i < DeadCharsNumber; i++)
        {
            DeathCharPlaces[i].GetComponent<Image>().sprite = SpriteBasedOnDeathCharacter(DeadCharacter[i]);
        }

        //foreach (Transform child in DeathPlaceParent.transform)
        //{
        //    Destroy(child.gameObject);
        //}

        //for (int i = 0; i < DeadCharsNumber; i++)
        //{
        //    GameObject DeathPlace = Instantiate(DeathPlacePrefab);
        //    DeathPlace.GetComponent<Transform>().SetParent(DeathPlaceParent.transform, false);
        //    DeathPlace.GetComponent<DeathImage>().PlaceDeathCharInPic(SpriteBasedOnDeathCharacter(DeadCharacter[i]));

        //}




        #region When a character dies, The seleced character should be checked not to be removed, if removed, then empty seleted card
        int Counter = 0;
        for (int i = 0; i < 4; i++)
        {
            if (CharDeckManager.CharacterCardsDeckPickable[i].GetComponent<CharType>().charactersType == myConn.CharacterdCardSelected)
            {
                Counter ++;
                break;
            }
        }

        if(Counter == 0)
        {
            myConn.CardTypeSelected = CardType.NoSelection;
        }
        #endregion
    }

    public void UpdateHouseTileMap(HouseCellsType[] HouseCellsArray, bool IsMyBoard = true)
    {

        if (IsMyBoard)
        {
            for (int i = 0; i < HouseCellsArray.Length; i++)
            {
                myBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Image>().sprite = SpriteBasedOnHouseCellType(HouseCellsArray[i]);


                if (HouseCellsArray[i] == HouseCellsType.BannedTile)
                {
                    myBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Button>().interactable = false;
                    myBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Image>().sprite = SpriteBasedOnHouseCellType(HouseCellsType.BannedTile);
                    myBoardGenerator.BoardCellsArray[i / 7, i % 7].tag = "Banned_Cell";

                }

                if (HouseCellsArray[i] != HouseCellsType.EmptyTile)
                {
                    myBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Image>().color = new Color(1, 1, 1, 1);

                    if (HouseCellsArray[i] != HouseCellsType.BannedTile)
                        myBoardGenerator.BoardCellsArray[i / 7, i % 7].tag = "Full_Cell";

                }

            }
        }
        else if (!IsMyBoard)
        {
            for (int i = 0; i < HouseCellsArray.Length; i++)
            {
                myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<CellIndex>().CellType = BoardType.EnemyBoard;
                myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Image>().sprite = SpriteBasedOnHouseCellType(HouseCellsArray[i]);
                myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Button>().interactable = false;

                if (HouseCellsArray[i] != HouseCellsType.EmptyTile)
                {
                    myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }

                if (HouseCellsArray[i] == HouseCellsType.BannedTile)
                {
                    myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].tag = "BannedHouse";
                }

                

            }

        }
    }

    public void UpdateCharacterTileMap(int GhostAttackedIndex, bool IsMyBoard = true)
    {
        Image CharacterPlaceHolderImageComponenet;
        if (IsMyBoard)
        {
            CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[GhostAttackedIndex / 7, GhostAttackedIndex % 7].transform.GetChild(0).GetComponent<Image>();
        }
        else
        {
            CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[GhostAttackedIndex / 7, GhostAttackedIndex % 7].transform.GetChild(0).GetComponent<Image>();
        }

        CharacterPlaceHolderImageComponenet.sprite = SpriteBasedOnCharacterCellType(CharactersType.Ghost);

        soundManager.SFX_GhostPlacedInHouse();

        Color tempColor = CharacterPlaceHolderImageComponenet.color;
        tempColor.a = 1;
        CharacterPlaceHolderImageComponenet.color = tempColor;

    }

    public void UpdateCharacterTileMap(CharactersType[] CharacterCellsArray, HouseCellsType[] HouseCellsArray, bool IsMyBoard = true)
    {
        if (IsMyBoard)
        {
            Image CharacterPlaceHolderImageComponenet;

            for (int i = 0; i < CharacterCellsArray.Length; i++)
            {
                if (CharacterCellsArray[i] != CharactersType.Empty)
                {
                    if (CharacterCellsArray[i] == CharactersType.TwoHouseGuy_Up)
                    {
                        CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("DownCenter").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.TwoHouseGuy_Down)
                    {
                        CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("UpCenter").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.FourHouseGuy_Down_Left)
                    {
                        CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("UpRight").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.FourHouseGuy_Down_Right)
                    {
                        CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("UpLeft").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.FourHouseGuy_Up_Left)
                    {
                        CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("DownRight").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.FourHouseGuy_Up_Right)
                    {
                        CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("DownLeft").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.ThreeHouseLGuy_Up)
                    {
                        CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("DownRight").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.ThreeHouseLGuy_Center)
                    {
                        CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("UpRight").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.ThreeHouseLGuy_Down_Right)
                    {
                        CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("UpLeft").GetComponent<Image>();
                    }
                    else
                    {
                        CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("DownLeft").GetComponent<Image>();
                    }

                    Color tempColor;

                    CharacterPlaceHolderImageComponenet.sprite = SpriteBasedOnCharacterCellType(CharacterCellsArray[i]);

                    tempColor = CharacterPlaceHolderImageComponenet.color;
                    tempColor.a = 1;
                    CharacterPlaceHolderImageComponenet.color = tempColor;
                }
            }
        }
        else if (!IsMyBoard)
        {
            Image CharacterPlaceHolderImageComponenet;
            for (int i = 0; i < CharacterCellsArray.Length; i++)
            {
                if (CharacterCellsArray[i] != CharactersType.Empty)
                {

                    if (CharacterCellsArray[i] == CharactersType.TwoHouseGuy_Up)
                    {
                        CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("DownCenter").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.TwoHouseGuy_Down)
                    {
                        CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("UpCenter").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.FourHouseGuy_Down_Left)
                    {
                        CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("UpRight").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.FourHouseGuy_Down_Right)
                    {
                        CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("UpLeft").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.FourHouseGuy_Up_Left)
                    {
                        CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("DownRight").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.FourHouseGuy_Up_Right)
                    {
                        CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("DownLeft").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.ThreeHouseLGuy_Up)
                    {
                        CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("DownRight").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.ThreeHouseLGuy_Center)
                    {
                        CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("UpRight").GetComponent<Image>();
                    }
                    else if (CharacterCellsArray[i] == CharactersType.ThreeHouseLGuy_Down_Right)
                    {
                        CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("UpLeft").GetComponent<Image>();
                    }
                    else
                    {
                        CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.Find("DownLeft").GetComponent<Image>();
                    }


                    CharacterPlaceHolderImageComponenet.sprite = SpriteBasedOnCharacterCellType(CharacterCellsArray[i]);

                    CharacterPlaceHolderImageComponenet.color = CharacterPlaceHolderImageComponenet.color = new Color(0.68f, 0.68f, 0.68f, 1);


                    myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Button>().interactable = false;

                }
            }

        }

    }


    public void UpdateHouseDeck(HouseCellsType[] HouseCellsArray)
    {
        for (int i = 0; i < 4; i++)
        {
            Image imageTempComponent = HouseDeckManager.HousesCardsDeckPickable[i].GetComponent<Image>();
            Color tempColor;

            tempColor = imageTempComponent.color;
            tempColor.a = 1;
            imageTempComponent.color = tempColor;

            // Error : IndexOutOfRange
            imageTempComponent.sprite = SpriteBasedOnHouseCellType(HouseCellsArray[i]);
            HouseDeckManager.HousesCardsDeckPickable[i].GetComponent<HouseType>().houseCellsType = HouseCellsArray[i];

        }
    }


    // To-Do : int[] CreationTime bayad daryaft beshe
    public void UpdateCharacterDeck(CharactersType[] CharactersType, float[] CreationTime, float ServerTime)
    {
        for (int i = 0; i < 4; i++)
        {

            Image imageTempComponent = CharDeckManager.CharacterCardsDeckPickable[i].GetComponent<Image>();
            Color tempColor;

            tempColor = imageTempComponent.color;
            tempColor.a = 1;
            imageTempComponent.color = tempColor;

            imageTempComponent.sprite = SpriteBasedOnCharacterCellType(CharactersType[i]);
            CharDeckManager.CharacterCardsDeckPickable[i].GetComponent<CharType>().charactersType = CharactersType[i];
        }
    }

    public void UpdateCharactersHealthBar(float[] HealthBars, CharactersType[] Characters)
    {
        
        for (int i = 0; i < Characters.Length; i++)
        {
            for (int j = 0; j < CharDeckManager.CharacterCardsDeckPickable.Length; j++)
            {
                if (CharDeckManager.CharacterCardsDeckPickable[j].GetComponent<CharType>().charactersType == Characters[i])
                {
                    CharDeckManager.CharacterCardsDeckPickable[j].GetComponentInParent<CharacterLife>().UpdateCharactersHealthBar(HealthBars[i]);
                    break;
                }

            }
        }
        
    }

    IEnumerator GhostFlying(float FlyingSpeed, int GhostNumber, bool IsItFirstTimeGhostIn)
    {
       
        while (Vector2.Distance(GhostCardToFly.GetComponent<RectTransform>().anchoredPosition, GhostDestinPoint) > 10)
        {
            GhostCardToFly.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GhostCardToFly.GetComponent<RectTransform>().anchoredPosition, GhostDestinPoint, Time.deltaTime* FlyingSpeed);
            yield return null;
        }
        

        GhostCardToFly.GetComponent<RectTransform>().anchoredPosition = GhostDestinPoint;
        if(IsItFirstTimeGhostIn)
        {
            myBoardGenerator.GhostPanel.GetComponent<Image>().sprite = spriteReference.GhostInsideSprite;
        }
        myBoardGenerator.GhostNumberText.text = GhostNumber.ToString();
        myBoardGenerator.GhostPanel.SetActive(true);
        GameObject.Destroy(GhostCardToFly);

        yield return null;

    }

    public void UpdateGhost(int Ghosts, bool IsItMyBoard, int SelectedIndex, bool IsGhostIncreasing)
    {

        if (IsItMyBoard)
        {
            if(Ghosts > 0)
            {
                if(IsGhostIncreasing)
                {
                    bool IsItFirstTimeGhostWantToEnterTheHouse = false;

                    if (myBoardGenerator.GhostPanel.active == true)
                    {
                        IsItFirstTimeGhostWantToEnterTheHouse = false;
                        //myBoardGenerator.GhostPanel.GetComponent<Image>().sprite = spriteReference.GhostInsideSprite;
                    }
                    else if (myBoardGenerator.GhostPanel.active == false)
                    {
                        IsItFirstTimeGhostWantToEnterTheHouse = true;
                        myBoardGenerator.GhostPanel.GetComponent<Image>().sprite = spriteReference.GhostNotInsideSprite;
                    }

                    GhostCardToFly = Instantiate<GameObject>(GhostPrefab);
                    GhostCardToFly.GetComponent<Transform>().SetParent(MyGhostParent.transform, false);
                    GhostCardToFly.GetComponent<RectTransform>().anchoredPosition = myBoardGenerator.BoardCellsArray[SelectedIndex / 7, SelectedIndex % 7].GetComponent<RectTransform>().anchoredPosition;

                    GhostDestinPoint = myBoardGenerator.GhostPanel.GetComponent<RectTransform>().anchoredPosition;

                    myBoardGenerator.GhostPanel.SetActive(true);

                    // PlaySFX For Ghost ----------->>>>>
                    soundManager.SFX_GhostFlyPlay();
                    StartCoroutine(GhostFlying(3, Ghosts, IsItFirstTimeGhostWantToEnterTheHouse));

                }
                else
                {
                    myBoardGenerator.GhostNumberText.text = Ghosts.ToString();
                    myBoardGenerator.GhostPanel.SetActive(true);

                }
            }
            else
            {
                myBoardGenerator.GhostPanel.SetActive(false);

            }
        }
        else
        {
            if (Ghosts > 0)
            {
                myEnemyBoardGenerator.GhostNumberText.text = Ghosts.ToString();
                myEnemyBoardGenerator.GhostPanel.SetActive(true);
            }
            else
            {
                myEnemyBoardGenerator.GhostPanel.SetActive(false);

            }

        }
    }


    public void HideconnectionHUDPanel()
    {
        NetworkHudBtns.SetActive(false);
    }

    #region Sprite SpriteBasedOnHouseCellType(HouseCellsType HouseCellType)
    /// <summary>
    /// Return a Sprite based on the enum house cell type [Update Card on screen]
    /// </summary>
    /// <param name="HouseCellType"></param>
    /// <returns></returns>
    public Sprite SpriteBasedOnHouseCellType(HouseCellsType HouseCellType)
    {
        Sprite tempHouseSprite;

        switch (HouseCellType)
        {
            case HouseCellsType.BannedTile:
                tempHouseSprite = spriteReference.BannedTile;
                break;

            case HouseCellsType.EmptyTile:
                tempHouseSprite = spriteReference.EmptyTile;
                break;

            case HouseCellsType.BlueTile:
                tempHouseSprite = spriteReference.BlueTile;
                break;

            case HouseCellsType.RedTile:
                tempHouseSprite = spriteReference.RedTile;
                break;

            case HouseCellsType.PurpleTile:
                tempHouseSprite = spriteReference.PurpleTile;
                break;

            case HouseCellsType.YellowTile:
                tempHouseSprite = spriteReference.YellowTile;
                break;

            case HouseCellsType.OldBlueTile:
                tempHouseSprite = spriteReference.OldBlueTile;
                break;

            case HouseCellsType.OldRedTile:
                tempHouseSprite = spriteReference.OldRedTile;
                break;

            case HouseCellsType.OldPurpleTile:
                tempHouseSprite = spriteReference.OldPurpleTile;
                break;

            case HouseCellsType.OldYellowTile:
                tempHouseSprite = spriteReference.OldYellowTile;
                break;

            case HouseCellsType.PentHouse:
                tempHouseSprite = spriteReference.PentHouse;
                break;

            case HouseCellsType.Parking:
                tempHouseSprite = spriteReference.Parking;
                break;

            case HouseCellsType.Terrace:
                print("Terrace");
                tempHouseSprite = spriteReference.Terrace;
                break;

            case HouseCellsType.Garden:
                tempHouseSprite = spriteReference.Garden;
                break;

            default:
                tempHouseSprite = spriteReference.EmptyTile;
                break;
        }

        return tempHouseSprite;
    }
    #endregion

    #region Sprite SpriteBasedOnCharacterCellType(CharactersType CharactersType)
    /// <summary>
    /// Return a Sprite based on the enum character cell type
    /// </summary>
    /// <param name="CharactersType"></param>
    /// <returns></returns>
    public Sprite SpriteBasedOnCharacterCellType(CharactersType CharactersType)
    {
        Sprite tempCharacterSprite;

        switch (CharactersType)
        {
            case CharactersType.Empty:
                tempCharacterSprite = spriteReference.Empty;
                break;
            case CharactersType.NoramlGuy:
                tempCharacterSprite = spriteReference.NoramlGuy;
                break;
            case CharactersType.DoubleGuys:
                tempCharacterSprite = spriteReference.DoubleGuys;
                break;
            case CharactersType.TripleGuys:
                tempCharacterSprite = spriteReference.TripleGuys;
                break;
            case CharactersType.RedGuy:
                tempCharacterSprite = spriteReference.RedGuy;
                break;
            case CharactersType.RedNoBlueGuy:
                tempCharacterSprite = spriteReference.RedNoBlueGuy;
                break;
            case CharactersType.BlueGuy:
                tempCharacterSprite = spriteReference.BlueGuy;
                break;
            case CharactersType.BlueNoYellow:
                tempCharacterSprite = spriteReference.BlueNoYellow;
                break;
            case CharactersType.PurpuleGuy:
                tempCharacterSprite = spriteReference.PurpuleGuy;
                break;
            case CharactersType.PurpleNoRedGuy:
                tempCharacterSprite = spriteReference.PurpleNoRedGuy;
                break;
            case CharactersType.OldGuy:
                tempCharacterSprite = spriteReference.OldGuy;
                break;
            case CharactersType.PenthouseGuy:
                tempCharacterSprite = spriteReference.PenthouseGuy;
                break;
            case CharactersType.TwoHouseGuy:
                tempCharacterSprite = spriteReference.TwoHouseGuy;
                break;
            case CharactersType.TwoHouseGuy_Down:
                tempCharacterSprite = spriteReference.TwoGuys_Down;
                break;
            case CharactersType.TwoHouseGuy_Up:
                tempCharacterSprite = spriteReference.TwoGuys_Up;
                break;
            case CharactersType.ThreeHouseLGuy:
                tempCharacterSprite = spriteReference.ThreeHouseLGuy;
                break;
            case CharactersType.ThreeHouseLGuy_Up:
                tempCharacterSprite = spriteReference.ThreeHouseLGuy_Up;
                break;
            case CharactersType.ThreeHouseLGuy_Center:
                tempCharacterSprite = spriteReference.ThreeHouseLGuy_Center;
                break;
            case CharactersType.ThreeHouseLGuy_Down_Right:
                tempCharacterSprite = spriteReference.ThreeHouseLGuy_Down_Left;
                break;

            case CharactersType.FourHouseGuy:
                tempCharacterSprite = spriteReference.FourHouseGuy;
                break;
            case CharactersType.FourHouseGuy_Down_Left:
                tempCharacterSprite = spriteReference.FourHouseGuy_Down_Left;
                break;
            case CharactersType.FourHouseGuy_Down_Right:
                tempCharacterSprite = spriteReference.FourHouseGuy_Down_Right;
                break;
            case CharactersType.FourHouseGuy_Up_Left:
                tempCharacterSprite = spriteReference.FourHouseGuy_Up_Left;
                break;
            case CharactersType.FourHouseGuy_Up_Right:
                tempCharacterSprite = spriteReference.FourHouseGuy_Up_Right;
                break;
            case CharactersType.Ghost:
                tempCharacterSprite = spriteReference.Ghost;
                break;
            case CharactersType.FamilyTwoGuys:
                tempCharacterSprite = spriteReference.FamilyTwoGuys;
                break;
            case CharactersType.Animal:
                tempCharacterSprite = spriteReference.Animal;
                break;
            case CharactersType.GuyWithAnimal:
                tempCharacterSprite = spriteReference.GuyWithAnimal;
                break;
            case CharactersType.GuyNeedParking:
                tempCharacterSprite = spriteReference.GuyNeedParking;
                break;
            case CharactersType.GuyNeedGarden:
                tempCharacterSprite = spriteReference.GuyNeedGarden;
                break;
            case CharactersType.Baby:
                tempCharacterSprite = spriteReference.Baby;
                break;
            case CharactersType.GhostCatcher:
                tempCharacterSprite = spriteReference.GhostCatcher;
                break;
            case CharactersType.Gangster:
                tempCharacterSprite = spriteReference.Gangster;
                break;
            case CharactersType.Wizard:
                tempCharacterSprite = spriteReference.Wizard;
                break;
            default:
                tempCharacterSprite = spriteReference.Empty;
                break;
        }

        return tempCharacterSprite;
    }

    #endregion

    #region Sprite SpriteBasedOnDeathCharacter(CharactersType CharactersType)
    /// <summary>
    /// Return a Sprite of death Cahracter based on the enum character cell type
    /// </summary>
    /// <param name="CharactersType"></param>
    /// <returns></returns>
    public Sprite SpriteBasedOnDeathCharacter(CharactersType CharactersType)
    {
        Sprite tempCharacterSprite;

        switch (CharactersType)
        {
            case CharactersType.NoramlGuy:
                tempCharacterSprite = spriteReference.NoramlGuy_Dead;
                break;
            case CharactersType.RedGuy:
                tempCharacterSprite = spriteReference.RedGuy_Dead;
                break;
            case CharactersType.RedNoBlueGuy:
                tempCharacterSprite = spriteReference.RedNoBlueGu_Dead;
                break;
            case CharactersType.BlueGuy:
                tempCharacterSprite = spriteReference.BlueGuy_Dead;
                break;
            case CharactersType.BlueNoYellow:
                tempCharacterSprite = spriteReference.BlueNoYellow_Dead;
                break;
            case CharactersType.PurpuleGuy:
                tempCharacterSprite = spriteReference.PurpuleGuy_Dead;
                break;
            case CharactersType.PurpleNoRedGuy:
                tempCharacterSprite = spriteReference.PurpleNoRedGuy_Dead;
                break;
            case CharactersType.OldGuy:
                tempCharacterSprite = spriteReference.OldGuy_Dead;
                break;
            case CharactersType.PenthouseGuy:
                tempCharacterSprite = spriteReference.PenthouseGuy_Dead;
                break;
            case CharactersType.TwoHouseGuy:
                tempCharacterSprite = spriteReference.TwoHouseGuy_Dead;
                break;
            case CharactersType.ThreeHouseLGuy:
                tempCharacterSprite = spriteReference.ThreeHouseLGuy_Dead;
                break;
            case CharactersType.FourHouseGuy:
                tempCharacterSprite = spriteReference.FourHouseGuy_Dead;
                break;
            default:
                tempCharacterSprite = spriteReference.Empty;
                break;
        }

        return tempCharacterSprite;
    }

    #endregion

    // ------------- Checking Index Functions -------------------

    #region TileIndex_Below(int SelectedIndex): Return The index of below house of entered index
    /// <summary>
    /// Return The index of below house of entered index
    /// </summary>
    /// <param name="SelectedIndex"></param>
    /// <returns></returns>
    public static int TileIndex_Below(int SelectedIndex)
    {
        if (SelectedIndex < 7)
        {
            return -1;
        }

        return (SelectedIndex-7);

    }
    #endregion

    #region IsTerraceTileAllowed(int SelectedIndex): Return true if entered index is in or higher than level 2
    /// <summary>
    /// Return true if entered index is in or higher than level 2
    /// </summary>
    /// <param name="SelectedIndex"></param>
    /// <returns></returns>
    public static bool IsTerraceTileAllowed(int SelectedIndex)
    {
        if (SelectedIndex > 6)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region IsRoofTileAllowed(int SelectedIndex): Return true if entered index is in or higher than level 5
    /// <summary>
    /// Return true if entered index is in or higher than level 5
    /// </summary>
    /// <param name="SelectedIndex"></param>
    /// <returns></returns>
    public static bool IsRoofTileAllowed(int SelectedIndex)
    {
        if(SelectedIndex > 27)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region IsTileInFirstRow(int SelectedIndex): Return true if entered index is in the first row
    /// <summary>
    /// Return true if entered index is in the first row
    /// </summary>
    /// <param name="SelectedIndex"></param>
    /// <returns></returns>
    public static bool IsTileInFirstRow(int SelectedIndex)
    {
        if (SelectedIndex < 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region TileIndex_Above (int SelectedIndex): Return The index of above house of entered index
    /// <summary>
    /// Return The index of above house of entered index
    /// </summary>
    /// <param name="SelectedIndex"></param>
    /// <returns></returns>
    public static int TileIndex_Above (int SelectedIndex)
    {
        if (SelectedIndex > 41)
        {
            return -1;
        }

        return (SelectedIndex + 7);

    }
    #endregion

    #region ThreeTileBelowSelectedIndex(int SelectedIndex): Return the index of three house below the entered index
    /// <summary>
    /// Return the index of three house below the entered index
    /// </summary>
    /// <param name="SelectedIndex"></param>
    /// <returns></returns>
    public static int ThreeTileBelowSelectedIndex(int SelectedIndex)
    {
        int tempIndex = -1;

        if (SelectedIndex > 20)
        {
            tempIndex = (SelectedIndex - (3*7));
        }

        return tempIndex;
    }
    #endregion

    #region TwooTileBelowSelectedIndex(int SelectedIndex): Return the array of two house below the entered index
    /// <summary>
    /// Return the array of two house below the entered index
    /// </summary>
    /// <param name="SelectedIndex"></param>
    /// <returns></returns>
    public static int[] TwooTileBelowSelectedIndex(int SelectedIndex)
    {
        List<int> twoTileBelow = new List<int>();

        if (SelectedIndex > 20)
        {
            twoTileBelow.Add(SelectedIndex - (1*7));
            twoTileBelow.Add(SelectedIndex - (2 * 7));
        }
        else
        {
            print("The Row is below 3");
        }

        return twoTileBelow.ToArray();
    }
    #endregion

    #region TileIndex_Around (int SelectedIndex) : Return Array of valid indexes around entered index
    /// <summary>
    /// Return Array of valid indexes around entered index
    /// </summary>
    /// <param name="SelectedIndex"></param>
    /// <returns></returns>
    public static int[] TileIndex_Around (int SelectedIndex)
    {
        bool IsInLeftEdge = false;
        bool IsInRightEdge = false;
        bool IsInBottomEdge = false;
        bool IsInTopEdge = false;

        List<int> AroundIndex = new List<int>();

        #region Check for Left edge
        for (int i = 0; i < 7; i++)
        {
            if(SelectedIndex == LeftEdge[i])
            {
                IsInLeftEdge = true;
                break;
            }
        }
        #endregion

        #region Check for Right edge
        for (int i = 0; i < 7; i++)
        {
            if (SelectedIndex == RightEdge[i])
            {
                IsInRightEdge = true;
                break;
            }
        }
        #endregion

        #region Check For Bottom Edge
        if (SelectedIndex < 7)
        {
            IsInBottomEdge = true;
        }
        #endregion

        #region Check For Top Edge
        if (SelectedIndex > 40)
        {
            IsInTopEdge = true;
        }
        #endregion



        if ( !IsInLeftEdge )
        {
            AroundIndex.Add(SelectedIndex - 1);
        }

        if( !IsInRightEdge )
        {
            AroundIndex.Add(SelectedIndex + 1);
        }

        if( !IsInBottomEdge )
        {
            AroundIndex.Add(SelectedIndex - 7);
        }

        if (!IsInTopEdge)
        {
            AroundIndex.Add(SelectedIndex + 7);
        }

        /*
        if(!IsInTopEdge && !IsInRightEdge)
        {
            AroundIndex.Add((SelectedIndex + 7) + 1);
        }

        if(!IsInTopEdge && !IsInLeftEdge)
        {
            AroundIndex.Add((SelectedIndex + 7) - 1);
        }

        if (!IsInBottomEdge && !IsInRightEdge)
        {

            AroundIndex.Add((SelectedIndex - 7) + 1);
        }

        if(!IsInBottomEdge && !IsInLeftEdge)
        {
            AroundIndex.Add((SelectedIndex - 7) - 1);
        }
        */

        return AroundIndex.ToArray();
    }

    #endregion

    #region TileIndex_InTheRow (int SelectedIndex) : Return Array of indexes in the selected row
    /// <summary>
    /// Return Array of indexes in the selected row
    /// </summary>
    /// <param name="SelectedIndex"></param>
    /// <returns></returns>
    public static int[] TileIndex_InTheRow(int SelectedIndex)
    {
        List<int> RowIndex = new List<int>();

        int Row = SelectedIndex / 7;

        for (int i = (Row*7); i < ((Row+1)*7); i++)
        {
            if (i == SelectedIndex)
                continue;

            RowIndex.Add(i);
        }

        return RowIndex.ToArray();
    }
    #endregion

    #region TileIndex_Sides(int SelectedIndex): Return Array of indexes in two sides of index and remove the index which is in the edge
    /// <summary>
    /// Return Array of indexes in two sides of index and remove the index which is in the edge
    /// </summary>
    /// <param name="SelectedIndex"></param>
    /// <returns></returns>
    public static int[] TileIndex_Sides(int SelectedIndex)
    {
        List<int> SidesIndex = new List<int>();


        bool InLeftEdge = false;
        bool InRightEdge = false;

        for (int i = 0; i < 7; i++)
        {
            if(SelectedIndex == LeftEdge[i])
            {
                InLeftEdge = true;
            }
            else if(SelectedIndex == RightEdge[i])
            {
                InRightEdge = true;
            }
        }

        if( !InLeftEdge )
        {
            SidesIndex.Add(SelectedIndex - 1);
        }

        if (!InRightEdge)
        {
            SidesIndex.Add(SelectedIndex + 1);
        }



        return SidesIndex.ToArray();
    }
    #endregion

    #region IsTileIndexesNearEachOther(CharactersType Character, int[] HouseIndexes): Check for MultipleCharacters to all the selected cells be near each other
    /// <summary>
    /// Check for MultipleCharacters to all the selected cells be near each other
    /// </summary>
    /// <param name="Character"></param>
    /// <param name="HouseIndexes"></param>
    /// <returns></returns>
    public static bool IsTileIndexesNearEachOther(CharactersType Character, int[] HouseIndexes)
    {
        bool IsNearEachOther = false;

        #region Check if Two House selected near eachother
        if (Character == CharactersType.TwoHouseGuy)
        {
            int tempSum = HouseIndexes[0] - HouseIndexes[1];

            if (tempSum == 7 || tempSum == (-7))
            {
                IsNearEachOther = true;
            }
        }
        #endregion
       
        #region Check if four House selected near eachother
        else if (Character == CharactersType.FourHouseGuy)
        {
            int tempTheSmallestNumber = 49;
            
            #region Get The lowest index of the array
            for (int i = 0; i < 4; i++)
            {
                if(tempTheSmallestNumber > HouseIndexes[i])
                    tempTheSmallestNumber = HouseIndexes[i];
            }
            #endregion

            #region Check all possible index based on the lowest index
            int CounterCheck = 0;
            for (int i = 0; i < 4; i++)
            {
                if( HouseIndexes[i] == tempTheSmallestNumber )
                {
                    CounterCheck++;
                }
                else if(HouseIndexes[i] == (tempTheSmallestNumber+1))
                {
                    CounterCheck++;
                }
                else if (HouseIndexes[i] == (tempTheSmallestNumber + 7))
                {
                    CounterCheck++;
                }
                else if (HouseIndexes[i] == (tempTheSmallestNumber + 8))
                {
                    CounterCheck++;
                }

            }

            if (CounterCheck == 4)
            {
                IsNearEachOther = true;
            }
            #endregion

        }
        #endregion

        #region Check if three House selected near eachother
        else if (Character == CharactersType.ThreeHouseLGuy)
        {
            int tempTheSmallestNumber = 49;

            #region Get The lowest index of the array
            for (int i = 0; i < 3; i++)
            {
                if (tempTheSmallestNumber > HouseIndexes[i])
                    tempTheSmallestNumber = HouseIndexes[i];
            }
            #endregion

            #region Check all possible index based on the lowest index
            int CounterCheck = 0;
            for (int i = 0; i < 3; i++)
            {
                if (HouseIndexes[i] == tempTheSmallestNumber)
                {
                    CounterCheck++;
                }
                else if (HouseIndexes[i] == (tempTheSmallestNumber + 1))
                {
                    CounterCheck++;
                }
                else if (HouseIndexes[i] == (tempTheSmallestNumber + 7))
                {
                    CounterCheck++;
                }

            }

            if (CounterCheck == 3)
            {
                IsNearEachOther = true;
            }
            #endregion

        }
        #endregion

        return IsNearEachOther;
    }
    #endregion

    #region IsTileIndexesTheSameColor(HouseCellsType[] Houses): Check for MultipleCharacters to all the selected cells be colord and the same color or terace tile
    public static bool IsTileIndexesTheSameColor(HouseCellsType[] Houses)
    {
        bool AreTilesTheSameColor = true;

        switch (Houses[0])
        {
            case HouseCellsType.BlueTile:
                for (int i = 1; i < Houses.Length; i++)
                {
                    if(Houses[i] != HouseCellsType.BlueTile && Houses[i] != HouseCellsType.OldBlueTile && Houses[i] != HouseCellsType.Terrace)
                    {
                        AreTilesTheSameColor = false;
                        break;
                    }
                }
                break;
            case HouseCellsType.RedTile:
                for (int i = 1; i < Houses.Length; i++)
                {
                    if (Houses[i] != HouseCellsType.RedTile && Houses[i] != HouseCellsType.OldRedTile && Houses[i] != HouseCellsType.Terrace)
                    {
                        AreTilesTheSameColor = false;
                        break;
                    }
                }
                break;
            case HouseCellsType.PurpleTile:
                for (int i = 1; i < Houses.Length; i++)
                {
                    if (Houses[i] != HouseCellsType.PurpleTile && Houses[i] != HouseCellsType.OldPurpleTile && Houses[i] != HouseCellsType.Terrace)
                    {
                        AreTilesTheSameColor = false;
                        break;
                    }
                }
                break;
            case HouseCellsType.YellowTile:
                for (int i = 1; i < Houses.Length; i++)
                {
                    if (Houses[i] != HouseCellsType.YellowTile && Houses[i] != HouseCellsType.OldYellowTile && Houses[i] != HouseCellsType.Terrace)
                    {
                        AreTilesTheSameColor = false;
                        break;
                    }
                }
                break;
            case HouseCellsType.OldBlueTile:
                for (int i = 1; i < Houses.Length; i++)
                {
                    if (Houses[i] != HouseCellsType.BlueTile && Houses[i] != HouseCellsType.OldBlueTile && Houses[i] != HouseCellsType.Terrace)
                    {
                        AreTilesTheSameColor = false;
                        break;
                    }
                }
                break;
            case HouseCellsType.OldRedTile:
                for (int i = 1; i < Houses.Length; i++)
                {
                    if (Houses[i] != HouseCellsType.RedTile && Houses[i] != HouseCellsType.OldRedTile && Houses[i] != HouseCellsType.Terrace)
                    {
                        AreTilesTheSameColor = false;
                        break;
                    }
                }
                break;
            case HouseCellsType.OldPurpleTile:
                for (int i = 1; i < Houses.Length; i++)
                {
                    if (Houses[i] != HouseCellsType.PurpleTile && Houses[i] != HouseCellsType.OldPurpleTile && Houses[i] != HouseCellsType.Terrace)
                    {
                        AreTilesTheSameColor = false;
                        break;
                    }
                }
                break;
            case HouseCellsType.OldYellowTile:
                for (int i = 1; i < Houses.Length; i++)
                {
                    if (Houses[i] != HouseCellsType.YellowTile && Houses[i] != HouseCellsType.OldYellowTile && Houses[i] != HouseCellsType.Terrace)
                    {
                        AreTilesTheSameColor = false;
                        break;
                    }
                }
                break;
            case HouseCellsType.Terrace:
                for (int i = 1; i < Houses.Length; i++)
                {
                    if(Houses[i] != HouseCellsType.Parking && Houses[i] != HouseCellsType.Terrace && Houses[i] != HouseCellsType.PentHouse)
                    {
                        HouseCellsType[] TempHouses = Houses;
                        TempHouses[0] = Houses[i];
                        TempHouses[i] = Houses[0];
                        AreTilesTheSameColor = IsTileIndexesTheSameColor(TempHouses);
                    }
                }
                break;
            default:
                AreTilesTheSameColor = false;
                break;
        }
        //........Here......... LastNight..........

        return AreTilesTheSameColor;
    }
    #endregion

    public static CharactersType[] OrderHouseTilesForMultipleUnitCharacter(int[] SelectedHouses, CharactersType Character)
    {
        CharactersType[] tempCharacterUnits = new CharactersType[SelectedHouses.Length];

        if (Character == CharactersType.TwoHouseGuy)
        {
            if (SelectedHouses[0] - SelectedHouses[1] == 7)
            {
                tempCharacterUnits[0] = CharactersType.TwoHouseGuy_Up;
                tempCharacterUnits[1] = CharactersType.TwoHouseGuy_Down;
            }
            else if (SelectedHouses[0] - SelectedHouses[1] == -7)
            {
                tempCharacterUnits[0] = CharactersType.TwoHouseGuy_Down;
                tempCharacterUnits[1] = CharactersType.TwoHouseGuy_Up;
            }
        }
        else if (Character == CharactersType.FourHouseGuy)
        {
            int tempSmallestIndex = 49;

            for (int i = 0; i < 4; i++)
            {
                if (SelectedHouses[i] < tempSmallestIndex)
                {
                    tempSmallestIndex = SelectedHouses[i];
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (SelectedHouses[i] == tempSmallestIndex)
                {
                    tempCharacterUnits[i] = CharactersType.FourHouseGuy_Down_Left;
                }
                else if (SelectedHouses[i] == (tempSmallestIndex + 1))
                {
                    tempCharacterUnits[i] = CharactersType.FourHouseGuy_Down_Right;
                }
                else if (SelectedHouses[i] == (tempSmallestIndex + 7))
                {
                    tempCharacterUnits[i] = CharactersType.FourHouseGuy_Up_Left;
                }
                else if (SelectedHouses[i] == (tempSmallestIndex + 8))
                {
                    tempCharacterUnits[i] = CharactersType.FourHouseGuy_Up_Right;
                }

            }
        }
        else if (Character == CharactersType.ThreeHouseLGuy)
        {
            int tempSmallestIndex = 49;

            for (int i = 0; i < 3; i++)
            {
                if (SelectedHouses[i] < tempSmallestIndex)
                {
                    tempSmallestIndex = SelectedHouses[i];
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (SelectedHouses[i] == tempSmallestIndex)
                {
                    tempCharacterUnits[i] = CharactersType.ThreeHouseLGuy_Center;
                }
                else if (SelectedHouses[i] == (tempSmallestIndex + 1))
                {
                    tempCharacterUnits[i] = CharactersType.ThreeHouseLGuy_Down_Right;
                }
                else if (SelectedHouses[i] == (tempSmallestIndex + 7))
                {
                    tempCharacterUnits[i] = CharactersType.ThreeHouseLGuy_Up;
                }

            }
        }

        return tempCharacterUnits;
    }

    // ------------------ Show Wrong Selection Function ---------------

    public void ShowWrongSelection()
    {
        int RandomIndex = UnityEngine.Random.Range(0, ErrorMessageSprites.Length);
        Sprite RandomErrorSprite = ErrorMessageSprites[RandomIndex];
        ErrorMessagePanel.GetComponent<Image>().sprite = RandomErrorSprite;
        ErrorMessagePanel.SetActive(true);
        soundManager.SFX_WrongACtionPlay();
    }

    public void HideErrorPanel()
    {
        ErrorMessagePanel.SetActive(false);
        print("Disable...");
    }

    public static int CalculateNegativeScore(CharactersType[] characters, HouseCellsType[] houseCells)
    {
        int tempNegativeScore = 0;
        int allPlacedHouses = 0;
        int emptyHouses = 0;
        int PlacedCharacter = 0;

        for (int i = 0; i < characters.Length; i++)
        {
            //if(characters[i] == CharactersType.Ghost)
            //{
            //    tempNegativeScore -= 10;
            //    print("Ghost --");
            //}
            if(characters[i] != CharactersType.Empty)
            {
                PlacedCharacter++;
            }
        }

        for (int i = 0; i < houseCells.Length; i++)
        {
            if(houseCells[i] != HouseCellsType.EmptyTile && houseCells[i] != HouseCellsType.BannedTile)
            {
                allPlacedHouses++;
            }
        }

        emptyHouses = allPlacedHouses - PlacedCharacter;

        print("Placed houses: " + PlacedCharacter);

        print("Empty houses: " + emptyHouses);

        tempNegativeScore -= (emptyHouses*5);

        print("tempNegativeScore: " + tempNegativeScore);

        return tempNegativeScore;
    }


    public void FinishGame(string winnerName,string winnerScore,string winnerPpoint,string winnerNpoint, string loserName, string loserScore, string loserPpoint, string loserNpoint,int deathNumberInGame)
    {
        soundManager.SoundTrackStop();

        if (deathNumberInGame == 5)   // if the witch win the game
        {
            WinnerIconReference.sprite = LoserIcon;
            WinnerLableTxt.text = "The Loser";
            WinnerLableTxt.color = new Color(0.517f, 0, 0, 1);
            WinnerName.GetComponent<Text>().color = new Color(0.517f, 0, 0, 1);
            WitchWinnerText.text = "The Winner";
            soundManager.WitchLaughter_SFX();
        }
        else
        {
            soundManager.SFX_EndOfGamePlay();
        }

        WinnerName.GetComponent<Text>().text = winnerName;
        WinnerScore.GetComponent<Text>().text = winnerScore;
        WinnerPpoint.GetComponent<Text>().text = winnerPpoint;
        WinnerNpoint.GetComponent<Text>().text = winnerNpoint;
        LoserName.GetComponent<Text>().text = loserName;
        LoserScore.GetComponent<Text>().text = loserScore;
        LoserPpoint.GetComponent<Text>().text = loserPpoint;
        LoserNpoint.GetComponent<Text>().text = loserNpoint;
        DeathPoint.GetComponent<Text>().text = deathNumberInGame.ToString();

        FinishedPanel.SetActive(true);
    }

    //public void LoseConnection(string Name)
    //{
    //    StartCoroutine(OtherConnectionLost(Name, 2));
    //}

    public IEnumerator OtherConnectionLost(float waitTime)
    {

        //networkButtonManager.Disconnection();

        // Displaying ConnectioLost Panel
        ConnctionLostPlayer.text = enemyName;
        ConnectionLostPanel.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        // Load MainMenue Scene
        ConnectionLostPanel.SetActive(false);
        
        SceneManager.LoadScene(1);


    }

    public void EnableEnemyTilesActiveForGhosts()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (myEnemyBoardGenerator.BoardCellsArray[i, j].CompareTag("BannedHouse"))
                    continue;
                myEnemyBoardGenerator.BoardCellsArray[i, j].GetComponent<Button>().interactable = true;
            }
        }
    }

    public void DisableEnemyTilesActiveForGhosts()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                myEnemyBoardGenerator.BoardCellsArray[i, j].GetComponent<Button>().interactable = false;
            }
        }

    }

    public void ResetGhostColorToWhite()
    {
        myBoardGenerator.GhostPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    public void ResetAllCellsColorToDefault()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if(myBoardGenerator.BoardCellsArray[i, j].CompareTag("Empty_Cell"))
                {
                    myBoardGenerator.BoardCellsArray[i, j].GetComponent<Image>().color = new Color(1, 1, 1, 0.15f);
                    print("Reset Empty cells");
                }
                else if (myBoardGenerator.BoardCellsArray[i, j].CompareTag("Full_Cell"))
                {
                    myBoardGenerator.BoardCellsArray[i, j].GetComponent<Image>().color = new Color(1,1,1,1);
                    print("Reset Full cells");

                }
            }
        }
        
    }
}