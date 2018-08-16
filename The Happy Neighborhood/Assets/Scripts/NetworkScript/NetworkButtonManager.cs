using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkButtonManager : MonoBehaviour {

    NetworkManager networkManager;
    MyNetworkDiscovery networkDiscovery;
    HUDNetworkManagerCostumized hudManager;
    public Text serverText;


    private void Start()
    {
        networkManager = GameObject.FindObjectOfType<NetworkManager>();
        networkDiscovery = GameObject.FindObjectOfType<MyNetworkDiscovery>();
        hudManager = FindObjectOfType<HUDNetworkManagerCostumized>();
    }

    public void CreateRoom()
    {
        networkDiscovery.StopBroadcast();


        var newHost = networkManager.StartHost();

        if (newHost == null)
        {
            StopCoroutine(ShowError());
            StartCoroutine(ShowError());
        }

        //networkManager.StartHost();
        networkDiscovery.StartBroadcast();
    }


    IEnumerator ShowError()
    {
        serverText.text = ("Error: Port already in use. Try Find a room");
        yield return new WaitForSeconds(2);
        serverText.text = "";
    }

    public void FindRoom()
    {
        networkDiscovery.StopBroadcast();


        networkDiscovery.StartListening();
    }



    public void Exit()
    {
        networkManager.StopHost();
        SceneManager.LoadScene(0);
    }


}
