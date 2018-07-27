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

    // Server-Side And Client-Side Arrays
    public int[] BanedCells0_P1;   // because 2D array is not supported on network I use 2 seperated array
    public int[] BanedCells1_P1;

    public int[] BanedCells0_P2;
    public int[] BanedCells1_P2;   


    #endregion




    void Start()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        gameManagerscript = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        CmdAskToSetPlayerTurn();

        CmdAskToTellActiveConnection();

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

        ShowAnimationBasedOnActiveConnectionNumbers();
        gameManagerscript.SetUserNames(UserName, EnemyName);

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
                    CmdAskToCalculateRandomBannedLocation(ServerTurn);                    
                    gameManagerscript.ShowGameLoading();
                    gameManagerscript.SetDiactiveUIBeginingWaitingPanel();

                    
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

    #region CmdAskToCalculateRandomBannedLocation(int Turn)
    /// <summary>
    /// Command server to create an array of banned construction to fill the map
    /// </summary>
    [Command]
    void CmdAskToCalculateRandomBannedLocation(int Turn)
    {
        int[] BanedCells0_temp = new int[BanedConstructionCellNumber];
        int[] BanedCells1_temp = new int[BanedConstructionCellNumber];

        for (int i = 0; i < BanedConstructionCellNumber; i++)
        {
            bool RepeatedCell;
            int randomCellsFirstDemension;
            int randomCellsSecondtDemension;

            // Checking not to use repeated cell for NoConstructionCell
            do
            {
                RepeatedCell = false;
                randomCellsFirstDemension = UnityEngine.Random.Range(0, 7);
                randomCellsSecondtDemension = UnityEngine.Random.Range(0, 7);
                for (int j = 0; j < BanedConstructionCellNumber; j++)
                {
                    if (BanedCells0_temp[j] == randomCellsFirstDemension)
                    {
                        if (BanedCells1_temp[j] == randomCellsSecondtDemension)
                        {
                            RepeatedCell = true;
                        }
                    }
                }
            }
            while (RepeatedCell);

            BanedCells0_temp[i] = randomCellsFirstDemension;
            BanedCells1_temp[i] = randomCellsSecondtDemension;

        }

        if (Turn == 1)
        {
            BanedCells0_P1 = BanedCells0_temp;
            BanedCells1_P1 = BanedCells1_temp;
        }
        else if (Turn == 2)
        {
            BanedCells0_P2 = BanedCells0_temp;
            BanedCells1_P2 = BanedCells1_temp;
        }


        RpcTellBannedConstructionCells(BanedCells0_temp, BanedCells1_temp);
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

    #region RpcTellBannedConstructionCells(int[] BannedConsCells0, int[] BannedConsCells1)
    /// <summary>
    /// Tell Clients Banned Cells
    /// </summary>
    /// <param name="BannedConsCells0"></param>
    /// <param name="BannedConsCells1"></param>
    [ClientRpc]
    void RpcTellBannedConstructionCells(int[] BannedConsCells0, int[] BannedConsCells1)
    {
        if(MyTurnID == 1)
        {
            BanedCells0_P1 = BannedConsCells0;
            BanedCells1_P1 = BannedConsCells1;

        }
        else if(MyTurnID == 2)
        {
            BanedCells0_P2 = BannedConsCells0;
            BanedCells1_P2 = BannedConsCells1;

        }

    }
    #endregion

    #endregion





    // To-Do:
    // 1- Creat a custom HUD network manager
    // 2- When Stop or disconnection button on HUD network manager pressed, gameManagerscript.Initialazation(true) should be called
    // 3- Place back button in waiting room and room is full panel


    // ==> To Continiue : Banned Cell is calculated and stored on the server, now player should create his map based on this fields
}
