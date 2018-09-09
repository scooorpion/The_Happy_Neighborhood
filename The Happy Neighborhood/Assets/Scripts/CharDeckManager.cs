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

    public static float CharInsertionTime(CharactersType CharTile)
    {
        float tempCharTime = 0;

        switch (CharTile)
        {
            case CharactersType.NoramlGuy:
                tempCharTime = -1;
                break;
            case CharactersType.RedGuy:
                tempCharTime = -1;
                break;
            case CharactersType.RedNoBlueGuy:
                tempCharTime = 30;
                break;
            case CharactersType.BlueGuy:
                tempCharTime = -1;
                break;
            case CharactersType.BlueNoYellow:
                tempCharTime = 30;
                break;
            case CharactersType.PurpuleGuy:
                tempCharTime = -1;
                break;
            case CharactersType.PurpleNoRedGuy:
                tempCharTime = 30;
                break;
            case CharactersType.OldGuy:
                tempCharTime = 20;
                break;
            case CharactersType.PenthouseGuy:
                tempCharTime = 200;
                break;
            case CharactersType.TwoHouseGuy:
                tempCharTime = 60;
                break;
            case CharactersType.ThreeHouseLGuy:
                tempCharTime = 90;
                break;
            case CharactersType.FourHouseGuy:
                tempCharTime = 180;
                break;
            default:
                break;
        }

        return tempCharTime;
    }

}
