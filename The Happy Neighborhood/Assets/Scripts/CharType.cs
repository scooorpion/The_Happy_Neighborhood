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

    public static int CalculateCharacterScore(CharactersType Character)
    {
        int tempScore = 0;

        switch (Character)
        {
            case CharactersType.NoramlGuy:
                tempScore = 10;
                break;

            case CharactersType.RedGuy:
                tempScore = 15;
                break;

            case CharactersType.RedNoBlueGuy:
                tempScore = 25;
                break;

            case CharactersType.BlueGuy:
                tempScore = 15;
                break;

            case CharactersType.BlueNoYellow:
                tempScore = 25;
                break;

            case CharactersType.PurpuleGuy:
                tempScore = 15;
                break;

            case CharactersType.PurpleNoRedGuy:
                tempScore = 25;
                break;

            case CharactersType.OldGuy:
                tempScore = 20;
                break;

            case CharactersType.PenthouseGuy:
                tempScore = 50;
                break;

            case CharactersType.TwoHouseGuy:
                tempScore = 20;
                break;

            case CharactersType.ThreeHouseLGuy:
                tempScore = 30;
                break;

            case CharactersType.FourHouseGuy:
                tempScore = 40;
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


    public void ShowCharacterInfo()
    {
        switch (charactersType)
        {
            case CharactersType.NoramlGuy:
                ValidHouses = "All of the tiles";
                CharacterNames = "Noraml Guy";
                characterScore = 10;
                break;

            case CharactersType.RedGuy:
                ValidHouses = "Red tiles";
                CharacterNames = "Red Guy";
                characterScore = 15;
                break;

            case CharactersType.RedNoBlueGuy:
                ValidHouses = "Red tiles, But not adjacent to blue tiles";
                CharacterNames = "Red No Blue Guy";
                characterScore = 25;
                break;

            case CharactersType.BlueGuy:
                ValidHouses = "Blue tiles";
                CharacterNames = "Blue Guy";
                characterScore = 15;
                break;

            case CharactersType.BlueNoYellow:
                ValidHouses = "Blue tiles, But not adjacent to yellow tiles";
                CharacterNames = "Blue No Yellow";
                characterScore = 25;
                break;

            case CharactersType.PurpuleGuy:
                ValidHouses = "Purpule tiles";
                CharacterNames = "Purpule Guy";
                characterScore = 15;
                break;

            case CharactersType.PurpleNoRedGuy:
                ValidHouses = "Purple tiles, But not adjacent to red tiles";
                CharacterNames = "Purple No Red Guy";
                characterScore = 25;
                break;

            case CharactersType.OldGuy:
                ValidHouses = "First floor tiles, no tile on top";
                CharacterNames = "Old Guy";
                characterScore = 20;
                break;

            case CharactersType.PenthouseGuy:
                ValidHouses = "Roof tiles";
                CharacterNames = "Penthouse Guy";
                characterScore = 50;
                break;

            case CharactersType.TwoHouseGuy:
                ValidHouses = "Two tiles, adjacent & the same color";
                CharacterNames = "Two House Guy";
                characterScore = 20;
                break;

            case CharactersType.ThreeHouseLGuy:
                ValidHouses = "Three tiles, adjacent & the same color";
                CharacterNames = "Three House L Guy";
                characterScore = 30;
                break;

            case CharactersType.FourHouseGuy:
                ValidHouses = "Four tiles, adjacent & the same color";
                CharacterNames = "Four House Guy";
                characterScore = 40;
                break;


            case CharactersType.Wizard:
                ValidHouses = "Damaged tiles";
                CharacterNames = "Wizard";
                characterScore = 0;
                break;

            default:
                ValidHouses = "";
                CharacterNames = "";
                characterScore = 0;
                break;
        }

    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowCharacterInfo();
        deckManager.ShowCharacterInfo(CharacterNames, ValidHouses, characterScore);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        deckManager.HideCharacterInfo();
    }
}
