using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkButtonManager : MonoBehaviour {

    NetworkManager networkManager;
    MyNetworkDiscovery networkDiscovery;
    HUDNetworkManagerCostumized hudManager;

    private void Start()
    {
        networkManager = GameObject.FindObjectOfType<NetworkManager>();
        networkDiscovery = GameObject.FindObjectOfType<MyNetworkDiscovery>();
        hudManager = FindObjectOfType<HUDNetworkManagerCostumized>();
    }

    public void CreateRoom()
    {
        networkManager.StopHost();

        networkManager.StartHost();
        networkDiscovery.StartBroadcast();
    }


    public void FindRoom()
    {
        networkDiscovery.StartListening();
    }



    public void Exit()
    {
        PlayerConnection myConnectionScript = GameObject.FindGameObjectWithTag("MyConnection").GetComponent<PlayerConnection>();

        myConnectionScript.CmdOnePlayerLeft(myConnectionScript.MyTurnID);
    }

    public void Disconnection()
    {
        
        //networkDiscovery.StopBroadcast();
        networkManager.StopHost();
        networkManager.StopClient();
        Network.Disconnect();
    }

}
