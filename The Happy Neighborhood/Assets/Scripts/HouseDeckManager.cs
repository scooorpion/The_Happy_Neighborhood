using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum CardType { NoSelection, CharacterCard, HouseCard, GhostCard }

public class HouseDeckManager : MonoBehaviour
{

    public GameObject EmptyCardPrefab;
    public GameObject[] ParentHouseSlots = new GameObject[4];  // 4 ui panel that house tiles cards go inside them
    public static GameObject[] HousesCardsDeckPickable = new GameObject[4];   // an array of houseTiles to use in game

    public GameObject HouseInfo;
    public Image HouseHelpImage;



    void Start()
    {
        FirstBuildArray();
    }

    void FirstBuildArray()
    {
        for (int i = 0; i < 4; i++)
        {
            HousesCardsDeckPickable[i] = Instantiate(EmptyCardPrefab);
            HousesCardsDeckPickable[i].GetComponent<Transform>().SetParent(ParentHouseSlots[i].transform, false);
        }
    }

    public void ShowHouseInfo(Sprite HelpSprite)
    {
        HouseHelpImage.sprite = HelpSprite;
        HouseInfo.SetActive(true);
    }

    public void HideHouseInfo()
    {
        //CharacterNameInfo.text = "";
        //CharacterscoreInfo.text = "";
        //CharacterValidHouseInfo.text = "";
        HouseInfo.SetActive(false);
    }


}

