using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkButtonManager : MonoBehaviour {

    NetworkManager manager;
    MyNetworkDiscovery networkDiscovery;
    //public GameObject MyConnection;

    private void Start()
    {
        manager = GameObject.FindObjectOfType<NetworkManager>();
        networkDiscovery = GameObject.FindObjectOfType<MyNetworkDiscovery>();
    }

    public void CreateRoom()
    {
        manager.StartHost();
        networkDiscovery.StartBroadcast();
    }

    public void FindRoom()
    {
        networkDiscovery.StartListening();

    }

    public void Exit()
    {
        //GameObject.FindObjectOfType<GameManager>().Initialazation(true);
        //GameObject.FindGameObjectWithTag("MyConnection").GetComponent<PlayerConnection>().CmdResetServerData();

        SceneManager.LoadScene(0);
        networkDiscovery.StopBroadcast();
        manager.StopHost();
        manager.StopClient();

        // Here ............
    }

}
