using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HUDNetworkManagerCostumized : MonoBehaviour
{
    [SerializeField]
    public NetworkManager manager;
    MyNetworkDiscovery networkDiscovery;

    private void Awake()
    {
        networkDiscovery = GetComponent<MyNetworkDiscovery>();
    }

    // ToDo: Create a button and user click on it, then join the game not automatically
    public void JoinRoom(string IP)
    {
        manager.networkAddress = IP;
        manager.StartClient();
    }



}
