using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CardSelecting : MonoBehaviour
{
    PlayerConnection myConnection;
    SoundManager soundManager;

    private void Start()
    {
        myConnection = GameObject.FindGameObjectWithTag("MyConnection").GetComponent<PlayerConnection>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void SelectCard()
    {
        CharType charType;
        HouseType houseType;

        soundManager.SFX_SelectingCardPlay();
        if (charType = GetComponent<CharType>())
        {
            myConnection.CharacterdCardSelected = charType.charactersType;
            myConnection.HouseCardSelected = HouseCellsType.EmptyTile;
            myConnection.CardTypeSelected = CardType.CharacterCard;
            ResetAllButtonToNormalColor();
            GetComponent<Image>().color = new Color(0.725f, 1f, 0.725f);
        }
        else if(houseType = GetComponent<HouseType>())
        {
            myConnection.HouseCardSelected = houseType.houseCellsType;
            myConnection.CharacterdCardSelected = CharactersType.Empty;
            myConnection.CardTypeSelected = CardType.HouseCard;
            ResetAllButtonToNormalColor();
            GetComponent<Image>().color = new Color(0.725f, 1f, 0.725f);

        }

    }

    void ResetAllButtonToNormalColor()
    {
        for (int i = 0; i < 4; i++)
        {
            HouseDeckManager.HousesCardsDeckPickable[i].GetComponent<Image>().color = Color.white;
        }
        for (int i = 0; i < 4; i++)
        {
            CharDeckManager.CharacterCardsDeckPickable[i].GetComponent<Image>().color = Color.white;
        }

    }
}
