using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsManager : MonoBehaviour {

    public enum CharactersType
    {
        Empty,
        NoramlGuy,
        DoubleGuys,
        TripleGuys,
        RedGuy,
        RedNoBlueGuy,
        BlueGuy,
        BlueNoYellow,
        PurpuleGuy,
        PurpleNoRedGuy,
        OldGuy,
        PenthouseGuy,
        TwoHouseGuy,
        ThreeHouseLGuy,
        FourHouseGuy,
        Ghost,
        FamilyThreeGuys,
        Animal,
        GuyWithAnimal,
        GuyNeedParking,
        GuyNeedGarden,
        Baby,
        GhostCatcher,
        Gangster,
        Wizard
    }

    public enum HouseCellsType
    {
        BannedTile,
        EmptyTile,
        BlueTile,
        RedTile,
        PurpleTile,
        YellowTile,
        OldBlueTile,
        OldRedTile,
        OldPurpleTile,
        OldYellowTile,
        PentHouse,
        Parking,
        Terrace,
        Garden,
    }

    public HouseCellsType[,] HouseCellsArray;
    public CharactersType[,] CharactersCellsArray;

    private BoardGenerator boardGenerator;

    void Start ()
    {
        boardGenerator = GetComponent<BoardGenerator>();
        HouseCellsArray = new HouseCellsType[boardGenerator.BoardCellsInRow, boardGenerator.BoardCellsInRow];
        CharactersCellsArray = new CharactersType[boardGenerator.BoardCellsInRow, boardGenerator.BoardCellsInRow];

        #region Initializing CharactersCellsArray With Empty Characters
        for (int i = 0; i < CharactersCellsArray.GetLength(0); i++)
        {
            for (int j = 0; j < CharactersCellsArray.GetLength(1); j++)
            {
                CharactersCellsArray[i, j] = CharactersType.Empty;
                print("CharactersCellsArray[" + i + "," + j + "]: " + CharactersCellsArray[i, j]);
            }
        }
        #endregion
    }

}
