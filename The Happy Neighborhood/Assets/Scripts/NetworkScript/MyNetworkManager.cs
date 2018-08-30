using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        //StopHost();

        if ( !PlayerConnection.IsExitAfterFinishPanel )
        {
            FindObjectOfType<SoundManager>().SFX_WrongACtionPlay();
            StartCoroutine(FindObjectOfType<GameManager>().OtherConnectionLost(2));
            print("Server is disconnected");

        }

        StopHost();

    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        
        //StopClient();
       // GameObject.FindGameObjectWithTag("MyConnection").GetComponent<PlayerConnection>().ShowConnectionLostPanel = true;


        if (!PlayerConnection.IsExitAfterFinishPanel)
        {
            FindObjectOfType<SoundManager>().SFX_WrongACtionPlay();
            StartCoroutine(FindObjectOfType<GameManager>().OtherConnectionLost(2));
            print("Client is disconnected");

        }
        StopHost();

    }
}

