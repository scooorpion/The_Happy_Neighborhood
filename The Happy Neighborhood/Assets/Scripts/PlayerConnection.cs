using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class PlayerConnection : NetworkBehaviour     
{
    public enum CharType { Guy, OldMan, Boy, Girl };

    static int ActiveConnections;

    public GameObject PlayerUnitPrefab;

    [SyncVar]
    string UserName;

    public CharType[] CardDecks = new CharType[7];

    GameObject[] playerConnsArray;




    void Start()
    {
        if (!isLocalPlayer)
        {
            print("This is not my script");
            return;
        }

        // Ask Server To Create My Unit and Tell everyone about it
        CmdSpwanMyPlayerUnit();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerConnsArray = GameObject.FindGameObjectsWithTag("PlayerConnection");

            print("Ther is " + playerConnsArray.Length + " Units");


            for (int i = 0; i < playerConnsArray.Length; i++)
            {
                if(isLocalPlayer)
                {
                    print("My Unit Is: " + UserName);
                }
                else
                {
                    print("My Rival's Unit Is: " + UserName);
                }
            }
        }


        if (!isLocalPlayer)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            string n = "Mohammad_" + UnityEngine.Random.Range(1, 10);
            CmdAskToChangePlyerUserName(n);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            for (int i = 0; i < CardDecks.Length; i++)
            {
                CardDecks[i] = (CharType)UnityEngine.Random.Range(0, 4);
            }

            CmdAskToChangeDeckArray(CardDecks);
        }

    }




    #region COMMANDS

    [Command]
    void CmdSpwanMyPlayerUnit()
    {
        GameObject go = Instantiate<GameObject>(PlayerUnitPrefab);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }

    [Command]
    void CmdAskToChangePlyerUserName(string n)
    {
        UserName = n;
    }

    [Command]
    void CmdAskToChangeDeckArray(CharType[] charArray)
    {
        CardDecks = charArray;
        RpcChangeArray(CardDecks);
    }
    #endregion


    #region RPCs

    [ClientRpc]
    void RpcChangeArray(CharType[] chArray)
    {
        CardDecks = chArray;
    }
    #endregion

}
