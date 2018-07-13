using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using System;

public class BoardGenerator : MonoBehaviour
{

    public GameObject[] NoConstructionPrefab;
    public GameObject GridLinePrefab;
    public GameObject GridCellPrefab;
    public GameObject BoardBackground;
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


    void Start()
    {
        BoardCellsArray = new GameObject[BoardCellsInRow, BoardCellsInRow];
        GenerateBoard();

    }



    void GenerateBoard()
    {

        // Set the Board Size Based on the input
        BoardBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(BoardSizeInPixle, BoardSizeInPixle);

        generateGridLines();
        generateGridCells();
        GeneratingNoConstCells();

    }

    #region generateGridLines()
    void generateGridLines()
    {
        GameObject tempGridRow; // Temp GameObject for generating gridlines

        #region Calculating GridLine distance to each grids
        gridLinesDistnace = BoardSizeInPixle / BoardCellsInRow;
        gridLinesDistnace = Mathf.Round(gridLinesDistnace * 100f) / 100f;
        #endregion

        #region Calculating first grid position to generate
        firstGridPosition = gridLinesDistnace - (BoardSizeInPixle / 2);
        firstGridPosition = Mathf.Round(firstGridPosition * 100f) / 100f;
        #endregion

        #region Calculating first cell position to generate
        firstCellPosition = -(BoardSizeInPixle / 2);
        #endregion


        #region Generating Vertical Lines
        for (int i = 0; i < (BoardCellsInRow - 1); i++)
        {
            RectTransform gridRectTransform;
            tempGridRow = Instantiate<GameObject>(GridLinePrefab);          // Generating Vertical Lines
            gridRectTransform = tempGridRow.GetComponent<RectTransform>();  // Getting RectTransform Component
            tempGridRow.transform.SetParent(verticalParent, false);         // Setting the parent to stay clean in hierarchy
            gridRectTransform.anchoredPosition = new Vector3(firstGridPosition + (i * gridLinesDistnace), 0, 0);    // set the position for line
            gridRectTransform.sizeDelta = new Vector2(GridLineWidth, BoardSizeInPixle); // set the line width and height
        }
        #endregion

        #region Generating Horizontal Lines
        for (int i = 0; i < (BoardCellsInRow - 1); i++)
        {
            RectTransform gridRectTransform;
            tempGridRow = Instantiate<GameObject>(GridLinePrefab);          // Generating Horizontal Lines
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
            }
        }

    }
    #endregion

    #region NoRandomRepeatedCell
    void GeneratingNoConstCells()
    {
        int[,] tempNoConstruction = new int[2, NoConstructionCells];
        for (int i = 0; i < NoConstructionCells; i++)
        {
            bool RepeatedCell = false;
            int randomCellsFirstDemension;
            int randomCellsSecondtDemension;
            int randomNoConstPrefabIndex = UnityEngine.Random.Range(0, (NoConstructionPrefab.Length));

            // Checking not to use repeated cell for NoConstructionCell
            do
            {
                RepeatedCell = false;
                randomCellsFirstDemension = UnityEngine.Random.Range(0, BoardCellsInRow);
                randomCellsSecondtDemension = UnityEngine.Random.Range(0, BoardCellsInRow);
                for (int j = 0; j < NoConstructionCells; j++)
                {
                    if (randomCellsFirstDemension == tempNoConstruction[0, j])
                    {
                        if (randomCellsSecondtDemension == tempNoConstruction[1, j])
                        {
                            RepeatedCell = true;
                            print("Repeated");
                        }
                    }
                }
            }
            while (RepeatedCell);

            // Changing Board Cell Image to a NoConstructionCell
            BoardCellsArray[randomCellsFirstDemension, randomCellsSecondtDemension].GetComponent<Image>().overrideSprite =
                NoConstructionPrefab[randomNoConstPrefabIndex].GetComponent<Image>().sprite;

            // Making NoConstructionCell to unInteractable Button
            BoardCellsArray[randomCellsFirstDemension, randomCellsSecondtDemension].GetComponent<Button>().interactable = false;

            tempNoConstruction[0, i] = randomCellsFirstDemension;
            tempNoConstruction[1, i] = randomCellsSecondtDemension;

        }
    }
    #endregion

}
