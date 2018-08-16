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

    public GameObject WinnerName;
    public GameObject WinnerScore;
    public GameObject WinnerPpoint;
    public GameObject WinnerNpoint;
    public GameObject LoserName;
    public GameObject LoserScore;
    public GameObject LoserPpoint;
    public GameObject LoserNpoint;


    public GameObject MyBoard;
    public GameObject EnemyBoard;

    public Sprite[] ErrorMessageSprites;

    private BoardGenerator myBoardGenerator;
    private BoardGenerator myEnemyBoardGenerator;
    private SoundManager soundManager;
    private NetworkButtonManager networkButtonManager;

    private static int[] LeftEdge = new int[] { 0, 7, 14, 21, 28, 35, 42 };
    private static int[] RightEdge = new int[] { 6, 13, 20, 27, 34, 41, 48 };


    [SerializeField]
    public SpriteReference spriteReference;

    private void Awake()
    {
        Initialazation();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            HideErrorPanel();
        }
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
        ErrorMessagePanel.SetActive(false);
        NetworkHudBtns.SetActive(true);
        FinishedPanel.SetActive(false);
        soundManager = FindObjectOfType<SoundManager>();
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
        CharacterDeck.SetActive(true);
        HouseDeck.SetActive(true);
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

    }

    public void HighlightPlayerNameWhoHasTheTurn(bool IsItMyTurn)
    {
        if (IsItMyTurn)
        {
            myBoardGenerator.UserNameTextBox.color = Color.white;
            myBoardGenerator.scoreText.color = Color.white;
            myBoardGenerator.ScoreLable.color = Color.white;

            myEnemyBoardGenerator.UserNameTextBox.color = Color.gray;
            myEnemyBoardGenerator.scoreText.color = Color.gray;
            myEnemyBoardGenerator.ScoreLable.color = Color.gray;

            myBoardGenerator.TurnPanel.SetActive(true);
            myEnemyBoardGenerator.TurnPanel.SetActive(false);
        }
        else
        {
            myBoardGenerator.UserNameTextBox.color = Color.gray;
            myBoardGenerator.scoreText.color = Color.gray;
            myBoardGenerator.ScoreLable.color = Color.gray;

            myEnemyBoardGenerator.UserNameTextBox.color = Color.white;
            myEnemyBoardGenerator.scoreText.color = Color.white;
            myEnemyBoardGenerator.ScoreLable.color = Color.white;

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
                }
            }
        }
        else if (!IsMyBoard)
        {
            for (int i = 0; i < HouseCellsArray.Length; i++)
            {
                myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Image>().sprite = SpriteBasedOnHouseCellType(HouseCellsArray[i]);
                myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Button>().interactable = false;
            }

        }
    }

    public void UpdateCharacterTileMap(CharactersType[] CharacterCellsArray, HouseCellsType[] HouseCellsArray, bool IsMyBoard = true)
    {
        if (IsMyBoard)
        {
            for (int i = 0; i < CharacterCellsArray.Length; i++)
            {
                if (CharacterCellsArray[i] != CharactersType.Empty)
                {
                    Image CharacterPlaceHolderImageComponenet = myBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.GetChild(0).GetComponent<Image>();
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
            for (int i = 0; i < CharacterCellsArray.Length; i++)
            {
                if (CharacterCellsArray[i] != CharactersType.Empty)
                {
                    Image CharacterPlaceHolderImageComponenet = myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].transform.GetChild(0).GetComponent<Image>();

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

    public void UpdateCharacterDeck(CharactersType[] CharactersType)
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


    public void HideconnectionHUDPanel()
    {
        NetworkHudBtns.SetActive(false);
    }

    #region Sprite SpriteBasedOnHouseCellType(HouseCellsType HouseCellType)
    /// <summary>
    /// Return a Sprite based on the enum house cell type
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
            case CharactersType.ThreeHouseLGuy:
                tempCharacterSprite = spriteReference.ThreeHouseLGuy;
                break;
            case CharactersType.FourHouseGuy:
                tempCharacterSprite = spriteReference.FourHouseGuy;
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
    }

    public static int CalculateNegativeScore(CharactersType[] characters, HouseCellsType[] houseCells, int PlacedCharacter)
    {
        int tempNegativeScore = 0;
        int allPlacedHouses = 0;
        int emptyHouses = 0;

        for (int i = 0; i < characters.Length; i++)
        {
            if(characters[i] == CharactersType.Ghost)
            {
                tempNegativeScore -= 10;
                print("Ghost --");
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

        print("Empty houses: " + emptyHouses);

        tempNegativeScore -= (emptyHouses*5);

        print("tempNegativeScore: " + tempNegativeScore);

        return tempNegativeScore;
    }

    public void FinishGame(string winnerName,string winnerScore,string winnerPpoint,string winnerNpoint, string loserName, string loserScore, string loserPpoint, string loserNpoint)
    {
        soundManager.SFX_EndOfGamePlay();

        WinnerName.GetComponent<Text>().text = winnerName;
        WinnerScore.GetComponent<Text>().text = winnerScore;
        WinnerPpoint.GetComponent<Text>().text = winnerPpoint;
        WinnerNpoint.GetComponent<Text>().text = winnerNpoint;
        LoserName.GetComponent<Text>().text = loserName;
        LoserScore.GetComponent<Text>().text = loserScore;
        LoserPpoint.GetComponent<Text>().text = loserPpoint;
        LoserNpoint.GetComponent<Text>().text = loserNpoint;

        FinishedPanel.SetActive(true);
    }

    public void LoseConnection(string Name)
    {
        StartCoroutine(OtherConnectionLost(Name, 2));

    }

    public IEnumerator OtherConnectionLost(string PlayerLost, float waitTime)
    {
        print("Enum...");

        //networkButtonManager.Disconnection();


        ConnctionLostPlayer.text = PlayerLost;
        ConnectionLostPanel.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        ConnectionLostPanel.SetActive(false);
        SceneManager.LoadScene(0);


    }
}