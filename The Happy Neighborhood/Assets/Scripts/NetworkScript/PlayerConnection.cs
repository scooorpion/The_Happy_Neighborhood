using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class PlayerConnection : NetworkBehaviour     
{
    #region Fields

    [SyncVar]
    public string UserName;

    public string EnemyName;

    private PlayerConnection enemyPlayerConnection;
    static int ActiveConnections = 0;
    private GameManager gameManagerscript;
    private bool IsGameStarted = false;


    #endregion




    void Start()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        gameManagerscript = FindObjectOfType<GameManager>().GetComponent<GameManager>();

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
        print("Server: Change the name syncVari");
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

    #endregion





    // To-Do:
    // 1- Creat a custom HUD network manager
    // 2- When Stop or disconnection button on HUD network manager pressed, gameManagerscript.Initialazation(true) should be called
    // 3- Place back button in waiting room and room is full panel

}
