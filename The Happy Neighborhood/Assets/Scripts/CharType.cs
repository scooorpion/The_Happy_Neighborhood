using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CharType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CharDeckManager deckManager;

    public CharactersType charactersType;

    public  int characterScore;
    public  string ValidHouses = "";
    public  string CharacterNames = "";


    private void Start()
    {
        deckManager = FindObjectOfType<CharDeckManager>();
        ShowCharacterInfo();
    }

    public int CalculateCharacterScore(CharactersType Character)
    {
        int tempScore = 0;

        switch (Character)
        {
            case CharactersType.Empty:
                tempScore = 0;
                break;
            case CharactersType.NoramlGuy:
                tempScore = 5;
                break;
            case CharactersType.DoubleGuys:
                tempScore = 10;
                break;
            case CharactersType.TripleGuys:
                tempScore = 15;
                break;
            case CharactersType.RedGuy:
                tempScore = 5;
                break;
            case CharactersType.RedNoBlueGuy:
                tempScore = 5;
                break;
            case CharactersType.BlueGuy:
                tempScore = 5;
                break;
            case CharactersType.BlueNoYellow:
                tempScore = 5;
                break;
            case CharactersType.PurpuleGuy:
                tempScore = 5;
                break;
            case CharactersType.PurpleNoRedGuy:
                tempScore = 5;
                break;
            case CharactersType.OldGuy:
                tempScore = 5;
                break;
            case CharactersType.PenthouseGuy:
                tempScore = 15;
                break;
            case CharactersType.TwoHouseGuy:
                tempScore = 10;
                break;
            case CharactersType.ThreeHouseLGuy:
                tempScore = 15;
                break;
            case CharactersType.FourHouseGuy:
                tempScore = 20;
                break;
            case CharactersType.Ghost:
                tempScore = -5;
                break;
            case CharactersType.Wizard:
                tempScore = 0;
                break;
            default:
                tempScore = 0;
                break;
        }


        return tempScore;
    }

    public string CalculateValidHouseFoCharacter(CharactersType Character)
    {
        string tempStringText = "";

        switch (Character)
        {
            case CharactersType.NoramlGuy:
                tempStringText = "Each house";
                break;

            case CharactersType.DoubleGuys:
                tempStringText = "Two houses";
                break;

            case CharactersType.TripleGuys:
                tempStringText = "Three houses";
                break;

            case CharactersType.RedGuy:
                tempStringText = "Red houses";
                break;

            case CharactersType.RedNoBlueGuy:
                tempStringText = "Red houses, Not in neighborhood of blue houses";
                break;

            case CharactersType.BlueGuy:
                tempStringText = "Blue houses";
                break;

            case CharactersType.BlueNoYellow:
                tempStringText = "Blue houses, Not in neighborhood of yellow houses";
                break;

            case CharactersType.PurpuleGuy:
                tempStringText = "Purpule houses";
                break;

            case CharactersType.PurpleNoRedGuy:
                tempStringText = "Purpule houses, Not in neighborhood of red houses";
                break;

            case CharactersType.OldGuy:
                tempStringText = "Each old house";
                break;

            case CharactersType.PenthouseGuy:
                tempStringText = "Penthouse";
                break;

            case CharactersType.TwoHouseGuy:
                tempStringText = "Two houses, near each other and the same color";
                break;

            case CharactersType.ThreeHouseLGuy:
                tempStringText = "Three houses, near each other and the same color";
                break;

            case CharactersType.FourHouseGuy:
                tempStringText = "Four houses, near each other and the same color";
                break;


            case CharactersType.Wizard:
                tempStringText = "Each old house";
                break;

            default:
                tempStringText = "";
                break;
        }

        return tempStringText;
    }

    public void ShowCharacterInfo()
    {
        switch (charactersType)
        {
            case CharactersType.NoramlGuy:
                ValidHouses = "Each house";
                CharacterNames = "Noraml Guy";
                characterScore = 5;
                break;

            case CharactersType.DoubleGuys:
                ValidHouses = "Two houses";
                CharacterNames = "Double Guys";
                characterScore = 5;
                break;

            case CharactersType.TripleGuys:
                ValidHouses = "Three houses";
                CharacterNames = "Triple Guys";
                characterScore = 5;
                break;

            case CharactersType.RedGuy:
                ValidHouses = "Red houses";
                CharacterNames = "Red Guy";
                characterScore = 5;
                break;

            case CharactersType.RedNoBlueGuy:
                ValidHouses = "Red houses, Not in neighborhood of blue houses";
                CharacterNames = "Red No Blue Guy";
                characterScore = 5;
                break;

            case CharactersType.BlueGuy:
                ValidHouses = "Blue houses";
                CharacterNames = "Blue Guy";
                characterScore = 5;
                break;

            case CharactersType.BlueNoYellow:
                ValidHouses = "Blue houses, Not in neighborhood of yellow houses";
                CharacterNames = "Blue No Yellow";
                characterScore = 5;
                break;

            case CharactersType.PurpuleGuy:
                ValidHouses = "Purpule houses";
                CharacterNames = "Purpule Guy";
                characterScore = 5;
                break;

            case CharactersType.PurpleNoRedGuy:
                ValidHouses = "Purpule houses, Not in neighborhood of red houses";
                CharacterNames = "Purple No Red Guy";
                characterScore = 5;
                break;

            case CharactersType.OldGuy:
                ValidHouses = "Each old house";
                CharacterNames = "Old Guy";
                characterScore = 5;
                break;

            case CharactersType.PenthouseGuy:
                ValidHouses = "Penthouse";
                CharacterNames = "Penthouse Guy";
                characterScore = 5;
                break;

            case CharactersType.TwoHouseGuy:
                ValidHouses = "Two houses, near each other and the same color";
                CharacterNames = "Two House Guy";
                characterScore = 5;
                break;

            case CharactersType.ThreeHouseLGuy:
                ValidHouses = "Three houses, near each other and the same color";
                CharacterNames = "Three House L Guy";
                characterScore = 5;
                break;

            case CharactersType.FourHouseGuy:
                ValidHouses = "Four houses, near each other and the same color";
                CharacterNames = "Four House Guy";
                characterScore = 5;
                break;


            case CharactersType.Wizard:
                ValidHouses = "Each old house";
                CharacterNames = "Wizard";
                characterScore = 5;
                break;

            default:
                ValidHouses = "";
                CharacterNames = "";
                characterScore = 5;
                break;
        }

    }



    public void OnPointerEnter(PointerEventData eventData)
    {
       if(CharacterNames == "")
        {
            ShowCharacterInfo();
        }
        deckManager.ShowCharacterInfo(CharacterNames, ValidHouses, characterScore);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        deckManager.HideCharacterInfo();
    }
}
