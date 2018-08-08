﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


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

    private bool IsReadyForUpdateCellsOnScreen = false;
    private bool IsReadyForUpdateHouseCardsOnScreen = false;
    private bool IsReadyForUpdateCharacterCardsOnScreen = false;
    public static bool IsGameTurnSet = false;


    private PlayerConnection enemyConnection;

    public HouseCellsType[] MyHouseCells = new HouseCellsType[49];
    public CharactersType[] MyCharCells = new CharactersType[49];


    static HouseCellsType[] HouseCardsInGameDeck = new HouseCellsType[4];
    static CharactersType[] CharacterCardsInGameDeck = new CharactersType[4];

    public CardType CardTypeSelected;
    public HouseCellsType HouseCardSelected;
    public CharactersType CharacterdCardSelected;


    // Server-Side Arrays

    static HouseCellsType[] HouseCells_P1_Server = new HouseCellsType[49];
    static HouseCellsType[] HouseCells_P2_Server = new HouseCellsType[49];

    static CharactersType[] CharCells_P1_Server = new CharactersType[49];
    static CharactersType[] CharCells_P2_Server = new CharactersType[49];

    static public List<HouseCellsType> HouseCardsDeckInGame_Server = new List<HouseCellsType>();
    static public List<CharactersType> CharacterCardsDeckInGame_Server = new List<CharactersType>();

    public static List<HouseCellsType> HousesDeckList_Server = new List<HouseCellsType>();
    public static List<CharactersType> CharactersDeckList_Server = new List<CharactersType>();

    static bool IsNoFirstRandomTurn = true;


    Text serverText;

    #endregion

    #region Start()
    void Start()
    {

        serverText = GameObject.FindGameObjectWithTag("ServerText").GetComponent<Text>();

        if (!isLocalPlayer)
        {
            return;
        }

        gameManagerscript = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        CmdAskToTellActiveConnection();

        CmdAskToSetPlayerTurn();

        //First change the name locally then on the server
        UserName = PlayerPrefs.GetString(MenuManager.UserNamePlayerPrefs);

        CmdAskToSetUserName(UserName);

        SetCardSelectedToNull();

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

        #region Wait untill MyTurnId is set by RPC to 1 or 2
        if (MyTurnID == 0)
        {
            return;
        }
        #endregion

        ShowAnimationBasedOnActiveConnectionNumbers();

        #region Simiulating the cell arrays of mine and my enemy board on screen based on flag: IsReadyForUpdateCellsOnScreen

        if (IsReadyForUpdateCellsOnScreen)
        {
            // Simiulating my cell array
            gameManagerscript.UpdateHouseTileMap(MyHouseCells);

            // Simiulating my enemy cell array
            StartCoroutine(CreateEnemyMap(0.5f));


            // Set Flag For Update Screen to flase
            IsReadyForUpdateCellsOnScreen = false;

        }
        #endregion

        #region Simiulating the character decks on screen based on flag: IsReadyForUpdateCharacterCardsOnScreen

        if (IsReadyForUpdateCharacterCardsOnScreen)
        {
            gameManagerscript.UpdateCharacterDeck(CharacterCardsInGameDeck);
            IsReadyForUpdateCharacterCardsOnScreen = false;
        }

        #endregion

        #region Simiulating the house decks on screen based on flag: IsReadyForUpdateHouseCardsOnScreen

        if (IsReadyForUpdateHouseCardsOnScreen)
        {
            gameManagerscript.UpdateHouseDeck(HouseCardsInGameDeck);
            IsReadyForUpdateHouseCardsOnScreen = false;
        }

        #endregion

        #region Enabling or disabling decks to select based on if it is my turn or not

        if (IsGameTurnSet)
        {
            if (ServerTurn == MyTurnID)
            {
                #region When its your turn, First update card deck and your enemy map

                gameManagerscript.UpdateHouseDeck(HouseCardsInGameDeck);
                gameManagerscript.UpdateCharacterDeck(CharacterCardsInGameDeck);
                StartCoroutine(CreateEnemyMap(0.5f));

                #endregion

                gameManagerscript.EnableDecks();
                CmdAskToUpdateItsTurnVariable();

                IsGameTurnSet = false;
            }
            else if (ServerTurn != MyTurnID)
            {
                gameManagerscript.DisableDecks();
                IsGameTurnSet = false;
            }

        }

        #endregion

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


    #region CreateEnemyMap(float waitTime) [Coroutine]
    /// <summary>
    /// Courotine for creating enemy map and show the name after a delay to prevent server problem
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator CreateEnemyMap(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameManagerscript.SetEnemyName(EnemyName);
        gameManagerscript.UpdateHouseTileMap(enemyConnection.MyHouseCells, false);
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
        GetEnemyScript();

        setEnemyName();

        gameManagerscript.SetMyName(UserName);

        gameManagerscript.ShowGameLoading();

        gameManagerscript.SetDiactiveUIBeginingWaitingPanel();

        CmdAskToCreateDeckLists(MyTurnID);

        CmdAskToFillEmptyCharInGameDeck();

        CmdAskToFillEmptyHouseInGameDeck(true);

        CmdAskToCreateHouseTilesArray(MyTurnID);

        StartCoroutine(RequestForFirstTurnSet(0.5f));

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

   public void CommandToCheckSelectedCell(int cellNumber)
    {
        CmdAskToCheckSelectedCell(cellNumber, CardTypeSelected, HouseCardSelected, CharacterdCardSelected, MyTurnID);
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

            // Checking not to use repeated cell for NoConstructionCell
            do
            {
                RepeatedCell = false;
                randomCell = UnityEngine.Random.Range(0, 49);

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


        RpcTellHouseCells(cellhouseTemp);
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

            for (int i = 0; i < EnumCharacterLenght; i++)
            {
                CharactersType CharacterTemp = (CharactersType)i;

                // These Character shouldnt be added to deck based on game design
                if(CharacterTemp == CharactersType.Ghost || CharacterTemp == CharactersType.Empty || CharacterTemp == CharactersType.TripleGuys)
                {
                    continue;
                }

                for (int j = 0; j < 3; j++)  // Each card has 3 Ratio in a deck
                {
                    CharactersDeckList_Server.Add(CharacterTemp);
                }

            }


            #endregion

            #region Creating HouseList Deck

            int EnumHouseLenght = Enum.GetNames(typeof(HouseCellsType)).Length;

            for (int i = 0; i < EnumHouseLenght; i++)
            {
                HouseCellsType HouseCellTemp = (HouseCellsType)i;
                int CardRatioNumber;

                // Assign proper card ratio to each card
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
                        CardRatioNumber = 8;
                        break;
                    case HouseCellsType.Terrace:
                        CardRatioNumber = 10;
                        break;
                    case HouseCellsType.Garden:
                        CardRatioNumber = 8;
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
    void CmdAskToFillEmptyCharInGameDeck()
    {
        if (CharactersDeckList_Server.Count < 4)
        {
            print("There is less than 4 Cards");
            return;
        }



        while (CharacterCardsDeckInGame_Server.Count < 4)
        {
            CharactersType CharTempDeck;
            int RandomIndex;
            bool RepeatedCard;

            // Check not to have a repetetive cell 
            do
            {
                RepeatedCard = false;

                RandomIndex = UnityEngine.Random.Range(0, CharactersDeckList_Server.Count);

                CharTempDeck = CharactersDeckList_Server[RandomIndex];

                if (CharacterCardsDeckInGame_Server.Contains(CharTempDeck))
                {
                    RepeatedCard = true;
                }

            } while (RepeatedCard);


            CharacterCardsDeckInGame_Server.Add(CharTempDeck);
            CharactersDeckList_Server.RemoveAt(RandomIndex);
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
    }
    #endregion


    [Command]
    void CmdAskToCheckSelectedCell(int cellNumber,CardType cardType, HouseCellsType houseCellsType, CharactersType charactersType, int PlayerID)
    {
        #region Check For Wrong Input and Cheat

        #region Cheat Detection
        // ServerTurn has next turn which is 1 or 2 and is opposit the player id who choose. sum of these two number is always 3 1+2 or 2+1
        if (ServerTurn+PlayerID != 3)
        {
            print("Cheat");
            RpcTellCharacterCells(CharCells_P1_Server);
            RpcTellHouseCells(HouseCells_P1_Server);
            return;
        }
        #endregion

        #region Wrong Input Detect And Reset Cells To Previous State And RPC It
        if (PlayerID == 1)
        {
            if (HouseCells_P1_Server[cellNumber] == HouseCellsType.BannedTile)
            {
                print("Error: Select baned house");
                RpcTellCharacterCells(CharCells_P1_Server);
                RpcTellHouseCells(HouseCells_P1_Server);
                return;
            }

            if (CharCells_P1_Server[cellNumber] != CharactersType.Empty)
            {
                print("Error: Select House tile with a character and wants to put card on it");
                RpcTellCharacterCells(CharCells_P1_Server);
                RpcTellHouseCells(HouseCells_P1_Server);
                return;
            }

            if (HouseCells_P1_Server[cellNumber] != HouseCellsType.EmptyTile && houseCellsType != HouseCellsType.EmptyTile)
            {
                print("Error: Select filled House tile and want to put another house card on it");
                RpcTellCharacterCells(CharCells_P1_Server);
                RpcTellHouseCells(HouseCells_P1_Server);
                return;
            }

            if (HouseCells_P1_Server[cellNumber] == HouseCellsType.EmptyTile && charactersType != CharactersType.Empty)
            {
                print("Error: Select Empty House tile and want to put character card on it");
                RpcTellCharacterCells(CharCells_P1_Server);
                RpcTellHouseCells(HouseCells_P1_Server);
                return;
            }
        }
        else if (PlayerID == 2)
        {
            if (HouseCells_P2_Server[cellNumber] == HouseCellsType.BannedTile)
            {
                print("Error: Select baned house");
                RpcTellCharacterCells(CharCells_P2_Server);
                RpcTellHouseCells(HouseCells_P2_Server);
                return;
            }

            if (CharCells_P2_Server[cellNumber] != CharactersType.Empty)
            {
                print("Error: Select House tile with a character and wants to put card on it");
                RpcTellCharacterCells(CharCells_P2_Server);
                RpcTellHouseCells(HouseCells_P2_Server);
                return;
            }

            if (HouseCells_P2_Server[cellNumber] != HouseCellsType.EmptyTile && houseCellsType != HouseCellsType.EmptyTile)
            {
                print("Error: Select filled House tile and want to put another house card on it");
                RpcTellCharacterCells(CharCells_P2_Server);
                RpcTellHouseCells(HouseCells_P2_Server);
                return;
            }

            if (HouseCells_P2_Server[cellNumber] == HouseCellsType.EmptyTile && charactersType != CharactersType.Empty)
            {
                print("Error: Select Empty House tile and want to put character card on it");
                RpcTellCharacterCells(CharCells_P2_Server);
                RpcTellHouseCells(HouseCells_P2_Server);
                return;
            }
        }
        #endregion

        #endregion

        // ToDo: Check if this action can be done based on game logic
        // ...
        // ...
        // In This State We Assume That The Action Is valid :

        if(PlayerID == 1)
        {
            if(charactersType != CharactersType.Empty && houseCellsType == HouseCellsType.EmptyTile)
            {
                CharCells_P1_Server[cellNumber] = charactersType;
                CharacterCardsDeckInGame_Server.Remove(charactersType);
                CharacterCardsDeckInGame_Server.Add(CharactersType.Empty);


                RpcTellCharacterInGameDeck(CharacterCardsDeckInGame_Server.ToArray());
                RpcTellCharacterCells(CharCells_P1_Server);
            }
            else if(houseCellsType != HouseCellsType.EmptyTile && charactersType == CharactersType.Empty)
            {
                HouseCells_P1_Server[cellNumber] = houseCellsType;
                HouseCardsDeckInGame_Server.Remove(houseCellsType);
                HouseCardsDeckInGame_Server.Add(HouseCellsType.EmptyTile);

                RpcTellHouseInGameDeck(HouseCardsDeckInGame_Server.ToArray());
                RpcTellHouseCells(HouseCells_P1_Server);
                CmdAskToFillEmptyHouseInGameDeck(false);

            }
            else
            {
                print("Empty Card Selected");
                return;
            }
            RpcTellTurn(ServerTurn);
        }
        else if (PlayerID == 2)
        {
            if (charactersType != CharactersType.Empty && houseCellsType == HouseCellsType.EmptyTile)
            {
                CharCells_P2_Server[cellNumber] = charactersType;
                CharacterCardsDeckInGame_Server.Remove(charactersType);
                CharacterCardsDeckInGame_Server.Add(CharactersType.Empty);


                RpcTellCharacterInGameDeck(CharacterCardsDeckInGame_Server.ToArray());
                RpcTellCharacterCells(CharCells_P2_Server);
            }
            else if (houseCellsType != HouseCellsType.EmptyTile && charactersType == CharactersType.Empty)
            {
                HouseCells_P2_Server[cellNumber] = houseCellsType;
                HouseCardsDeckInGame_Server.Remove(houseCellsType);
                HouseCardsDeckInGame_Server.Add(HouseCellsType.EmptyTile);

                RpcTellHouseInGameDeck(HouseCardsDeckInGame_Server.ToArray());
                RpcTellHouseCells(HouseCells_P2_Server);
            }
            else
            {
                print("Empty Card Selected");
                return;
            }
            RpcTellTurn(ServerTurn);
            CmdAskToFillEmptyCharInGameDeck();
            CmdAskToFillEmptyHouseInGameDeck(false);


        }

        // Here..........

        /*

        if (PlayerID == 1)
        {
            print("Player 1 Select a " + cardType + " Which is a " + houseCellsType + " Or a " + charactersType + " To Place in Slot: " + cellNumber
                + "Right now the house slot is : " + HouseCells_P1_Server[cellNumber] + " and the character slot is: " + CharCells_P1_Server[cellNumber]);
        }
        else if(PlayerID == 2)
        {
            print("Player 2 Select a " + cardType + " Which is a " + houseCellsType + " Or a " + charactersType + " To Place in Slot: " + cellNumber
                +"Right now the house slot is : "+ HouseCells_P2_Server[cellNumber] +" and the character slot is: "+CharCells_P2_Server[cellNumber] );

        }

        */
    }

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

    #region RpcTellHouseCells(HouseCellsType[] cellhouseArray)
    /// <summary>
    /// Tell Clients about House Cells Array
    /// </summary>
    /// <param name="cellhouseArray"></param>
    [ClientRpc]
    public void RpcTellHouseCells(HouseCellsType[] cellhouseArray)
    {
        MyHouseCells = cellhouseArray;

        IsReadyForUpdateCellsOnScreen = true;      
    }
    #endregion

    #region RpcTellCharacterCells(CharactersType[] charactersCellType)
    /// <summary>
    /// Tell Clients about Character Cells Array
    /// </summary>
    /// <param name="charactersCellType"></param>
    [ClientRpc]
    public void RpcTellCharacterCells(CharactersType[] charactersCellType)
    {
        MyCharCells = charactersCellType;

        IsReadyForUpdateCellsOnScreen = true;
    }
    #endregion



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


    #endregion





    // To-Do:
    // 1- Creat a custom HUD network manager
    // 2- When Stop or disconnection button on HUD network manager pressed, gameManagerscript.Initialazation(true) should be called
    // 3- Place back button in waiting room and room is full panel


    // ==> To Continiue : the player who has the turn can select card and put on map
    // Working On "CmdAskToCheckSelectedCell" which is called from CommandToCheckSelectedCell to check the cell
}
