using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDeckManager : MonoBehaviour
{
    public GameObject EmptyCardPrefab;
    public GameObject[] ParentCharacterSlots = new GameObject[4];  // 4 ui panel that character tiles cards go inside them
    public static GameObject[] CharacterCardsDeckPickable = new GameObject[4];   // an array of characterTiles to use in game

    void Start()
    {
        FirstBuildArray();
    }

    void FirstBuildArray()
    {
        for (int i = 0; i < 4; i++)
        {
            CharacterCardsDeckPickable[i] = Instantiate(EmptyCardPrefab);
            CharacterCardsDeckPickable[i].GetComponent<Transform>().SetParent(ParentCharacterSlots[i].transform, false);
        }
    }

}
