using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CardSelecting : MonoBehaviour
{
    PlayerConnection myConnection;
    SoundManager soundManager;
    GameManager gameManager;

    private void Start()
    {
        myConnection = GameObject.FindGameObjectWithTag("MyConnection").GetComponent<PlayerConnection>();
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<GameManager>();

    }

    public void SelectCard()
    {
        CharType charType;
        HouseType houseType;

        soundManager.SFX_SelectingCardPlay();
        gameManager.DisableEnemyTilesActiveForGhosts();

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
            myConnection.HouseSpriteIndexSelected = houseType.SpriteIndex;

            print("HouseSpriteIndexSelected: " + houseType.SpriteIndex);

            myConnection.CharacterdCardSelected = CharactersType.Empty;
            myConnection.CardTypeSelected = CardType.HouseCard;
            ResetAllButtonToNormalColor();
            GetComponent<Image>().color = new Color(0.725f, 1f, 0.725f);
        }

    }

    public void SelectGhost()
    {
        myConnection.CharacterdCardSelected = CharactersType.Ghost;
        myConnection.CardTypeSelected = CardType.GhostCard;
        myConnection.HouseCardSelected = HouseCellsType.EmptyTile;
        ResetAllButtonToNormalColor();
        GetComponent<Image>().color = new Color(0.725f, 1f, 0.725f);
        gameManager.EnableEnemyTilesActiveForGhosts();
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

        gameManager.ResetGhostColorToWhite();


    }
}
