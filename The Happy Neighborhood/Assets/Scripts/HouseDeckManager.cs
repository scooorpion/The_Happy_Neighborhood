using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDeckManager : MonoBehaviour
{
    public GameObject[] HousePrefabs;  // Creat houseTiles card decks out of these prefabs
    private List<GameObject> houseDeck = new List<GameObject>();  // list of houseTiles cards that shuffle and use during game
    private GameObject[] HousesTilesPickableInSlots;   // an array of houseTiles to use in game
    public GameObject[] ParentHouseSlots = new GameObject[4];  // 4 ui panel that house tiles cards go inside them
    public byte houseSlotNumbers = 4; // the number for creating cards in game

    void Start()
    {
        #region Initializing houseDeck List based on the House Cards
        for (int i = 0; i < HousePrefabs.Length; i++)
        {
            for (int j = 0; j < HousePrefabs[i].GetComponent<HouseType>().CardRatioInDeck; j++)
            {
                houseDeck.Add(HousePrefabs[i]);
            }
        }
        #endregion

        HousesTilesPickableInSlots = new GameObject[houseSlotNumbers];
        ShuffleAndGrantCard();

    }

    #region ShuffleAndGrantCard()
    void ShuffleAndGrantCard()
    {
        for (int i = 0; i < houseSlotNumbers; i++)
        {
            if (HousesTilesPickableInSlots[i] == null)
            {
                // check not select more than twice similar cards
                HousesTilesPickableInSlots[i] = houseDeck[ListIndex()];
                Instantiate<GameObject>(HousesTilesPickableInSlots[i]).GetComponent<Transform>().SetParent(ParentHouseSlots[i].transform, false);
                houseDeck.RemoveAt(ListIndex());
            }
        }
    }
    #endregion

    #region ListIndex() Check Card Index Not To Repeat More Than Twice
    private int ListIndex()
    {
        byte RepeatedFlag;
        int tempIndex;
        do
        {
            RepeatedFlag = 0;
            tempIndex = Random.Range(0, houseDeck.Count);
            for (int j = 0; j < houseSlotNumbers; j++)
            {
                if (HousesTilesPickableInSlots[j] == houseDeck[tempIndex] && HousesTilesPickableInSlots[j] != null)
                {
                    RepeatedFlag++;
                }
            }

        } while (RepeatedFlag > 2);

        return tempIndex;
    }
    #endregion

}
