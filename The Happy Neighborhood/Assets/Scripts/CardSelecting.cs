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

        ResetMultipleUnitCharacterFlagToFalse();
        
        if (charType = GetComponent<CharType>())
        {

            if (charType.charactersType == CharactersType.TwoHouseGuy)
            {
                CellSelecting.IsTwoUnitCharacterSelected = true;
            }
            else if (charType.charactersType == CharactersType.FourHouseGuy)
            {
                CellSelecting.IsFourUnitCharacterSelected = true;
            }
            else if (charType.charactersType == CharactersType.ThreeHouseLGuy)
            {
                CellSelecting.IsThreeUnitCharacterSelected = true;
            }



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

        ResetMultipleUnitCharacterFlagToFalse();

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

    void ResetMultipleUnitCharacterFlagToFalse()
    {
        if (CellSelecting.IsTwoUnitCharacterSelected)
        {
            gameManager.ResetAllCellsColorToDefault();
            CellSelecting.IsTwoUnitCharacterSelected = false;
            CellSelecting.CellSelectedForTwoUnitCharacter = 0;
            CellSelecting.SelectedIndexesForTwoUnitCharacter.Clear();

            print("<<< Reset TwoUnit Flag >>>");
        }

        if(CellSelecting.IsFourUnitCharacterSelected)
        {
            gameManager.ResetAllCellsColorToDefault();
            CellSelecting.IsFourUnitCharacterSelected = false;
            CellSelecting.CellSelectedForFourUnitCharacter = 0;
            CellSelecting.SelectedIndexesForFourUnitCharacter.Clear();
        }

        if (CellSelecting.IsThreeUnitCharacterSelected)
        {
            gameManager.ResetAllCellsColorToDefault();
            CellSelecting.IsThreeUnitCharacterSelected = false;
            CellSelecting.CellSelectedForThreeUnitCharacter = 0;
            CellSelecting.SelectedIndexesForThreeUnitCharacter.Clear();
        }


    }
}
