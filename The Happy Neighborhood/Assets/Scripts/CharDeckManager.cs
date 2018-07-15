using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDeckManager : MonoBehaviour
{

    public GameObject[] CharacterPrefabs;   // Creat Character cards decks out of these prefabs
    private List<GameObject> charDeck = new List<GameObject>(); // list of Character cards that shuffle and use during game
    private GameObject[] CaharctersPickableInSlots;       // an array of characters to use in game
    public GameObject[] ParenChartSlots = new GameObject[4];    // 4 ui panel that characters cards go inside them
    public byte charSlotNumbers = 4;    // the number for creating cards in game

	void Start ()
    {
        #region Initializing charDeck List based on the Character Cards
        for (int i = 0; i < CharacterPrefabs.Length; i++)
        {

            for (int j = 0; j < CharacterPrefabs[i].GetComponent<CharType>().CardRatioInDeck; j++)
            {
                charDeck.Add(CharacterPrefabs[i]);
            }
        }
        #endregion

        CaharctersPickableInSlots = new GameObject[charSlotNumbers];
        ShuffleAndGrantCard();
    }

    #region ShuffleAndGrantCard()
    void ShuffleAndGrantCard()
    {
        for (int i = 0; i < charSlotNumbers; i++)
        {
            if(CaharctersPickableInSlots[i] == null)
            {
                CaharctersPickableInSlots[i] = charDeck[ListIndex()];    // check not select more than twice similar cards
                Instantiate<GameObject>(CaharctersPickableInSlots[i]).GetComponent<Transform>().SetParent(ParenChartSlots[i].transform,false);
                charDeck.RemoveAt(ListIndex());
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
            tempIndex = Random.Range(0, charDeck.Count);
            for (int j = 0; j < charSlotNumbers; j++)
            {
                if (CaharctersPickableInSlots[j] == charDeck[tempIndex] && CaharctersPickableInSlots[j] != null)
                {
                    RepeatedFlag++;
                }
            }

        } while (RepeatedFlag > 2);

        return tempIndex;
    }
    #endregion



}
