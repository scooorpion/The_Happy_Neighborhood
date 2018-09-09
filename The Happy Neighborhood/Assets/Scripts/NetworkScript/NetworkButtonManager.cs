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
    SoundManager soundManager;
    public Text serverText;

    public GameObject CreateRoomBtn;
    public GameObject FindRoomBtn;

    public GameObject DifficultyLevelDropDownPanel;
    public Dropdown DifficultyLevel;

    public Animator UIAnimator;



    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        networkManager = GameObject.FindObjectOfType<NetworkManager>();
        networkDiscovery = GameObject.FindObjectOfType<MyNetworkDiscovery>();
        hudManager = FindObjectOfType<HUDNetworkManagerCostumized>();
        CreateRoomBtn.SetActive(true);
        FindRoomBtn.SetActive(true);
        DifficultyLevelDropDownPanel.SetActive(true);

    }

    public void ValueChange()
    {
        soundManager.SFX_MenuButtonPlay();
    }

    public void CreateRoom()
    {
        soundManager.SFX_MenuButtonPlay();
        soundManager.FindingLobbyTrackPlay();

        UIAnimator.SetBool("Find_Waiting", false);
        UIAnimator.SetBool("Create_waiting", true);

        CreateRoomBtn.SetActive(false);
        DifficultyLevelDropDownPanel.SetActive(false);
        FindRoomBtn.SetActive(true);

        switch (DifficultyLevel.value)
        {
            case 0:
                CharacterLife.difficultyLevel = global::DifficultyLevel.NormalWitch;
                break;

            case 1:
                CharacterLife.difficultyLevel = global::DifficultyLevel.EvilWitch;
                break;

            case 2:
                CharacterLife.difficultyLevel = global::DifficultyLevel.CrazyWitch;
                break;
            default:
                break;
        }

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
        networkManager.StopHost();

        soundManager.SFX_MenuButtonPlay();
        soundManager.FindingLobbyTrackPlay();

        UIAnimator.SetBool("Create_waiting", false);
        UIAnimator.SetBool("Find_Waiting", true);

        DifficultyLevelDropDownPanel.SetActive(true);
        CreateRoomBtn.SetActive(true);
        FindRoomBtn.SetActive(false);


        networkDiscovery.StopBroadcast();


        networkDiscovery.StartListening();

    }

    public void ExitInGame()
    {
        soundManager.SFX_MenuButtonPlay();
        soundManager.FindingLobbyTrackStop();

        networkManager.StopHost();
        SceneManager.LoadScene(1);

    }


    public void ExitInFinishPanel()
    {
        soundManager.SFX_MenuButtonPlay();
        if (GameObject.FindGameObjectWithTag("MyConnection"))
        {
            GameObject.FindGameObjectWithTag("MyConnection").GetComponent<PlayerConnection>().CmdAskToSetFinishPanelExitFlagTrue();
        }
        StartCoroutine(ExitDelay(1f));
    }

    IEnumerator ExitDelay(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        
        // It Work For Just The first player who exit and stop host
        if (PlayerConnection.HowManyClientExitAtTheEnd == 1)
        {
            networkManager.StopHost();
        }
        SceneManager.LoadScene(1);

    }




}
