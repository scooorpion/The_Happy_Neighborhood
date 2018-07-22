using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour
{
    static int ActiveConnections;

    public int PlayersToStartGame = 2;
    public GameObject PlayerUnit;
    bool FlagForCheckingConnection = true;
    GameManager gameManagerScript;

    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        gameManagerScript = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        CmdSpwanMyPlayerUnit();
        CmdCalculateActiveConnections();

    }


    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        print("Client ActiveConnections: "+ ActiveConnections);
        

        if (FlagForCheckingConnection)
        {

            if (PlayersToStartGame > ActiveConnections)
            {
                // Wait For Another Player
                gameManagerScript.ShowWaitingRoom();
            }
            else if (PlayersToStartGame == ActiveConnections)
            {
                // start the game
                gameManagerScript.StartTheGame();
                FlagForCheckingConnection = false;

            }
            else
            {
                // Sorry The Room Is Full
                gameManagerScript.ShowRoomIsFull();

            }

        }


    }



    #region COMMANDS

    [Command]
    void CmdSpwanMyPlayerUnit()
    {
        GameObject go = Instantiate<GameObject>(PlayerUnit);

        go.GetComponent<PlayerUnit>().UserName = PlayerPrefs.GetString(MenuManager.UserNamePlayerPrefs);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);

    }

    [Command]
    void CmdCalculateActiveConnections()
    {
        ActiveConnections = NetworkServer.connections.Count;
        RpcSendActiveConnections(ActiveConnections);
    }


    #endregion


    #region RPCs

    [ClientRpc]
    void RpcSendActiveConnections(int Connection)
    {
        ActiveConnections = Connection;

        print("RPC ActiveConnections: " + ActiveConnections);
    }
    #endregion

}
