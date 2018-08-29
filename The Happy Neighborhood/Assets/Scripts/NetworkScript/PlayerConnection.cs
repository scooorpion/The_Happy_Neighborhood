using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PlayerConnection : NetworkBehaviour     
{
    #region Fields
    public CharactersType[,] CharacterArrays = new CharactersType[7, 7];
    public HouseCellsType[,] HouseTileArrays = new HouseCellsType[7, 7];
    public int BanedConstructionCellNumber = 4;

    [SyncVar]
    public string UserName;

    static int ServerTurn = 0;

    [SyncVar]
    public int MyTurnID;
    public string EnemyName;
    private PlayerConnection enemyPlayerConnection;
    static int ActiveConnections = 0;
    private GameManager gameManagerscript;
    private bool IsGameStarted = false;

    private bool IsReadyForUpdateHouseCellsOnScreen = false;
    private bool IsReadyForUpdateCharacterCellsOnScreen = false;
    private bool IsReadyForUpdateHouseCardsOnScreen = false;
    private bool IsReadyForUpdateCharacterCardsOnScreen = false;
    private static bool  IsReadyForUpdateGhostsOnScreen = false;
    private static bool IsGhostAttackDone = false;
    private bool IsErrorMustShown = false;
    private int ErrorForPlayerID = 0;
    private static bool IsScoreChanged = false;
    private static int ScoreForPlayerID = 0;
    private static int ThePlayerIDWhoLeft = 0;

    private static int GhostAttacked_PlayerID;
    private static int GhostAttackedIndex;

    public static bool IsGameTurnSet = false;
    private bool IsOldHousePlacementDone = false;
    private bool IsHousePlacementDone = false;
    private bool IsCharacterPlacementDone = false;


    public static string WinnerName;
    public static int WinnerID;
    public static int WinnerScore;
    public static int WinnerPpoint;
    public static int WinnerNpoint;
    public static string LoserName;
    public static int LoserID;
    public static int LoserScore;
    public static int LoserPpoint;
    public static int LoserNpoint;

    public static bool IsOnePlayerLeftTheGame = false;


    private PlayerConnection enemyConnection;
    private SoundManager soundManager;

    public HouseCellsType[] MyHouseCells = new HouseCellsType[49];
    public CharactersType[] MyCharCells = new CharactersType[49];


    static HouseCellsType[] HouseCardsInGameDeck = new HouseCellsType[4];
    static CharactersType[] CharacterCardsInGameDeck = new CharactersType[4];

    public CardType CardTypeSelected;
    public HouseCellsType HouseCardSelected;
    public CharactersType CharacterdCardSelected;

    public static int Score = 0;
    static int tempGhost = 0;

    public static int GhostChangedID = 0;


    // Server-Side Arrays

    static HouseCellsType[] HouseCells_P1_Server ;
    static HouseCellsType[] HouseCells_P2_Server ;

    static int CharactersInHouse_P1_Server;
    static int CharactersInHouse_P2_Server;

    static int Score_P1_Server;
    static int Score_P2_Server;

    static bool IsGameFinished = false;

    static CharactersType[] CharCells_P1_Server ;
    static CharactersType[] CharCells_P2_Server ;

    static int Ghosts_P1_Server;
    static int Ghosts_P2_Server;

    static public List<HouseCellsType> HouseCardsDeckInGame_Server ;
    static public List<CharactersType> CharacterCardsDeckInGame_Server ;

    public static List<HouseCellsType> HousesDeckList_Server ;
    public static List<CharactersType> CharactersDeckList_Server ;


    static bool IsNoFirstRandomTurn = true;


    public bool ShowConnectionLostPanel = false;



    private bool[] FlagsToDisable;
    #endregion

    #region Start()
    void Start()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        if (isServer)
        {
            CmdResetServerData();
        }

        /*FlagsToDisable = new bool[] 
        {
            IsReadyForUpdateHouseCellsOnScreen ,
            IsReadyForUpdateCharacterCellsOnScreen,
            IsReadyForUpdateHouseCardsOnScreen,
            IsReadyForUpdateCharacterCardsOnScreen,
            IsReadyForUpdateGhostsOnScreen
        };    */

        soundManager = FindObjectOfType<SoundManager>();

        // Changed:===>  gameManagerscript = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        gameManagerscript = FindObjectOfType<GameManager>();

        CmdAskToTellActiveConnection();
       
        CmdAskToSetPlayerTurn();


        //First change the name locally then on the server
        UserName = PlayerPrefs.GetString(MenuManager.UserNamePlayerPrefs);

        CmdAskToSetUserName(UserName);

        SetCardSelectedToNull();

        HandleTextFile.CreateStreamLog();

    }
    #endregion


    void Update()
    {

        #region Check If it is our script or other player
        if (!isLocalPlayer)
        {
            return;
        }
        #endregion

        if (Input.GetMouseButtonDown(0))
        {
            gameManagerscript.HideErrorPanel();
        }

        #region Check If the game is finished
        if (IsGameFinished)
        {
            if(WinnerID == MyTurnID)
            {
                WinnerName = UserName;
                LoserName = EnemyName;
            }
            else
            {
                WinnerName = EnemyName;
                LoserName = UserName;
            }

            gameManagerscript.FinishGame(WinnerName, WinnerScore.ToString(), WinnerPpoint.ToString(), WinnerNpoint.ToString(), LoserName, LoserScore.ToString(), LoserPpoint.ToString(), LoserNpoint.ToString());
            IsGameFinished = false;
        }
        #endregion
        
        #region if the gmae is not finished
        else
        {
            #region Wait untill MyTurnId is set by RPC to 1 or 2
            if (MyTurnID == 0)
            {
                return;
            }
            #endregion

            ShowAnimationBasedOnActiveConnectionNumbers();

            #region Simiulating the HOUSE cell arrays of mine and my enemy board on screen based on flag: IsReadyForUpdateCellsOnScreen

            if (IsReadyForUpdateHouseCellsOnScreen)
            {
                if (IsOldHousePlacementDone)
                {
                    soundManager.SFX_OldHousePlacementPlay();
                    IsOldHousePlacementDone = false;
                }
                else if (IsHousePlacementDone)
                {
                    soundManager.SFX_HousePlacementPlay();
                    IsHousePlacementDone = false;
                }


                // Simiulating my cell array
                gameManagerscript.UpdateHouseTileMap(MyHouseCells);

                // Simiulating my enemy cell array
                StartCoroutine(CreateAndUpdateEnemyHouseTiles(0.5f));


                // Set Flag For Update Screen to flase
                IsReadyForUpdateHouseCellsOnScreen = false;              //===> Disable by couroutine

            }
            #endregion

            #region Simiulating the CHARACTER cell arrays of mine and my enemy board on screen based on flag: IsReadyForUpdateCellsOnScreen

            if (IsReadyForUpdateCharacterCellsOnScreen)
            {
                if (IsCharacterPlacementDone)
                {
                    soundManager.SFX_CharactersPlacementPlay();
                    IsCharacterPlacementDone = false;
                }

                // Simiulating my cell array
                gameManagerscript.UpdateCharacterTileMap(MyCharCells, MyHouseCells);

                // Simiulating my enemy cell array
                StartCoroutine(CreateAndUpdateEnemyCharacterTiles(0.5f));

                // Set Flag For Update Screen to flase
                IsReadyForUpdateCharacterCellsOnScreen = false;              //===> Disable by couroutine
                
            }
            #endregion

            if (IsGhostAttackDone)
            {
                if (GhostAttacked_PlayerID == MyTurnID)
                {
                    gameManagerscript.UpdateCharacterTileMap(GhostAttackedIndex, true);
                }
                else
                {
                    gameManagerscript.UpdateCharacterTileMap(GhostAttackedIndex, false);
                }

                soundManager.SFX_CharactersPlacementPlay();

                IsGhostAttackDone = false;
            }


            #region Show Ghost Card on mine and my enemy screen based on flag: IsReadyForUpdateGhostsOnScreen
            if (IsReadyForUpdateGhostsOnScreen)
            {

                if (GhostChangedID == MyTurnID)
                {
                    gameManagerscript.UpdateGhost(tempGhost, true);
                }
                else
                {
                    gameManagerscript.UpdateGhost(tempGhost, false);

                }

                IsReadyForUpdateGhostsOnScreen = false;   //==> Disable by couroutine
            }
            #endregion

            #region Show Error on my screen based on flag: IsErrorMustShown
            if (IsErrorMustShown)
            {
                if (ErrorForPlayerID == MyTurnID)
                {
                    gameManagerscript.ShowWrongSelection();
                }

                IsErrorMustShown = false;
            }
            #endregion

            #region Update Score on my enemy and my screen based on flag: IsScoreChanged
            if (IsScoreChanged)
            {

                if (ScoreForPlayerID == MyTurnID)
                {
                    gameManagerscript.UpdatePlayersScore(Score, true);
                }
                else
                {
                    gameManagerscript.UpdatePlayersScore(Score, false);
                }

                IsScoreChanged = false;
            }
            #endregion

            #region Simiulating the character decks on screen based on flag: IsReadyForUpdateCharacterCardsOnScreen

            if (IsReadyForUpdateCharacterCardsOnScreen)
            {
                gameManagerscript.UpdateCharacterDeck(CharacterCardsInGameDeck);
                IsReadyForUpdateCharacterCardsOnScreen = false;             // ===> Disable by couroutine

            }

            #endregion

            #region Simiulating the house decks on screen based on flag: IsReadyForUpdateHouseCardsOnScreen

            if (IsReadyForUpdateHouseCardsOnScreen)
            {
                gameManagerscript.UpdateHouseDeck(HouseCardsInGameDeck);
                IsReadyForUpdateHouseCardsOnScreen = false;              //===> Disable by couroutine

                print("Simiulating the house decks ");
            }

            #endregion

            #region Enabling or disabling decks to select based on if it is my turn or not

            if (IsGameTurnSet)
            {

                if (ServerTurn == MyTurnID)
                {
                    CmdAskToUpdateItsTurnVariable();
                    string Log = ("Command Server to Update Turn ===> [Client: " + MyTurnID + " ]");
                    HandleTextFile.WriteString(Log);
                    print(Log);

                    #region When its your turn, First update card deck and your enemy map

                    gameManagerscript.UpdateHouseDeck(HouseCardsInGameDeck);
                    gameManagerscript.UpdateCharacterDeck(CharacterCardsInGameDeck);

                    gameManagerscript.SetEnemyName(EnemyName);
                    gameManagerscript.UpdateHouseTileMap(enemyConnection.MyHouseCells, false);
                    gameManagerscript.UpdateCharacterTileMap(enemyConnection.MyCharCells, enemyConnection.MyHouseCells, false);

                    /*
                    StartCoroutine(CreateAndUpdateEnemyHouseTiles(0.5f));
                    StartCoroutine(CreateAndUpdateEnemyCharacterTiles(0.5f));
                    */
                    #endregion
                    gameManagerscript.HighlightPlayerNameWhoHasTheTurn(true);
                    gameManagerscript.EnableDecks();

                    IsGameTurnSet = false;

                }
                else if (ServerTurn != MyTurnID)
                {
                    gameManagerscript.HighlightPlayerNameWhoHasTheTurn(false);
                    gameManagerscript.DisableDecks();
                    IsGameTurnSet = false;
                }

            }

            #endregion


            //StartCoroutine(DisableFlag(FlagsToDisable, 0.4f));
        }
        #endregion


    }

    IEnumerator DisableFlag(bool[] FlagToDisable, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        for (int i = 0; i < FlagToDisable.Length; i++)
        {
            FlagToDisable[i] = false;
            
        }
    }


    #region ShowAnimationBasedOnActiveConnectionNumbers()
    /// <summary>
    /// Show Animation Based On Active Players(Waiting,Game Loading,Room is full)
    /// </summary>
    void ShowAnimationBasedOnActiveConnectionNumbers()
    {
        if(!IsGameStarted)
        {
            
            switch (ActiveConnections)
            {
                case 0:
                    break;
                case 1:     // Wait for other player to connecte
                    gameManagerscript.ShowWaitingAnimation();
                    break;
                case 2:      // Start the game:
                    FirstTimeStartTheGameSetting();
                    IsGameStarted = true;
                    break;
                default:        // Display an error that server is full and a back button 
                    gameManagerscript.ShowRoomIsFull();
                    break;
            }

            if(ActiveConnections > 0)
            {
                //HandleTextFile.WriteString( "Active Connections: " + ActiveConnections.ToString());                    
            }
        }
    }
    #endregion
   
    #region GetEnemyScript()
    /// <summary>
    /// Get PlayerConnection which belongs to enemy and store in enemyConnection
    /// </summary>
    void GetEnemyScript()
    {
        PlayerConnection[] PlayerConnections = GameObject.FindObjectsOfType<PlayerConnection>();

        for (int i = 0; i < PlayerConnections.Length; i++)
        {

            if (!PlayerConnections[i].CompareTag("MyConnection"))
            {
                enemyConnection = PlayerConnections[i];
                return;
            }
        }

    }
    #endregion

    #region setEnemyName()
    /// <summary>
    /// Set the enemy name on our local system from enemy GameObject component
    /// </summary>
    void setEnemyName()
    {
        EnemyName = enemyConnection.UserName;
    }

    #endregion


    #region CreateAndUpdateEnemyHouseTiles(float waitTime) [Coroutine]
    /// <summary>
    /// Courotine for creating enemy map and show the name after a delay to prevent server problem
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator CreateAndUpdateEnemyHouseTiles(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameManagerscript.SetEnemyName(EnemyName);
        gameManagerscript.UpdateHouseTileMap(enemyConnection.MyHouseCells, false);
    }
    #endregion

    #region CreateAndUpdateEnemyCharacterTiles(float waitTime) [Coroutine]
    /// <summary>
    /// Courotine for creating enemy map and show the name after a delay to prevent server problem
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator CreateAndUpdateEnemyCharacterTiles(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameManagerscript.UpdateCharacterTileMap(enemyConnection.MyCharCells, enemyConnection.MyHouseCells, false);
    }
    #endregion



    #region RequestForFirstTurnSet(float waitTime) [Coroutine]
    /// <summary>
    /// Coroutine for seting player turn after a delay in order to decks will be generated by server
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator RequestForFirstTurnSet(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CmdAskToSetPlayerNextTurn(true);
    }
    #endregion

    #region OnStartLocalPlayer()
    public override void OnStartLocalPlayer()
    {
        gameObject.tag = "MyConnection";
    }
    #endregion

    #region FirstTimeStartTheGameSetting()
    /// <summary>
    /// The Setting for Starting The Game For The First Time
    /// </summary>
    void FirstTimeStartTheGameSetting()
    {
        FindObjectOfType<MyNetworkDiscovery>().StopBroadcast();

        soundManager.SoundTrackPlay();

        GetEnemyScript();

        setEnemyName();

        GameManager.enemyName = EnemyName;

        gameManagerscript.HideconnectionHUDPanel();

        gameManagerscript.SetMyName(UserName);


        gameManagerscript.SetDiactiveUIBeginingWaitingPanel();

        CmdAskToCreateDeckLists(MyTurnID);

        CmdAskToFillEmptyCharInGameDeck(true);

        CmdAskToFillEmptyHouseInGameDeck(true);

        CmdAskToCreateHouseTilesArray(MyTurnID);

        StartCoroutine(RequestForFirstTurnSet(0.5f));

        gameManagerscript.ShowGameLoading();

    }

    #endregion

    #region SetCardSelectedToNull()
    /// <summary>
    /// To reset card selection to null this function must be called
    /// </summary>
    public void SetCardSelectedToNull()
    {
        CardTypeSelected = CardType.NoSelection;
        HouseCardSelected = HouseCellsType.EmptyTile;
        CharacterdCardSelected = CharactersType.Empty;
    }
    #endregion

    #region CommandToCheckSelectedCell(int cellNumber)
    /// <summary>
    /// When a place is selected on the map this function check it on the server
    /// </summary>
    /// <param name="cellNumber"></param>
    public void CommandToCheckSelectedCell(int cellNumber, BoardType WhosBoardSelection)
    {
        CmdAskToCheckSelectedCell(cellNumber, CardTypeSelected, HouseCardSelected, CharacterdCardSelected, MyTurnID, WhosBoardSelection);

    }
    #endregion

    public void CommandToCheckMultipleCharsOnSelectedCells(int[] cellNumbers, CharactersType Character )
    {
        CmdAskToCheckMultipleCharsOnSelectedCells(cellNumbers, Character, MyTurnID);
    }

    // --------------------- Network section ---------------------


    #region COMMANDS

    #region CmdAskToTellActiveConnection()
    /// <summary>
    /// Command Server To Tell active Connection
    /// </summary>
    [Command]
    void CmdAskToTellActiveConnection()
    {
        ActiveConnections = NetworkServer.connections.Count;
        RpcTellActiveClients(ActiveConnections);
    }
    #endregion

    #region CmdAskToSetUserName(string name)
    /// <summary>
    /// Command Server To Set User Name And Tell Every Clients
    /// </summary>
    /// <param name="name">change user name to this name</param>
    [Command]
    void CmdAskToSetUserName(string name)
    {
        UserName = name;
    }
    #endregion

    #region CmdAskToSetPlayerTurn()
    /// <summary>
    /// Command Server To Set User Turn Number And Tell Every Clients
    /// </summary>
    [Command]
    void CmdAskToSetPlayerTurn()
    {
        if(ServerTurn == 0)
        {
            ServerTurn = 1;
        }
        else if(ServerTurn == 1)
        {
            ServerTurn = 2;
        }
        else
        {
            print("Player Turn is Invalid: "+ServerTurn);
            ServerTurn = 1;
        }

        MyTurnID = ServerTurn;
    }
    #endregion

    #region CmdAskToCreateHouseTilesArray(int Turn)
    /// <summary>
    /// Command server to create an array of banned construction to fill the map
    /// </summary>
    [Command]
    void CmdAskToCreateHouseTilesArray(int Turn)
    {
        HouseCellsType[] cellhouseTemp = new HouseCellsType[49];
        int[] bannedCellsTemp = new int[BanedConstructionCellNumber];

        for (int i = 0; i < 49; i++)
        {
            cellhouseTemp[i] = HouseCellsType.EmptyTile;
        }

        for (int i = 0; i < BanedConstructionCellNumber; i++)
        {
            bool RepeatedCell;
            int randomCell;
            int[] RandomRangeValidIndex = new int[]
            {
                15,16,17,18,19,
                22,23,24,25,26,
                29,30,31,32,33,
                36,37,38,39,40,
                43,44,45,46,47
            };
            // Checking not to use repeated cell for NoConstructionCell
            do
            {
                RepeatedCell = false;
                
                //randomCell = UnityEngine.Random.Range()
                randomCell = RandomRangeValidIndex[UnityEngine.Random.Range(0, RandomRangeValidIndex.Length)];

                for (int j = 0; j < BanedConstructionCellNumber; j++)
                {
                    if (bannedCellsTemp[j] == randomCell)
                    {
                        RepeatedCell = true;
                        continue;
                    }
                }
            }
            while (RepeatedCell);

            bannedCellsTemp[i] = randomCell;
        }

        for (int i = 0; i < BanedConstructionCellNumber; i++)
        {
            cellhouseTemp[ bannedCellsTemp[i] ] = HouseCellsType.BannedTile;
        }

        if (Turn == 1)
        {
            HouseCells_P1_Server = cellhouseTemp;
        }
        else if (Turn == 2)
        {
            HouseCells_P2_Server = cellhouseTemp;
        }


        RpcTellHouseCells(cellhouseTemp,false,false);
    }
    #endregion

    #region CmdAskToCreateDeckLists(int PlayerTurnID)
    /// <summary>
    /// Command Server to Create Deck Lists of Character and House and store it
    /// </summary>
    /// <param name="PlayerTurnID">set a clients id in order not to called by all clients</param>
    [Command]
    void CmdAskToCreateDeckLists(int PlayerTurnID)
    {
        // Use Player ID to check no to call this Command twice by two player
        // because we need to create just one time this deck

        if (PlayerTurnID == 1)
        {
            #region Creating CharacterList Deck

            int EnumCharacterLenght = Enum.GetNames(typeof(CharactersType)).Length;

            CharactersType[] RemovedCharavter = new CharactersType[]
            {
                CharactersType.Ghost,
                CharactersType.Empty,
                CharactersType.TripleGuys,
                CharactersType.Animal,
                CharactersType.GhostCatcher,
                CharactersType.GuyNeedGarden,
                CharactersType.GuyNeedParking,
                CharactersType.GuyWithAnimal,
                CharactersType.Baby,
                CharactersType.Wizard,
                CharactersType.Gangster,
                CharactersType.DoubleGuys,
                CharactersType.FourHouseGuy,
                CharactersType.FamilyTwoGuys,
                CharactersType.ThreeHouseLGuy,
                CharactersType.TwoHouseGuy_Up,
                CharactersType.TwoHouseGuy_Down
            };

            /*
                        for (int i = 0; i < EnumCharacterLenght; i++)
                        {


                            CharactersType CharacterTemp = (CharactersType)i;

                            // These Character shouldnt be added to deck based on game design
                            if ( Array.IndexOf(RemovedCharavter,CharacterTemp) >= 0 )
                            {
                                continue;
                            }

                            for (int j = 0; j < 3; j++)  // Each card has 3 Ratio in a deck
                            {
                                CharactersDeckList_Server.Add(CharacterTemp);
                            }

                        }

                        $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                        $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                        $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                        Changeed For Test
                        $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                        $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            */
            for (int i = 0; i < 10; i++)
            {
                CharactersDeckList_Server.Add(CharactersType.RedNoBlueGuy);
            }
            for (int i = 0; i < 10; i++)
            {
                CharactersDeckList_Server.Add(CharactersType.OldGuy);
            }
            for (int i = 0; i < 10; i++)
            {
                CharactersDeckList_Server.Add(CharactersType.ThreeHouseLGuy);
            }
            for (int i = 0; i < 10; i++)
            {
                CharactersDeckList_Server.Add(CharactersType.BlueNoYellow);
            }


            #endregion

            #region Creating HouseList Deck

            int EnumHouseLenght = Enum.GetNames(typeof(HouseCellsType)).Length;

            for (int i = 0; i < EnumHouseLenght; i++)
            {
                HouseCellsType HouseCellTemp = (HouseCellsType)i;
                int CardRatioNumber;

                // Assign proper card ratio to each card
                /*
                switch (HouseCellTemp)
                {
                    case HouseCellsType.BannedTile:
                        CardRatioNumber = 0;
                        break;
                    case HouseCellsType.EmptyTile:
                        CardRatioNumber = 0;
                        break;
                    case HouseCellsType.BlueTile:
                        CardRatioNumber = 15;
                        break;
                    case HouseCellsType.RedTile:
                        CardRatioNumber = 15;
                        break;
                    case HouseCellsType.PurpleTile:
                        CardRatioNumber = 15;
                        break;
                    case HouseCellsType.YellowTile:
                        CardRatioNumber = 15;
                        break;
                    case HouseCellsType.OldBlueTile:
                        CardRatioNumber = 5;
                        break;
                    case HouseCellsType.OldRedTile:
                        CardRatioNumber = 5;
                        break;
                    case HouseCellsType.OldPurpleTile:
                        CardRatioNumber = 5;
                        break;
                    case HouseCellsType.OldYellowTile:
                        CardRatioNumber = 5;
                        break;
                    case HouseCellsType.PentHouse:
                        CardRatioNumber = 8;
                        break;
                    case HouseCellsType.Parking:
                        CardRatioNumber = 0;
                        break;
                    case HouseCellsType.Terrace:
                        CardRatioNumber = 0;
                        break;
                    case HouseCellsType.Garden:
                        CardRatioNumber = 0;
                        break;
                    default:
                        CardRatioNumber = 0;
                        break;
                }
                */
                switch (HouseCellTemp)
                {
                    case HouseCellsType.BannedTile:
                        CardRatioNumber = 0;
                        break;
                    case HouseCellsType.EmptyTile:
                        CardRatioNumber = 0;
                        break;
                    case HouseCellsType.BlueTile:
                        CardRatioNumber = 15;
                        break;
                    case HouseCellsType.RedTile:
                        CardRatioNumber = 15;
                        break;
                    case HouseCellsType.PurpleTile:
                        CardRatioNumber = 15;
                        break;
                    case HouseCellsType.YellowTile:
                        CardRatioNumber = 15;
                        break;
                    case HouseCellsType.OldBlueTile:
                        CardRatioNumber = 5;
                        break;
                    case HouseCellsType.OldRedTile:
                        CardRatioNumber = 5;
                        break;
                    case HouseCellsType.OldPurpleTile:
                        CardRatioNumber = 5;
                        break;
                    case HouseCellsType.OldYellowTile:
                        CardRatioNumber = 5;
                        break;
                    case HouseCellsType.PentHouse:
                        CardRatioNumber = 8;
                        break;
                    case HouseCellsType.Parking:
                        CardRatioNumber = 0;
                        break;
                    case HouseCellsType.Terrace:
                        CardRatioNumber = 100;
                        break;
                    case HouseCellsType.Garden:
                        CardRatioNumber = 0;
                        break;
                    default:
                        CardRatioNumber = 0;
                        break;
                }

                for (int j = 0; j < CardRatioNumber; j++)  // Each card has Specific Ratio in a deck
                {
                    HousesDeckList_Server.Add(HouseCellTemp);
                }


            }

            #endregion

        }


    }
    #endregion

    #region CmdAskToFillEmptyCharInGameDeck()
    /// <summary>
    /// Command Server To Fill each empty character slot in In-Game Deck Slot which has 4 slot and broadcast the list
    /// </summary>
    [Command]
    void CmdAskToFillEmptyCharInGameDeck(bool IsFirstTimeCreateion)
    {


        if (CharactersDeckList_Server.Count < 4)
        {
            print("There is less than 4 Cards");
            return;
        }

        if (IsFirstTimeCreateion)
        {
            while (CharacterCardsDeckInGame_Server.Count < 4)
            {
                CharactersType CharacterTemp;
                int RandomIndex;
                bool RepeatedCard;

                // Check not to have a repetetive cell 
                do
                {
                    RepeatedCard = false;

                    RandomIndex = UnityEngine.Random.Range(0, CharactersDeckList_Server.Count);

                    CharacterTemp = CharactersDeckList_Server[RandomIndex];

                    if (CharacterCardsDeckInGame_Server.Contains(CharacterTemp))
                    {
                        RepeatedCard = true;
                    }

                } while (RepeatedCard);


                CharacterCardsDeckInGame_Server.Add(CharacterTemp);
                CharactersDeckList_Server.RemoveAt(RandomIndex);
            }
        }
        else if (!IsFirstTimeCreateion)
        {

            while (CharacterCardsDeckInGame_Server.Contains(CharactersType.Empty))
            {
                CharactersType CharacterTemp;
                int RandomIndex;
                bool RepeatedCard;

                // Check not to have a repetetive cell 
                do
                {
                    RepeatedCard = false;

                    RandomIndex = UnityEngine.Random.Range(0, CharactersDeckList_Server.Count);

                    CharacterTemp = CharactersDeckList_Server[RandomIndex];

                    if (CharacterCardsDeckInGame_Server.Contains(CharacterTemp))
                    {
                        RepeatedCard = true;
                    }

                } while (RepeatedCard);

                CharacterCardsDeckInGame_Server.Remove(CharactersType.Empty);
                CharacterCardsDeckInGame_Server.Add(CharacterTemp);
                CharactersDeckList_Server.RemoveAt(RandomIndex);
            }
        }

        RpcTellCharacterInGameDeck(CharacterCardsDeckInGame_Server.ToArray());
    }
    #endregion

    #region CmdAskToFillEmptyHouseInGameDeck()
    /// <summary>
    /// Command Server To Fill each empty House slot in In-Game Deck Slot which has 4 slot and broadcast the list
    /// </summary>
    [Command]
    void CmdAskToFillEmptyHouseInGameDeck(bool IsFirstTimeCreateion)
    {
        

        if (HousesDeckList_Server.Count < 4)
        {
            print("There is less than 4 Cards");
            return;
        }

        if (IsFirstTimeCreateion)
        {
            while (HouseCardsDeckInGame_Server.Count < 4)
            {
                HouseCellsType HouseTemp;
                int RandomIndex;
                bool RepeatedCard;

                // Check not to have a repetetive cell 
                do
                {
                    RepeatedCard = false;

                    RandomIndex = UnityEngine.Random.Range(0, HousesDeckList_Server.Count);

                    HouseTemp = HousesDeckList_Server[RandomIndex];

                    if (HouseCardsDeckInGame_Server.Contains(HouseTemp))
                    {
                        RepeatedCard = true;
                    }

                } while (RepeatedCard);


                HouseCardsDeckInGame_Server.Add(HouseTemp);
                HousesDeckList_Server.RemoveAt(RandomIndex);
            }
        }
        else if (!IsFirstTimeCreateion)
        {
            while (HouseCardsDeckInGame_Server.Contains(HouseCellsType.EmptyTile))
            {
                HouseCellsType HouseTemp;
                int RandomIndex;
                bool RepeatedCard;

                // Check not to have a repetetive cell 
                do
                {
                    RepeatedCard = false;

                    RandomIndex = UnityEngine.Random.Range(0, HousesDeckList_Server.Count);

                    HouseTemp = HousesDeckList_Server[RandomIndex];

                    if (HouseCardsDeckInGame_Server.Contains(HouseTemp))
                    {
                        RepeatedCard = true;
                    }

                } while (RepeatedCard);

                HouseCardsDeckInGame_Server.Remove(HouseCellsType.EmptyTile);
                HouseCardsDeckInGame_Server.Add(HouseTemp);
                HousesDeckList_Server.RemoveAt(RandomIndex);
            }
        }

        RpcTellHouseInGameDeck(HouseCardsDeckInGame_Server.ToArray());
    }
    #endregion

    #region CmdAskToSetPlayerNextTurn(bool IsItForFirstTime)
    /// <summary>
    /// Command Server to set next game turn
    /// </summary>
    /// <param name="IsItForFirstTime">If it is the first time for setting turn</param>
    [Command]
    void CmdAskToSetPlayerNextTurn(bool IsItForFirstTime)
    {


        int nextTurn = 0;

        if (!IsItForFirstTime)
        {
            if(ServerTurn == 1)
            {
                nextTurn = 2;
            }
            else if(ServerTurn == 2)
            {
                nextTurn = 1;
            }

            RpcTellTurn(nextTurn);
        }
        else if (IsItForFirstTime)
        {
            if(IsNoFirstRandomTurn)
            {
                ServerTurn = UnityEngine.Random.Range(1, 3);
                IsNoFirstRandomTurn = false;
                nextTurn = ServerTurn;
                RpcTellTurn(nextTurn);


            }
            else
            {

            }
        }

    }
    #endregion

    #region  CmdAskToUpdateItsTurnVariable()
    /// <summary>
    /// Command server to upldate game player turn, (it is called by the player who has the turn, now)
    /// </summary>
    [Command]
    void CmdAskToUpdateItsTurnVariable()
    {
        if (ServerTurn == 1)
        {
            ServerTurn = 2;
        }
        else if (ServerTurn == 2)
        {
            ServerTurn = 1;
        }
        print("[Server] : Update Server Turn to: " + ServerTurn);
    }
    #endregion

    #region CmdAskToCheckSelectedCell(int cellNumber,CardType cardType, HouseCellsType houseCellsType, CharactersType charactersType, int PlayerID)
    /// <summary>
    /// Command Server To Validate the player`s selection on map
    /// </summary>
    /// <param name="cellNumber"></param>
    /// <param name="cardType"></param>
    /// <param name="houseCellsType"></param>
    /// <param name="charactersType"></param>
    /// <param name="PlayerID"></param>
    [Command]
    void CmdAskToCheckSelectedCell(int cellNumber, CardType cardType, HouseCellsType houseCellsType, CharactersType charactersType, int PlayerID, BoardType WhosBoard)
    {
        print("[Server] ==> CmdAskToCheckSelectedCell");

        HouseCellsType[] tempHouseCells = new HouseCellsType[49];
        CharactersType[] tempCharacterCells = new CharactersType[49];
        bool IsErorFound = false;
        int[] aroundIndex;

        bool IsOldHouseTile = false;

        #region Cheat Detection
        // ServerTurn has next turn which is 1 or 2 and is opposit the player id who choose. sum of these two number is always 3 1+2 or 2+1
        if (ServerTurn + PlayerID != 3)
        {
            print("Cheat--ServerTurn: " + ServerTurn + " *** PlayerID: " + PlayerID);
            //RpcTellCharacterCells(CharCells_P1_Server,false);
            //RpcTellHouseCells(HouseCells_P1_Server, IsOldHouseTile, false);
            return;
        }
        #endregion

        // If It is an attack to enemy
        if (WhosBoard == BoardType.EnemyBoard)
        {
            HouseCellsType[] tempEnemyHouseCells = new HouseCellsType[49];
            CharactersType[] tempEnemyCharacterCells = new CharactersType[49];

            if (PlayerID == 1)
            {
                tempEnemyHouseCells = HouseCells_P2_Server;
                tempEnemyCharacterCells = CharCells_P2_Server;
            }
            else if (PlayerID == 2)
            {
                tempEnemyHouseCells = HouseCells_P1_Server;
                tempEnemyCharacterCells = CharCells_P1_Server;
            }

            // Checking Ghost Attack

            HouseCellsType SelectedHouse = tempEnemyHouseCells[cellNumber];

            #region Check If The selected house is not empty or banned
            if (SelectedHouse == HouseCellsType.EmptyTile || SelectedHouse == HouseCellsType.BannedTile)
            {
                RpcTellError(PlayerID);
                return;
            }
            #endregion

            #region Check if the selected house has not any character inside
            if (tempEnemyCharacterCells[cellNumber] != CharactersType.Empty)
            {
                RpcTellError(PlayerID);
                return;
            }
            #endregion

            // Assume That Ghost is placable in the selected house of enemy

            if (PlayerID == 1)
            {
                #region decrease Ghost number of player after using
                Ghosts_P1_Server--;
                RpcTellGhostNumberUpdate(Ghosts_P1_Server, PlayerID);
                #endregion

                #region Decrease Enemy Score
                Score_P2_Server += CharType.CalculateCharacterScore(CharactersType.Ghost);
                #endregion

                #region Put Ghost in correspondig character cells array of enemy
                CharCells_P2_Server[cellNumber] = CharactersType.Ghost;
                #endregion

                #region Increase Placed Character of Player 2
                CharactersInHouse_P2_Server++;
                #endregion

                RpcTellScore(Score_P2_Server, 2);
                RpcTellGhostAttacked(cellNumber, 2);

            }
            else if (PlayerID == 2)
            {
                #region decrease Ghost number of player after using
                Ghosts_P2_Server--;
                RpcTellGhostNumberUpdate(Ghosts_P2_Server, PlayerID);
                #endregion

                #region Decrease Enemy Score
                Score_P1_Server += CharType.CalculateCharacterScore(CharactersType.Ghost);
                #endregion

                #region Put Ghost in correspondig character cells array of enemy
                CharCells_P1_Server[cellNumber] = CharactersType.Ghost;
                #endregion

                #region Increase Placed Character of Player 1
                CharactersInHouse_P1_Server++;
                #endregion


                RpcTellScore(Score_P1_Server, 1);
                RpcTellGhostAttacked(cellNumber, 1);
            }

            if (CharactersInHouse_P1_Server == 5 || CharactersInHouse_P2_Server == 5)
            {
                IsGameFinished = true;
                CmdCheckForFinishTheGame(IsGameFinished);
            }
            else
            {
                RpcTellTurn(ServerTurn);
            }


            return;
        }

        #region Wrong Input Detect And Reset Cells To Previous State And RPC It
        if (PlayerID == 1)
        {
            if (HouseCells_P1_Server[cellNumber] == HouseCellsType.BannedTile)
            {
                // Error: Select baned house
                RpcTellError(PlayerID);
                RpcTellCharacterCells(CharCells_P1_Server, false);
                RpcTellHouseCells(HouseCells_P1_Server, IsOldHouseTile, false);
                return;
            }

            if (CharCells_P1_Server[cellNumber] != CharactersType.Empty)
            {
                // Error: Select House tile with a character and wants to put card on it
                RpcTellError(PlayerID);
                RpcTellCharacterCells(CharCells_P1_Server, false);
                RpcTellHouseCells(HouseCells_P1_Server, IsOldHouseTile, false);
                return;
            }

            if (HouseCells_P1_Server[cellNumber] != HouseCellsType.EmptyTile && houseCellsType != HouseCellsType.EmptyTile)
            {
                // Error: Select filled House tile and want to put another house card on it
                RpcTellError(PlayerID);
                RpcTellCharacterCells(CharCells_P1_Server, false);
                RpcTellHouseCells(HouseCells_P1_Server, IsOldHouseTile, false);
                return;
            }

            if (HouseCells_P1_Server[cellNumber] == HouseCellsType.EmptyTile && charactersType != CharactersType.Empty)
            {
                // Error: Select Empty House tile and want to put character card on it
                RpcTellError(PlayerID);
                RpcTellCharacterCells(CharCells_P1_Server, false);
                RpcTellHouseCells(HouseCells_P1_Server, IsOldHouseTile, false);
                return;
            }
        }
        else if (PlayerID == 2)
        {
            if (HouseCells_P2_Server[cellNumber] == HouseCellsType.BannedTile)
            {
                // Error: Select baned house
                RpcTellError(PlayerID);
                RpcTellCharacterCells(CharCells_P2_Server, false);
                RpcTellHouseCells(HouseCells_P2_Server, IsOldHouseTile, false);
                return;
            }

            if (CharCells_P2_Server[cellNumber] != CharactersType.Empty)
            {
                // Error: Select House tile with a character and wants to put card on it
                RpcTellError(PlayerID);
                RpcTellCharacterCells(CharCells_P2_Server, false);
                RpcTellHouseCells(HouseCells_P2_Server, IsOldHouseTile, false);
                return;
            }

            if (HouseCells_P2_Server[cellNumber] != HouseCellsType.EmptyTile && houseCellsType != HouseCellsType.EmptyTile)
            {
                // Error: Select filled House tile and want to put another house card on it
                RpcTellError(PlayerID);
                RpcTellCharacterCells(CharCells_P2_Server, false);
                RpcTellHouseCells(HouseCells_P2_Server, IsOldHouseTile, false);
                return;
            }

            if (HouseCells_P2_Server[cellNumber] == HouseCellsType.EmptyTile && charactersType != CharactersType.Empty)
            {
                // Error: Select Empty House tile and want to put character card on it
                RpcTellError(PlayerID);
                RpcTellCharacterCells(CharCells_P2_Server, false);
                RpcTellHouseCells(HouseCells_P2_Server, IsOldHouseTile, false);
                return;
            }
        }
        #endregion



        // ToDo: Check if this action can be done based on game logic

        #region Assigning HouseCell and CharCell of coresponding player to tempHouseCells and tempCharacterCells
        if (PlayerID == 1)
        {
            tempHouseCells = HouseCells_P1_Server;
            tempCharacterCells = CharCells_P1_Server;
        }
        else if(PlayerID == 2)
        {
            tempHouseCells = HouseCells_P2_Server;
            tempCharacterCells = CharCells_P2_Server;
        }
        #endregion


        #region Check Character Card if it is a valid action or not
        if (charactersType != CharactersType.Empty && houseCellsType == HouseCellsType.EmptyTile)
        {
            switch (charactersType)
            {
                case CharactersType.RedGuy:

                    if (tempHouseCells[cellNumber] != HouseCellsType.RedTile && tempHouseCells[cellNumber] != HouseCellsType.OldRedTile && tempHouseCells[cellNumber] != HouseCellsType.Terrace)
                    {
                        IsErorFound = true;
                        break;
                    }

                    if(tempHouseCells[cellNumber] == HouseCellsType.Terrace)
                    {
                        aroundIndex = GameManager.TileIndex_Around(cellNumber);

                        for (int i = 0; i < aroundIndex.Length; i++)
                        {

                            if (tempCharacterCells[aroundIndex[i]] == CharactersType.PurpleNoRedGuy)
                            {
                                IsErorFound = true;
                                break;
                            }
                        }
                    }
                    break;
                case CharactersType.RedNoBlueGuy:

                    if (tempHouseCells[cellNumber] != HouseCellsType.RedTile && tempHouseCells[cellNumber] != HouseCellsType.OldRedTile && tempHouseCells[cellNumber] != HouseCellsType.Terrace)
                    {
                        IsErorFound = true;
                        break;
                    }

                    aroundIndex = GameManager.TileIndex_Around(cellNumber);
                    for (int i = 0; i < aroundIndex.Length; i++)
                    {
                        if(tempHouseCells[cellNumber] == HouseCellsType.Terrace)
                        {
                            if (tempCharacterCells[aroundIndex[i]] == CharactersType.PurpleNoRedGuy)
                            {
                                IsErorFound = true;
                                break;
                            }

                        }

                        if (tempHouseCells[aroundIndex[i]] == HouseCellsType.BlueTile || tempHouseCells[aroundIndex[i]] == HouseCellsType.OldBlueTile)
                        {
                            IsErorFound = true;
                            break;
                        }
                    }

                    break;
                case CharactersType.BlueGuy:

                    if (tempHouseCells[cellNumber] != HouseCellsType.BlueTile && tempHouseCells[cellNumber] != HouseCellsType.OldBlueTile && tempHouseCells[cellNumber] != HouseCellsType.Terrace)
                    {
                        IsErorFound = true;
                        break;
                    }

                    if (tempHouseCells[cellNumber] == HouseCellsType.Terrace)
                    {
                        aroundIndex = GameManager.TileIndex_Around(cellNumber);

                        for (int i = 0; i < aroundIndex.Length; i++)
                        {

                            if (tempCharacterCells[aroundIndex[i]] == CharactersType.RedNoBlueGuy)
                            {
                                IsErorFound = true;
                                break;
                            }
                        }

                    }

                    break;
                case CharactersType.BlueNoYellow:

                    if (tempHouseCells[cellNumber] != HouseCellsType.BlueTile && tempHouseCells[cellNumber] != HouseCellsType.OldBlueTile && tempHouseCells[cellNumber] != HouseCellsType.Terrace)
                    {
                        IsErorFound = true;
                        break;
                    }


                    aroundIndex = GameManager.TileIndex_Around(cellNumber);
                    for (int i = 0; i < aroundIndex.Length; i++)
                    {
                        if (tempHouseCells[cellNumber] == HouseCellsType.Terrace)
                        {
                            if (tempCharacterCells[aroundIndex[i]] == CharactersType.RedNoBlueGuy)
                            {
                                IsErorFound = true;
                                break;
                            }
                        }

                        if (tempHouseCells[aroundIndex[i]] == HouseCellsType.YellowTile || tempHouseCells[aroundIndex[i]] == HouseCellsType.OldYellowTile)
                        {
                            IsErorFound = true;
                            break;
                        }
                    }

                    break;
                case CharactersType.PurpuleGuy:

                    if (tempHouseCells[cellNumber] != HouseCellsType.PurpleTile && tempHouseCells[cellNumber] != HouseCellsType.OldPurpleTile && tempHouseCells[cellNumber] != HouseCellsType.Terrace)
                        IsErorFound = true;


                    break;
                case CharactersType.PurpleNoRedGuy:

                    if (tempHouseCells[cellNumber] != HouseCellsType.PurpleTile && tempHouseCells[cellNumber] != HouseCellsType.OldPurpleTile && tempHouseCells[cellNumber] != HouseCellsType.Terrace)
                    {
                        IsErorFound = true;
                        break;
                    }

                    aroundIndex = GameManager.TileIndex_Around(cellNumber);
                    for (int i = 0; i < aroundIndex.Length; i++)
                    {

                        if (tempHouseCells[aroundIndex[i]] == HouseCellsType.RedTile || tempHouseCells[aroundIndex[i]] == HouseCellsType.OldRedTile)
                        {
                            IsErorFound = true;
                            break;
                        }
                    }

                    break;
                case CharactersType.OldGuy:

                    if (GameManager.IsTileInFirstRow(cellNumber) == false)
                        IsErorFound = true;

                    if(GameManager.TileIndex_Above(cellNumber) != -1)
                    {
                        if(tempHouseCells[GameManager.TileIndex_Above(cellNumber)] != HouseCellsType.EmptyTile && tempHouseCells[GameManager.TileIndex_Above(cellNumber)] != HouseCellsType.BannedTile)
                            IsErorFound = true;
                    }

                    break;
                case CharactersType.PenthouseGuy:

                    if (tempHouseCells[cellNumber] != HouseCellsType.PentHouse)
                        IsErorFound = true;

                    break;
                default:
                    break;
            }

        }
        #endregion

        #region Check House Card if it is a valid action or not
        else if (charactersType == CharactersType.Empty && houseCellsType != HouseCellsType.EmptyTile)
        {

            #region Check: Terrace rules
            if(houseCellsType == HouseCellsType.Terrace)
            {
                #region Check: If Terace Tile is in or higher than second level
                if(!GameManager.IsTerraceTileAllowed(cellNumber))
                {
                    RpcTellError(PlayerID);
                    return;
                }
                #endregion

                #region Check: If vertical or horizontal connection is OK
                if (tempHouseCells[GameManager.TileIndex_Below(cellNumber)] == HouseCellsType.EmptyTile )
                {
                    int[] SidesIndex = GameManager.TileIndex_Sides(cellNumber);
                    int bannedTilesInSides = 0;
                    int emptyTilesInSides = 0;
                    int sidesAvailableTile = SidesIndex.Length;

                    for (int i = 0; i < sidesAvailableTile; i++)
                    {
                        if (tempHouseCells[SidesIndex[i]] == HouseCellsType.EmptyTile)
                            emptyTilesInSides++;
                        else if (tempHouseCells[SidesIndex[i]] == HouseCellsType.BannedTile)
                            bannedTilesInSides++;
                    }
                    #region If two sides are empty
                    if (sidesAvailableTile == 2 && emptyTilesInSides == 2)
                    {
                        RpcTellError(PlayerID);
                        return;
                    }
                    #endregion

                    #region If one side is empty and other side is edge
                    else if (sidesAvailableTile == 1 && emptyTilesInSides == 1)
                    {
                        RpcTellError(PlayerID);
                        return;
                    }
                    #endregion

                    #region If one side is empty and other side is banned
                    else if (emptyTilesInSides == 1 && bannedTilesInSides == 1)
                    {
                        RpcTellError(PlayerID);
                        return;
                    }
                    #endregion

                    #region If two sides are banned
                    else if (bannedTilesInSides == 2)
                    {
                        RpcTellError(PlayerID);
                        return;
                    }
                    #endregion

                }
                #endregion

            }
            #endregion

            #region Check: Vertical and Horizontal connection of all tiles except Terrace
            else
            {
                #region Check: Vertical connection check
                if (!GameManager.IsTileInFirstRow(cellNumber))
                {
                    if (tempHouseCells[GameManager.TileIndex_Below(cellNumber)] == HouseCellsType.EmptyTile)
                    {
                        RpcTellError(PlayerID);
                        return;
                    }
                }
                #endregion

                #region Check: Horizontal connection check
                
                int[] SelectedRowIndex = GameManager.TileIndex_InTheRow(cellNumber);
                bool IsThereAnotherHouseInTheRow = false;

                for (int i = 0; i < SelectedRowIndex.Length; i++)
                {
                    if (tempHouseCells[SelectedRowIndex[i]] != HouseCellsType.EmptyTile && tempHouseCells[SelectedRowIndex[i]] != HouseCellsType.BannedTile)
                    {
                        IsThereAnotherHouseInTheRow = true;
                        break;
                    }
                }

                if(GameManager.IsTileInFirstRow(cellNumber) && IsThereAnotherHouseInTheRow)
                {
                    int[] SidesIndex = GameManager.TileIndex_Sides(cellNumber);
                    bool IsThereHouseTileInSides = false;

                    for (int i = 0; i < SidesIndex.Length; i++)
                    {
                        if(tempHouseCells[SidesIndex[i]] != HouseCellsType.EmptyTile)
                        {
                            IsThereHouseTileInSides = true;
                            break;
                        }
                    }

                    if( !IsThereHouseTileInSides )
                    {
                        RpcTellError(PlayerID);
                        return;
                    }
                }
                /*
                if (IsThereAnotherHouseInTheRow)
                {
                    int[] SidesIndex = GameManager.TileIndex_Sides(cellNumber);
                    int bannedTilesInSides = 0;
                    int emptyTilesInSides = 0;
                    int sidesAvailableTile = SidesIndex.Length;

                    for (int i = 0; i < sidesAvailableTile; i++)
                    {
                        if (tempHouseCells[SidesIndex[i]] == HouseCellsType.EmptyTile)
                            emptyTilesInSides++;
                        else if (tempHouseCells[SidesIndex[i]] == HouseCellsType.BannedTile)
                            bannedTilesInSides++;
                    }

                    if (sidesAvailableTile == 2 && emptyTilesInSides == 2)
                    {
                        RpcTellError(PlayerID);
                        return;
                    }
                    else if (emptyTilesInSides == 1 && bannedTilesInSides == 1)
                    {
                        RpcTellError(PlayerID);
                        return;
                    }
                    else if (sidesAvailableTile == 1 && emptyTilesInSides == 1)
                    {
                        RpcTellError(PlayerID);
                        return;
                    }

                    // Here...
                }
                else if (!IsThereAnotherHouseInTheRow && !GameManager.IsTileInFirstRow(cellNumber))
                {
                    if (tempHouseCells[GameManager.TileIndex_Below(cellNumber)] == HouseCellsType.BannedTile)
                    {
                        RpcTellError(PlayerID);
                        return;
                    }
                }
                */
                #endregion

            }
            #endregion

            #region Check: If there is old House in three house below and there isnt baned house between them, Then this is an unvalid one
            if (GameManager.ThreeTileBelowSelectedIndex(cellNumber) != -1)
            {
                HouseCellsType ThreeHouseBelow = tempHouseCells[GameManager.ThreeTileBelowSelectedIndex(cellNumber)];

                if (ThreeHouseBelow == HouseCellsType.OldBlueTile || ThreeHouseBelow == HouseCellsType.OldPurpleTile || 
                    ThreeHouseBelow == HouseCellsType.OldRedTile || ThreeHouseBelow == HouseCellsType.OldYellowTile )
                {
                    print("Three tile below index is: " + GameManager.ThreeTileBelowSelectedIndex(cellNumber));

                    int[] twoIndexBelow = GameManager.TwooTileBelowSelectedIndex(cellNumber);
                    bool IsThereBanedOrEmptyHouseBelow = false;

                    for (int i = 0; i < twoIndexBelow.Length; i++)
                    {
                        if (tempHouseCells[twoIndexBelow[i]] == HouseCellsType.BannedTile || tempHouseCells[twoIndexBelow[i]] == HouseCellsType.EmptyTile)
                        {
                            IsThereBanedOrEmptyHouseBelow = true;
                            break;
                        }
                    }

                    if (IsThereBanedOrEmptyHouseBelow == false)
                    {
                        RpcTellError(PlayerID);
                        return;
                    }
                }

            }
            #endregion

            #region Check: If the below tile is roof tile, Then this is an unvalid one
            if(GameManager.TileIndex_Below(cellNumber) != -1)
            {
                if (tempHouseCells[GameManager.TileIndex_Below(cellNumber)] == HouseCellsType.PentHouse)
                {
                    RpcTellError(PlayerID);
                    return;
                }
            }
            #endregion

            #region Check: If in the below tile there is an Old Character, Then this is an unvalid one
            if (GameManager.TileIndex_Below(cellNumber) != -1)
            {
                if (tempCharacterCells[GameManager.TileIndex_Below(cellNumber)] == CharactersType.OldGuy)
                {
                    RpcTellError(PlayerID);
                    return;
                }
            }
            #endregion

            #region Check: When Penthouse or Red or Blue or Yellow card is selected
            switch (houseCellsType)
            {
                case HouseCellsType.BlueTile:

                    aroundIndex = GameManager.TileIndex_Around(cellNumber);
                    for (int i = 0; i < aroundIndex.Length; i++)
                    {
                        if (tempCharacterCells[aroundIndex[i]] == CharactersType.RedNoBlueGuy)
                        {
                            IsErorFound = true;
                            break;
                        }
                    }

                    break;

                case HouseCellsType.OldBlueTile:

                    aroundIndex = GameManager.TileIndex_Around(cellNumber);
                    for (int i = 0; i < aroundIndex.Length; i++)
                    {
                        if (tempCharacterCells[aroundIndex[i]] == CharactersType.RedNoBlueGuy)
                        {
                            IsErorFound = true;
                            break;
                        }
                    }

                    break;

                case HouseCellsType.RedTile:

                    aroundIndex = GameManager.TileIndex_Around(cellNumber);
                    for (int i = 0; i < aroundIndex.Length; i++)
                    {
                        if (tempCharacterCells[aroundIndex[i]] == CharactersType.PurpleNoRedGuy)
                        {
                            IsErorFound = true;
                            break;
                        }
                    }
                    
                    break;

                case HouseCellsType.OldRedTile:

                    aroundIndex = GameManager.TileIndex_Around(cellNumber);
                    for (int i = 0; i < aroundIndex.Length; i++)
                    {
                        if (tempCharacterCells[aroundIndex[i]] == CharactersType.PurpleNoRedGuy)
                        {
                            IsErorFound = true;
                            break;
                        }
                    }

                    break;

                case HouseCellsType.YellowTile:

                    aroundIndex = GameManager.TileIndex_Around(cellNumber);
                    for (int i = 0; i < aroundIndex.Length; i++)
                    {
                        if (tempCharacterCells[aroundIndex[i]] == CharactersType.BlueNoYellow)
                        {
                            IsErorFound = true;
                            break;
                        }
                    }

                    break;

                case HouseCellsType.OldYellowTile:

                    aroundIndex = GameManager.TileIndex_Around(cellNumber);
                    for (int i = 0; i < aroundIndex.Length; i++)
                    {
                        if (tempCharacterCells[aroundIndex[i]] == CharactersType.BlueNoYellow)
                        {
                            IsErorFound = true;
                            break;
                        }
                    }

                    break;

                case HouseCellsType.PentHouse:

                    if (GameManager.IsRoofTileAllowed(cellNumber) == false)
                        IsErorFound = true;

                    break;
                default:
                    break;
            }
            #endregion
        }
        #endregion


        if (IsErorFound)
        {
            RpcTellError(PlayerID);
            return;
        }

        // In This State We Assume That The Action Is valid :

        if (PlayerID == 1)
        {

            #region if Player 1 Select a Character Card
            if (charactersType != CharactersType.Empty && houseCellsType == HouseCellsType.EmptyTile)
            {
                CharactersInHouse_P1_Server++;
                Score_P1_Server += CharType.CalculateCharacterScore(charactersType);

                CharCells_P1_Server[cellNumber] = charactersType;
                CharacterCardsDeckInGame_Server.Remove(charactersType);
                CharacterCardsDeckInGame_Server.Add(CharactersType.Empty);

                RpcTellScore(Score_P1_Server, 1);
                RpcTellCharacterInGameDeck(CharacterCardsDeckInGame_Server.ToArray());
                RpcTellCharacterCells(CharCells_P1_Server,true);

                //print("CharactersInHouse_P1_Server: " + CharactersInHouse_P1_Server);
                if (CharactersInHouse_P1_Server == 5)
                {
                    // Finish the game 
                    IsGameFinished = true;
                }

            }
            #endregion

            #region else if Player 1 Select a House Card
            else if (houseCellsType != HouseCellsType.EmptyTile && charactersType == CharactersType.Empty)
            {
                HouseCells_P1_Server[cellNumber] = houseCellsType;
                HouseCardsDeckInGame_Server.Remove(houseCellsType);
                HouseCardsDeckInGame_Server.Add(HouseCellsType.EmptyTile);

                if (houseCellsType == HouseCellsType.OldBlueTile || houseCellsType == HouseCellsType.OldPurpleTile ||
                    houseCellsType == HouseCellsType.OldRedTile || houseCellsType == HouseCellsType.OldYellowTile)
                {
                    IsOldHouseTile = true;
                    Ghosts_P1_Server++;
                    RpcTellGhostNumberUpdate(Ghosts_P1_Server, 1);
                    print("Server ==> Call RPC for Ghost Player 1");

                }
                else
                {
                    IsOldHouseTile = false;
                }
                

                RpcTellHouseInGameDeck(HouseCardsDeckInGame_Server.ToArray());
                RpcTellHouseCells(HouseCells_P1_Server, IsOldHouseTile,true);
            }
            #endregion

            #region else if Player 1 Select an empty Card
            else
            {
                print("Empty Card Selected");
                return;
            }

            #endregion
        }
        else if (PlayerID == 2)
        {
            #region if Player 2 Select a Character Card

            if (charactersType != CharactersType.Empty && houseCellsType == HouseCellsType.EmptyTile)
            {
                CharactersInHouse_P2_Server++;
                Score_P2_Server += CharType.CalculateCharacterScore(charactersType);

                CharCells_P2_Server[cellNumber] = charactersType;
                CharacterCardsDeckInGame_Server.Remove(charactersType);
                CharacterCardsDeckInGame_Server.Add(CharactersType.Empty);

                RpcTellScore(Score_P2_Server, 2);
                RpcTellCharacterInGameDeck(CharacterCardsDeckInGame_Server.ToArray());
                RpcTellCharacterCells(CharCells_P2_Server,true);

                print("CharactersInHouse_P2_Server: " + CharactersInHouse_P2_Server);
                if (CharactersInHouse_P2_Server == 5)
                {
                    // Finish the game 
                    IsGameFinished = true;
                }
            }

            #endregion

            #region else if Player 2 Select a House Card

            else if (houseCellsType != HouseCellsType.EmptyTile && charactersType == CharactersType.Empty)
            {
                HouseCells_P2_Server[cellNumber] = houseCellsType;
                HouseCardsDeckInGame_Server.Remove(houseCellsType);
                HouseCardsDeckInGame_Server.Add(HouseCellsType.EmptyTile);

                if (houseCellsType == HouseCellsType.OldBlueTile || houseCellsType == HouseCellsType.OldPurpleTile ||
                    houseCellsType == HouseCellsType.OldRedTile || houseCellsType == HouseCellsType.OldYellowTile)
                {
                    IsOldHouseTile = true;
                    Ghosts_P2_Server++;
                    RpcTellGhostNumberUpdate(Ghosts_P2_Server, 2);
                   // print("Server ==> Call RPC for Ghost Player 2");

                }
                else
                {
                    IsOldHouseTile = false;
                }


                RpcTellHouseInGameDeck(HouseCardsDeckInGame_Server.ToArray());
                RpcTellHouseCells(HouseCells_P2_Server, IsOldHouseTile,true);

            }

            #endregion

            #region else if Player 2 Select an empty Card

            else
            {
                print("Empty Card Selected");
                return;
            }
            #endregion
        }

        if(IsGameFinished)
        {
            int negativeScore_P1 = GameManager.CalculateNegativeScore(CharCells_P1_Server, HouseCells_P1_Server);
            int negativeScore_P2 = GameManager.CalculateNegativeScore(CharCells_P2_Server, HouseCells_P2_Server);
            int score_P1 = Score_P1_Server + negativeScore_P1;
            int score_P2 = Score_P2_Server + negativeScore_P2;

            if (score_P1 > score_P2)
            {
                RpcTellGameFinished(1, score_P1, Score_P1_Server, negativeScore_P1, 2, score_P2, Score_P2_Server, negativeScore_P2);
            }
            else
            {
                RpcTellGameFinished(2, score_P2, Score_P2_Server, negativeScore_P2, 1, score_P1, Score_P1_Server, negativeScore_P1);
            }

        }
        else if(!IsGameFinished)
        {
            RpcTellTurn(ServerTurn);
            CmdAskToFillEmptyCharInGameDeck(false);
            CmdAskToFillEmptyHouseInGameDeck(false);

        }

    }
    #endregion

    #region CmdAskToCheckMultipleCharsOnSelectedCells(int[] cellNumbers, CharactersType character, int myTurnID)
    [Command]
    void CmdAskToCheckMultipleCharsOnSelectedCells(int[] cellNumbers, CharactersType character, int myTurnID)
    {

        HouseCellsType[] houseTemp = new HouseCellsType[49];
        CharactersType[] charTemp = new CharactersType[49];
        HouseCellsType[] SelectedHouses = new HouseCellsType[cellNumbers.Length];

        if (myTurnID == 1)
        {
            houseTemp = HouseCells_P1_Server;
            charTemp = CharCells_P1_Server;
        }
        else if(myTurnID == 2)
        {
            houseTemp = HouseCells_P2_Server;
            charTemp = CharCells_P2_Server;
        }

        for (int i = 0; i < cellNumbers.Length; i++)
        {
            #region Check Cells to be empty of character
            if (charTemp[cellNumbers[i]] != CharactersType.Empty)
            {
                RpcTellError(myTurnID);
                return;
            }
            #endregion

            #region Putting All the selected house tile in the SelectedHouses Array
            SelectedHouses[i] = houseTemp[cellNumbers[i]];
            #endregion
        }

        #region Check Cells to be near each other
        if ( !GameManager.IsTileIndexesNearEachOther(character, cellNumbers) )
        {
            RpcTellError(myTurnID);
            return;
        }
        #endregion

        #region Check Cells to be Colored Tiled and the same color
        if ( !GameManager.IsTileIndexesTheSameColor(SelectedHouses) )
        {
            RpcTellError(myTurnID);
            return;
        }
        #endregion

        #region When a multiplie unit character is legally selected by Client ID 1
        if (myTurnID == 1)
        {
            CharactersInHouse_P1_Server++;
            Score_P1_Server += CharType.CalculateCharacterScore(character);

            CharactersType[] tempCharacterUnits = GameManager.OrderHouseTilesForMultipleUnitCharacter(cellNumbers, character);

            for (int i = 0; i < cellNumbers.Length; i++)
            {
                CharCells_P1_Server[cellNumbers[i]] = tempCharacterUnits[i];
            }

            RpcTellScore(Score_P1_Server, 1);
            RpcTellCharacterCells(CharCells_P1_Server, true);

            if (CharactersInHouse_P1_Server == 5)
            {
                // Finish the game 
                IsGameFinished = true;
            }

        }
        #endregion

        #region When a multiplie unit character is legally selected by Client ID 2
        else if (myTurnID == 2)
        {
            CharactersInHouse_P2_Server++;
            Score_P2_Server += CharType.CalculateCharacterScore(character);

            CharactersType[] tempCharacterUnits = GameManager.OrderHouseTilesForMultipleUnitCharacter(cellNumbers, character);

            for (int i = 0; i < cellNumbers.Length; i++)
            {
                CharCells_P2_Server[cellNumbers[i]] = tempCharacterUnits[i];
            }

            RpcTellScore(Score_P2_Server, 2);
            RpcTellCharacterCells(CharCells_P2_Server, true);

            if (CharactersInHouse_P2_Server == 5)
            {
                // Finish the game 
                IsGameFinished = true;
            }

        }
        #endregion



        if (IsGameFinished)
        {
            int negativeScore_P1 = GameManager.CalculateNegativeScore(CharCells_P1_Server, HouseCells_P1_Server);
            int negativeScore_P2 = GameManager.CalculateNegativeScore(CharCells_P2_Server, HouseCells_P2_Server);
            int score_P1 = Score_P1_Server + negativeScore_P1;
            int score_P2 = Score_P2_Server + negativeScore_P2;

            if (score_P1 > score_P2)
            {
                RpcTellGameFinished(1, score_P1, Score_P1_Server, negativeScore_P1, 2, score_P2, Score_P2_Server, negativeScore_P2);
            }
            else
            {
                RpcTellGameFinished(2, score_P2, Score_P2_Server, negativeScore_P2, 1, score_P1, Score_P1_Server, negativeScore_P1);
            }

        }
        else if (!IsGameFinished)
        {
            CharacterCardsDeckInGame_Server.Remove(character);
            CharacterCardsDeckInGame_Server.Add(CharactersType.Empty);

            RpcTellCharacterInGameDeck(CharacterCardsDeckInGame_Server.ToArray());
            RpcTellTurn(ServerTurn);
            CmdAskToFillEmptyCharInGameDeck(false);
            CmdAskToFillEmptyHouseInGameDeck(false);
        }

    }
    #endregion

    #region CmdResetServerData(int playerTurn)
    /// <summary>
    /// Reset All Data after reseting game
    /// </summary>
    /// <param name="playerTurn">in order not to be called two time use a player id</param>
    [Command]
    public void CmdResetServerData()
    {
        //print("ResetData [Server]");

        Ghosts_P1_Server = 0;
        Ghosts_P2_Server = 0;

        HouseCells_P1_Server = new HouseCellsType[49];
        HouseCells_P2_Server = new HouseCellsType[49];

        CharCells_P1_Server = new CharactersType[49];
        CharCells_P2_Server = new CharactersType[49];

        HouseCardsDeckInGame_Server = new List<HouseCellsType>();
        CharacterCardsDeckInGame_Server = new List<CharactersType>();

        CharactersInHouse_P1_Server = 0;
        CharactersInHouse_P2_Server = 0;

        Score_P1_Server = 0;
        Score_P2_Server = 0;

        HousesDeckList_Server = new List<HouseCellsType>();
        CharactersDeckList_Server = new List<CharactersType>();

        IsNoFirstRandomTurn = true;

        IsNoFirstRandomTurn = true;
        ServerTurn = 0;


    }
    #endregion

    #region CmdCheckForFinishTheGame(bool IsItFinished)
    [Command]
    public void CmdCheckForFinishTheGame(bool IsItFinished)
    {

        if (IsItFinished)
        {
            int negativeScore_P1 = GameManager.CalculateNegativeScore(CharCells_P1_Server, HouseCells_P1_Server);
            int negativeScore_P2 = GameManager.CalculateNegativeScore(CharCells_P2_Server, HouseCells_P2_Server);
            int score_P1 = Score_P1_Server + negativeScore_P1;
            int score_P2 = Score_P2_Server + negativeScore_P2;

            if (score_P1 > score_P2)
            {
                RpcTellGameFinished(1, score_P1, Score_P1_Server, negativeScore_P1, 2, score_P2, Score_P2_Server, negativeScore_P2);
            }
            else
            {
                RpcTellGameFinished(2, score_P2, Score_P2_Server, negativeScore_P2, 1, score_P1, Score_P1_Server, negativeScore_P1);
            }

        }
        else if (!IsItFinished)
        {
            RpcTellTurn(ServerTurn);
            CmdAskToFillEmptyCharInGameDeck(false);
            CmdAskToFillEmptyHouseInGameDeck(false);

        }

    }
    #endregion


    #endregion


    #region RPCs

    #region RpcTellActiveClients(int Conns)
    /// <summary>
    /// Update ActiveConnections Variable on each Client
    /// </summary>
    /// <param name="Conns"></param>
    [ClientRpc]
    void RpcTellActiveClients(int Conns)
    {
        ActiveConnections = Conns;
    }
    #endregion

    #region RpcTellHouseCells(HouseCellsType[] cellhouseArray, bool IsOldTile, bool IsThereChange)
    /// <summary>
    /// Tell Clients about House Cells Array
    /// </summary>
    /// <param name="cellhouseArray"></param>
    /// <param name="IsOldTile"></param>
    /// <param name="IsThereChange"></param>
    [ClientRpc]
    public void RpcTellHouseCells(HouseCellsType[] cellhouseArray, bool IsOldTile, bool IsThereChange)
    {
        if (IsThereChange)
        {
            if (IsOldTile)
            {
                IsOldHousePlacementDone = true;
            }
            else
            {
                IsHousePlacementDone = true;
            }
        }

        MyHouseCells = cellhouseArray;

        IsReadyForUpdateHouseCellsOnScreen = true;
    }
    #endregion

    #region RpcTellCharacterCells(CharactersType[] charactersCellType, bool IsChanged)
    /// <summary>
    /// Tell Clients about Character Cells Array
    /// </summary>
    /// <param name="charactersCellType"></param>
    /// <param name="IsChanged"></param>
    [ClientRpc]
    public void RpcTellCharacterCells(CharactersType[] charactersCellType, bool IsChanged)
    {
        if(IsChanged)
        {
            IsCharacterPlacementDone = true;
        }

        MyCharCells = charactersCellType;

        IsReadyForUpdateCharacterCellsOnScreen = true;
    }
    #endregion

    [ClientRpc]
    public void RpcTellGhostAttacked(int AttackedIndex, int PlayerID_GhostAttacked)
    {
        GhostAttacked_PlayerID = PlayerID_GhostAttacked;

        GhostAttackedIndex = AttackedIndex;

        IsGhostAttackDone = true;
    }


    #region RpcTellCharacterInGameDeck(CharactersType[] characterCardsDeckInGame_Server)
    /// <summary>
    /// Tell Clients About In-Game Character Deck
    /// </summary>
    /// <param name="characterCardsDeckInGame_Server"></param>
    [ClientRpc]
    public void RpcTellCharacterInGameDeck(CharactersType[] characterCardsDeckInGame_Server)
    {
        CharacterCardsInGameDeck = characterCardsDeckInGame_Server;
        IsReadyForUpdateCharacterCardsOnScreen = true;
    }

    #endregion

    #region RpcTellHouseInGameDeck(HouseCellsType[] houseCardsDeckInGame_Server)
    /// <summary>
    /// Tell Clients About In-Game House Deck
    /// </summary>
    /// <param name="houseCardsDeckInGame_Server"></param>
    [ClientRpc]
    public void RpcTellHouseInGameDeck(HouseCellsType[] houseCardsDeckInGame_Server)
    {
        HouseCardsInGameDeck = houseCardsDeckInGame_Server;
        IsReadyForUpdateHouseCardsOnScreen = true;
    }

    #endregion

    #region RpcTellTurn(int NextTurn)
    /// <summary>
    /// Tell Clients About server decision on whos turn is it now
    /// </summary>
    /// <param name="NextTurn"></param>
    [ClientRpc]
    void RpcTellTurn(int NextTurn)
    {
        ServerTurn = NextTurn;
        IsGameTurnSet = true;
    }
    #endregion

    #region RpcTellError(int turn)
    /// <summary>
    /// Tell Clients About Error must shown on who makes it
    /// </summary>
    /// <param name="PlayerTurn"></param>
    [ClientRpc]
    void RpcTellError(int PlayerTurn)
    {
        ErrorForPlayerID = PlayerTurn;
        IsErrorMustShown = true;
    }
    #endregion

    [ClientRpc]
    public void RpcTellGameFinished(int WinnerID_S, int WinnerScor_S, int WinnerPScore_S, int WinnerNScore_S, int LoserID_S, int LoserScor_S, int LoserPScore_S, int LoserNScore_S)
    {
        WinnerID = WinnerID_S;
        WinnerScore = WinnerScor_S;
        WinnerPpoint = WinnerPScore_S;
        WinnerNpoint = WinnerNScore_S;
        LoserID = LoserID_S;
        LoserScore = LoserScor_S;
        LoserPpoint = LoserPScore_S;
        LoserNpoint = LoserNScore_S;


        IsGameFinished = true;
    }

    [ClientRpc]
    void RpcTellOneLeft(int playerID)
    {
        print("RPC: One Left");

        ThePlayerIDWhoLeft = playerID;
        IsOnePlayerLeftTheGame = true;
    }

    [ClientRpc]
    public void RpcTellScore(int score, int PlayerTurn)
    {
        ScoreForPlayerID = PlayerTurn;
        Score = score;
        IsScoreChanged = true;
    }

    [ClientRpc]
    public void RpcTellGhostNumberUpdate(int GhostsNumber, int GhostsID)
    {
        GhostChangedID = GhostsID;
        tempGhost = GhostsNumber;
        IsReadyForUpdateGhostsOnScreen = true;
        //print("RPC ==> Ghost Update Flag: " + IsReadyForUpdateGhostsOnScreen);
    }

    #endregion




    // To-Do:
    // 1- Creat a custom HUD network manager
    // 2- When Stop or disconnection button on HUD network manager pressed, gameManagerscript.Initialazation(true) should be called
    // 3- Place back button in waiting room and room is full panel

    // ==> To Continiue First : Handle When a player left the game the other should be noticed and end the game


}
