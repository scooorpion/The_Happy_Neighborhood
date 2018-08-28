using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class CharDeckManager : MonoBehaviour
{
    public GameObject EmptyCardPrefab;
    public GameObject[] ParentCharacterSlots = new GameObject[4];  // 4 ui panel that character tiles cards go inside them
    public static GameObject[] CharacterCardsDeckPickable = new GameObject[4];   // an array of characterTiles to use in game


    public GameObject CharacterInfo;
    public Text CharacterNameInfo;
    public Text CharacterscoreInfo;
    public Text CharacterValidHouseInfo;
    public Image CharacterHelpImage;

    void Start()
    {
        FirstBuildArray();
        CharacterInfo.SetActive(false);
    }

    void FirstBuildArray()
    {
        for (int i = 0; i < 4; i++)
        {
            CharacterCardsDeckPickable[i] = Instantiate(EmptyCardPrefab);
            CharacterCardsDeckPickable[i].GetComponent<Transform>().SetParent(ParentCharacterSlots[i].transform, false);
        }
    }

    public void ShowCharacterInfo(string CharName, string CharValidHouse, int CharScore, Sprite HelpSprite)
    {
        //CharacterNameInfo.text = CharName;
        //CharacterscoreInfo.text = CharScore.ToString();
        //CharacterValidHouseInfo.text = CharValidHouse;
        CharacterHelpImage.sprite = HelpSprite;
        CharacterInfo.SetActive(true);
    }

    public void HideCharacterInfo()
    {
        //CharacterNameInfo.text = "";
        //CharacterscoreInfo.text = "";
        //CharacterValidHouseInfo.text = "";
        CharacterInfo.SetActive(false);
    }


}
