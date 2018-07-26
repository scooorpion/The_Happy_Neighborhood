using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class PlayerConnection : NetworkBehaviour     
{
    #region Fields

    static int ActiveConnections = 0;
    public GameObject PlayerUnitPrefab;
    private GameManager gameManagerscript;

    #endregion

    void Start()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        gameManagerscript = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        CmdAskToTellActiveConnection();

        CmdSpwanMyPlayerUnit();
    }


    void Update()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        ShowAnimationBasedOnActiveConnectionNumbers();

    }

    #region ShowAnimationBasedOnActiveConnectionNumbers()
    /// <summary>
    /// Show Animation Based On Active Players(Waiting,Game Loading,Room is full)
    /// </summary>
    void ShowAnimationBasedOnActiveConnectionNumbers()
    {
        switch (ActiveConnections)
        {
            case 0:
                break;
            case 1:
                gameManagerscript.ShowWaitingAnimation();
                break;
            case 2:
                gameManagerscript.ShowGameLoading();
                break;
            default:
                gameManagerscript.ShowRoomIsFull();
                break;
        }
    }
    #endregion


    #region COMMANDS

    #region CmdSpwanMyPlayerUnit()
    /// <summary>
    /// Command Server to spawn PlayerUnit Prefab
    /// </summary>
    [Command]
    void CmdSpwanMyPlayerUnit()
    {
        GameObject go = Instantiate<GameObject>(PlayerUnitPrefab);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
    #endregion

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

}
