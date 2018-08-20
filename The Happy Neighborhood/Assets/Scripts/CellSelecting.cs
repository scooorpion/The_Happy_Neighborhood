using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CellSelecting : MonoBehaviour
{

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
