using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CellSelecting : MonoBehaviour
{
    public static bool IsTwoUnitCharacterSelected = false;
    public static int CellSelectedForTwoUnitCharacter = 0;

    public static List<int> SelectedIndexesForTwoUnitCharacter = new List<int>();


    public static bool IsFourUnitCharacterSelected = false;
    public static int CellSelectedForFourUnitCharacter = 0;

    public static List<int> SelectedIndexesForFourUnitCharacter = new List<int>();

    public static bool IsThreeUnitCharacterSelected = false;
    public static int CellSelectedForThreeUnitCharacter = 0;

    public static List<int> SelectedIndexesForThreeUnitCharacter = new List<int>();

    PlayerConnection myConnection;
    GameManager gameManager;

    private void Start()
    {
        myConnection = GameObject.FindGameObjectWithTag("MyConnection").GetComponent<PlayerConnection>();
        gameManager = FindObjectOfType<GameManager>();
    }


    public void SelectCell()
    {
        if (myConnection.CardTypeSelected == CardType.NoSelection)
        {
            //print("No Selection");
            return;
        }

        if (myConnection.CharacterdCardSelected != CharactersType.Empty || myConnection.HouseCardSelected != HouseCellsType.EmptyTile)
        {
            if (myConnection.CharacterdCardSelected == CharactersType.TwoHouseGuy)
            {
                CellSelectedForTwoUnitCharacter++;

                if (CellSelectedForTwoUnitCharacter <= 2)
                {
                    GetComponent<Image>().color = new Color(0.62f, 1, 0.62f, 0.8f);
                    SelectedIndexesForTwoUnitCharacter.Add(GetComponent<CellIndex>().cellIndex);
                    // Change Color of Selected Cell ....
                }

                if (CellSelectedForTwoUnitCharacter == 2)// When Two Cell is selected
                {
                    // send List For checking to Server
                    myConnection.CommandToCheckMultipleCharsOnSelectedCells(SelectedIndexesForTwoUnitCharacter.ToArray(), CharactersType.TwoHouseGuy);

                    // Reset Flags :
                    IsTwoUnitCharacterSelected = false;
                    SelectedIndexesForTwoUnitCharacter.Clear();
                    CellSelectedForTwoUnitCharacter = 0;
                    gameManager.ResetAllCellsColorToDefault();
                }

                return;
            }
            else if (myConnection.CharacterdCardSelected == CharactersType.FourHouseGuy)
            {
                CellSelectedForFourUnitCharacter++;

                if (CellSelectedForFourUnitCharacter <= 4)
                {
                    GetComponent<Image>().color = new Color(0.62f, 1, 0.62f, 0.8f);
                    SelectedIndexesForFourUnitCharacter.Add(GetComponent<CellIndex>().cellIndex);
                    // Change Color of Selected Cell ....
                }

                if (CellSelectedForFourUnitCharacter == 4)// When Four Cell is selected
                {
                    // send List For checking to Server
                    myConnection.CommandToCheckMultipleCharsOnSelectedCells(SelectedIndexesForFourUnitCharacter.ToArray(), CharactersType.FourHouseGuy);

                    // Reset Flags :
                    IsFourUnitCharacterSelected = false;
                    SelectedIndexesForFourUnitCharacter.Clear();
                    CellSelectedForFourUnitCharacter = 0;
                    gameManager.ResetAllCellsColorToDefault();

                }

                return;
            }
            else if (myConnection.CharacterdCardSelected == CharactersType.ThreeHouseLGuy)
            {
                CellSelectedForThreeUnitCharacter++;

                if (CellSelectedForThreeUnitCharacter <= 3)
                {
                    GetComponent<Image>().color = new Color(0.62f, 1, 0.62f, 0.8f);
                    SelectedIndexesForThreeUnitCharacter.Add(GetComponent<CellIndex>().cellIndex);
                    // Change Color of Selected Cell ....
                }

                if (CellSelectedForThreeUnitCharacter == 3)// When Three Cell is selected
                {
                    // send List For checking to Server
                    myConnection.CommandToCheckMultipleCharsOnSelectedCells(SelectedIndexesForThreeUnitCharacter.ToArray(), CharactersType.ThreeHouseLGuy);

                    // Reset Flags :
                    IsThreeUnitCharacterSelected = false;
                    SelectedIndexesForThreeUnitCharacter.Clear();
                    CellSelectedForThreeUnitCharacter = 0;
                    gameManager.ResetAllCellsColorToDefault();

                }

                return;
            }

            // ------------- Before --------------------

            if (myConnection.CharacterdCardSelected == CharactersType.Ghost)
            {
                gameManager.DisableEnemyTilesActiveForGhosts();
            }

            myConnection.CommandToCheckSelectedCell(GetComponent<CellIndex>().cellIndex, GetComponent<CellIndex>().CellType);

            myConnection.CharacterdCardSelected = CharactersType.Empty;
            myConnection.HouseCardSelected = HouseCellsType.EmptyTile;
            //print("Select Card");

        }
    }
}
