using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



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

    private bool IsReadyForUpdateScreen = false;

    public HouseCellsType[] MyHouseCells = new HouseCellsType[49];
    public CharactersType[] MyCharCells = new CharactersType[49];


    // Server-Side Arrays

    static HouseCellsType[] HouseCells_P1_Server = new HouseCellsType[49];
    static HouseCellsType[] HouseCells_P2_Server = new HouseCellsType[49];

    static CharactersType[] CharCells_P1_Server = new CharactersType[49];
    static CharactersType[] CharCells_P2_Server = new CharactersType[49];



    #endregion




    void Start()
    {

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


    }


    void Update()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        // Wait untill MyTurnId is set by RPC
        if(MyTurnID == 0)
        {
            return;
        }

        gameManagerscript.SetUserNames(UserName, EnemyName);

        ShowAnimationBasedOnActiveConnectionNumbers();

        if(IsReadyForUpdateScreen)
        {

            // Simiulating my cell array
            gameManagerscript.UpdateHouseTileMap(MyHouseCells);


            // Simiulating my enemy cell array
            PlayerConnection[] PlayerConnections = GameObject.FindObjectsOfType<PlayerConnection>();

            for (int i = 0; i < PlayerConnections.Length; i++)
            {

                if (!PlayerConnections[i].CompareTag("MyConnection"))
                {
                    gameManagerscript.UpdateHouseTileMap(PlayerConnections[i].MyHouseCells, false);
                    return;
                }
            }

            IsReadyForUpdateScreen = false;

        }
    }


    #region OnStartLocalPlayer()
    public override void OnStartLocalPlayer()
    {
        gameObject.tag = "MyConnection";
    }
    #endregion

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
                case 1:
                    // Wait for other player to connecte
                    gameManagerscript.ShowWaitingAnimation();
                    break;
                case 2:
                    // Start the game
                    setEnemyName();
                    gameManagerscript.ShowGameLoading();
                    gameManagerscript.SetDiactiveUIBeginingWaitingPanel();
                    CmdAskToCreateHouseTilesArray(MyTurnID);
                    IsGameStarted = true;
                    break;
                default:
                    // Display an error that server is full and a back button 
                    gameManagerscript.ShowRoomIsFull();
                    break;
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
        PlayerConnection[] PlayerConnections = GameObject.FindObjectsOfType<PlayerConnection>();

        for (int i = 0; i < PlayerConnections.Length; i++)
        {
            
            if(!PlayerConnections[i].CompareTag("MyConnection"))
            {
                EnemyName = PlayerConnections[i].UserName;
                return;
            }
        }
    }

    #endregion


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

        print("Server ==> PlayerTurn: " + ServerTurn);
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
        print("RPC, My ID: " + MyTurnID);
        for (int i = 0; i < 49; i++)
        {
            print("Temp Cell: "+cellhouseArray[i]);
        }

        MyHouseCells = cellhouseArray;

        IsReadyForUpdateScreen = true;      // New Variable............

        print("House Cell is created and stored");
    }
    #endregion



    #endregion





    // To-Do:
    // 1- Creat a custom HUD network manager
    // 2- When Stop or disconnection button on HUD network manager pressed, gameManagerscript.Initialazation(true) should be called
    // 3- Place back button in waiting room and room is full panel


    // ==> To Continiue : simulate cell arrays on screen
}
