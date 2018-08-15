using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkButtonManager : MonoBehaviour {

    NetworkManager networkManager;
    MyNetworkDiscovery networkDiscovery;
    //public GameObject MyConnection;

    private void Start()
    {
        networkManager = GameObject.FindObjectOfType<NetworkManager>();
        networkDiscovery = GameObject.FindObjectOfType<MyNetworkDiscovery>();
    }

    public void CreateRoom()
    {
        //NetworkServer.Reset();
        //Disconnection();
        networkManager.StopHost();
        
        networkManager.StartHost();
        networkDiscovery.StartBroadcast();
    }


    public void FindRoom()
    {
        //NetworkServer.Reset();

        networkDiscovery.StartListening();
        networkManager.StartClient();
    }



    public void Exit()
    {
        print("Exiiit");
        GameObject myConnection;

        if (myConnection = GameObject.FindGameObjectWithTag("MyConnection"))
        {
            PlayerConnection myConnectionScript = myConnection.GetComponent<PlayerConnection>();

            myConnectionScript.CmdOnePlayerLeft(myConnectionScript.MyTurnID);
        }

        //GameObject myConnection = GameObject.FindGameObjectWithTag("MyConnection");

        //PlayerConnection myConnectionScript = myConnection.GetComponent<PlayerConnection>();

        //myConnectionScript.CmdOnePlayerLeft(myConnectionScript.MyTurnID);


    }

    public void Disconnection()
    {
        
        networkDiscovery.StopBroadcast();
        networkManager.StopHost();
        networkManager.StopClient();
    }

}
