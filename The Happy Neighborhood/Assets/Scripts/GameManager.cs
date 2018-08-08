using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject GameLoadingPanel;
    public GameObject CharacterDeck;
    public GameObject HouseDeck;

    public GameObject MyBoard;
    public GameObject EnemyBoard;

    private BoardGenerator myBoardGenerator;
    private BoardGenerator myEnemyBoardGenerator;

    [SerializeField]
    public SpriteReference spriteReference;

    [SerializeField]
    public CharacterHouseReference CharacterHouse;

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
        CharacterDeck.SetActive(false);
        HouseDeck.SetActive(false);
        MyBoard.SetActive(false);
        EnemyBoard.SetActive(false);
        myBoardGenerator = MyBoard.GetComponent<BoardGenerator>();
        myEnemyBoardGenerator = EnemyBoard.GetComponent<BoardGenerator>();
        myBoardGenerator.TurnPanel.SetActive(false);
        myEnemyBoardGenerator.TurnPanel.SetActive(false);


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
            if(HouseDeckManager.HousesCardsDeckPickable[i])
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
        if(IsItMyTurn)
        {
            myBoardGenerator.UserNameTextBox.color = Color.white;
            myEnemyBoardGenerator.UserNameTextBox.color = Color.gray;
            myBoardGenerator.TurnPanel.SetActive(true);
            myEnemyBoardGenerator.TurnPanel.SetActive(false);
        }
        else
        {
            myBoardGenerator.UserNameTextBox.color = Color.gray;
            myEnemyBoardGenerator.UserNameTextBox.color = Color.white;
            myBoardGenerator.TurnPanel.SetActive(false);
            myEnemyBoardGenerator.TurnPanel.SetActive(true);

        }
    }

    // New Functions: 



    public void UpdateHouseTileMap(HouseCellsType[] HouseCellsArray, bool IsMyBoard = true)
    {

        if (IsMyBoard)
        {
            for (int i = 0; i < HouseCellsArray.Length; i++)
            {
                myBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Image>().overrideSprite = SpriteBasedOnHouseCellType(HouseCellsArray[i]);

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
                myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Image>().overrideSprite = SpriteBasedOnHouseCellType(HouseCellsArray[i]);
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
                if(CharacterCellsArray[i]!= CharactersType.Empty)
                {
                    myBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Image>().overrideSprite =
                        SpriteBasedOnCharacterCellInHouseType(CharacterCellsArray[i], HouseCellsArray[i]);
                }
            }
        }
        else if (!IsMyBoard)
        {
            for (int i = 0; i < CharacterCellsArray.Length; i++)
            {
                if (CharacterCellsArray[i] != CharactersType.Empty)
                {
                    myEnemyBoardGenerator.BoardCellsArray[i / 7, i % 7].GetComponent<Image>().overrideSprite = 
                        SpriteBasedOnCharacterCellInHouseType(CharacterCellsArray[i], HouseCellsArray[i]);

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


    #region Sprite SpriteBasedOnCharacterCellInHouseType(CharactersType CharactersType)
    /// <summary>
    /// Return a Sprite based on the enum character cell type
    /// </summary>
    /// <param name="CharactersType"></param>
    /// <returns></returns>
    public Sprite SpriteBasedOnCharacterCellInHouseType(CharactersType CharactersType, HouseCellsType houseCellsType)
    {
        Sprite tempCharacterSprite;

        if(CharactersType == CharactersType.Wizard)
        {
            tempCharacterSprite = CharacterHouse.wizard.ShowHouse(houseCellsType);
        }

        return CharacterHouse.wizard.ShowHouse(houseCellsType);

        //return tempCharacterSprite;
    }

    #endregion

}
