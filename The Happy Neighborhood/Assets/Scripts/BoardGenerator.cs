using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using System;

public enum BoardType { MyBoard, EnemyBoard }

public class BoardGenerator : MonoBehaviour
{
    #region Fileds
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
        FamilyTwoGuys,
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
        BannedTile,
        EmptyTile,

    }

    public BoardType boardType;
    public GameObject[] NoConstructionPrefab;
    public GameObject GridLineVerticalPrefab;
    public GameObject GridLineHorizontalPrefab;
    public GameObject GridCellPrefab;
    public GameObject BoardBackground;
    public Text UserNameTextBox;
    public Text scoreText;
    public Text ScoreLable;
    public Text GhostNumberText;
    public GameObject TurnPanel;
    public GameObject GhostPanel;


    public float BoardSizeInPixle = 700f;
    public byte BoardCellsInRow = 7;
    public byte GridLineWidth = 5;
    public byte NoConstructionCells = 4;

    private float gridLinesDistnace;
    private float firstGridPosition;
    private float firstCellPosition;
    private float cellSizeWithGridLineOffset;

    public Transform verticalParent;
    public Transform horizontalParent;
    public Transform cellsParent;

    public GameObject[,] BoardCellsArray;


    public HouseCellsType[,] HouseCellsArray;
    public CharactersType[,] CharactersCellsArray;
    #endregion

    void Start()
    {
        BoardCellsArray = new GameObject[BoardCellsInRow, BoardCellsInRow];
        HouseCellsArray = new HouseCellsType[BoardCellsInRow, BoardCellsInRow];
        CharactersCellsArray = new CharactersType[BoardCellsInRow, BoardCellsInRow];

        #region Initializing CharactersCellsArray With Empty Characters
        for (int i = 0; i < BoardCellsInRow; i++)
        {
            for (int j = 0; j < BoardCellsInRow; j++)
            {
                CharactersCellsArray[i, j] = CharactersType.Empty;
            }
        }
        #endregion

        GenerateBoard();
    }


    #region GenerateBoard()
    public void GenerateBoard()
    {

        // Set the Board Size Based on the input
        BoardBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(BoardSizeInPixle, BoardSizeInPixle);

        generateGridLines();
        generateGridCells();
        //GeneratingNoConstCells();

    }
    #endregion

    #region generateGridLines()
    void generateGridLines()
    {
        GameObject tempGridRow; // Temp GameObject for generating gridlines

        #region Calculating GridLine distance to each grids
        gridLinesDistnace = BoardSizeInPixle / BoardCellsInRow;
        gridLinesDistnace = Mathf.Round(gridLinesDistnace * 100f) / 100f;
        #endregion

        #region Calculating first grid position to generate
        //firstGridPosition = gridLinesDistnace - (BoardSizeInPixle / 2);

        firstGridPosition =  -(BoardSizeInPixle / 2);
        firstGridPosition = Mathf.Round(firstGridPosition * 100f) / 100f;

        #endregion

        #region Calculating first cell position to generate
        firstCellPosition = -(BoardSizeInPixle / 2);
        #endregion


        #region Generating Vertical Lines
        for (int i = 0; i < (BoardCellsInRow+1); i++)
        {
            RectTransform gridRectTransform;
            tempGridRow = Instantiate<GameObject>(GridLineVerticalPrefab);          // Generating Vertical Lines
            gridRectTransform = tempGridRow.GetComponent<RectTransform>();  // Getting RectTransform Component
            tempGridRow.transform.SetParent(verticalParent, false);         // Setting the parent to stay clean in hierarchy
            gridRectTransform.anchoredPosition = new Vector3(firstGridPosition + (i * gridLinesDistnace), 0, 0);    // set the position for line
            gridRectTransform.sizeDelta = new Vector2(GridLineWidth, BoardSizeInPixle); // set the line width and height
        }
        #endregion

        #region Generating Horizontal Lines
        for (int i = 0; i < (BoardCellsInRow +1); i++)
        {
            RectTransform gridRectTransform;
            tempGridRow = Instantiate<GameObject>(GridLineHorizontalPrefab);          // Generating Horizontal Lines
            gridRectTransform = tempGridRow.GetComponent<RectTransform>();  // Getting RectTransform Component
            tempGridRow.transform.SetParent(horizontalParent, false);       // Setting the parent to stay clean in hierarchy
            gridRectTransform.anchoredPosition = new Vector3(0, firstGridPosition + (i * gridLinesDistnace), 0);    // set the position for line
            gridRectTransform.sizeDelta = new Vector2(BoardSizeInPixle, GridLineWidth);     // set the line width and height
        }
        #endregion
    }
    #endregion

    #region generateGridCells()
    void generateGridCells()
    {
        int CurrentCellIndex = 0;
        for (int i = 0; i < BoardCellsInRow; i++)
        {
            for (int j = 0; j < BoardCellsInRow; j++)
            {
                RectTransform cellRectTransform;
                BoardCellsArray[i, j] = Instantiate<GameObject>(GridCellPrefab);
                cellRectTransform = BoardCellsArray[i, j].GetComponent<RectTransform>();
                cellRectTransform.sizeDelta = new Vector2((gridLinesDistnace - GridLineWidth), (gridLinesDistnace - GridLineWidth));
                cellSizeWithGridLineOffset = cellRectTransform.rect.width + GridLineWidth;
                BoardCellsArray[i, j].transform.SetParent(cellsParent, false);
                cellRectTransform.anchoredPosition = new Vector3(firstCellPosition + (cellSizeWithGridLineOffset / 2) + (j * cellSizeWithGridLineOffset),
                                                                  firstCellPosition + (cellSizeWithGridLineOffset / 2) + (i * cellSizeWithGridLineOffset),
                                                                  0);
                BoardCellsArray[i, j].GetComponent<CellIndex>().cellIndex = CurrentCellIndex;
                BoardCellsArray[i, j].GetComponent<CellIndex>().CellType = boardType;

                // Initializing HouseCells Array With Empty Tiles 
                HouseCellsArray[i, j] = HouseCellsType.EmptyTile;

                CurrentCellIndex++;
            }
        }

    }
    #endregion


    #region GeneratingNoConstCells(int[] FirstDimension,int[] SecondDimension)
    public void GeneratingNoConstCells(int[] FirstDimension,int[] SecondDimension)
    {
        int[,] tempNoConstruction = new int[2, NoConstructionCells];
        for (int i = 0; i < NoConstructionCells; i++)
        {
            int randomNoConstPrefabIndex = UnityEngine.Random.Range(0, (NoConstructionPrefab.Length));

            // Changing Board Cell Image to a NoConstructionCell
            BoardCellsArray[FirstDimension[i], SecondDimension[i]].GetComponent<Image>().overrideSprite =
                NoConstructionPrefab[randomNoConstPrefabIndex].GetComponent<Image>().sprite;

            // Making NoConstructionCell to unInteractable Button
            BoardCellsArray[FirstDimension[i], SecondDimension[i]].GetComponent<Button>().interactable = false;

            // Assigning Banned Tile To HouseCells Array
            HouseCellsArray[FirstDimension[i], SecondDimension[i]] = HouseCellsType.BannedTile;
        }
    }
    #endregion



    
}
