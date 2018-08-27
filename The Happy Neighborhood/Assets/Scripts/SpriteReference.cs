using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SpriteReference
{
    #region Sprite Refrences Array
    [Header("House Sprite Array: ")]

    [SerializeField]
    public Sprite[] BannedTile_Arr;
    [SerializeField]
    public Sprite[] BlueTile_Arr;
    [SerializeField]
    public Sprite[] RedTile_Arr;
    [SerializeField]
    public Sprite[] PurpleTile_Arr;
    [SerializeField]
    public Sprite[] YellowTile_Arr;
    [SerializeField]
    public Sprite[] OldBlueTile_Arr;
    [SerializeField]
    public Sprite[] OldRedTile_Arr;
    [SerializeField]
    public Sprite[] OldPurpleTile_Arr;
    [SerializeField]
    public Sprite[] OldYellowTile_Arr;
    [SerializeField]
    public Sprite[] PentHouse_Arr;
    [SerializeField]
    public Sprite[] Parking_Arr;
    [SerializeField]
    public Sprite[] Terrace_Arr;
    [SerializeField]
    public Sprite[] Garden_Arr;
    [SerializeField]
    public Sprite EmptyTile;



    [Header("Character Sprite: ")]
    [SerializeField]
    public Sprite Empty;
    [SerializeField]
    public Sprite NoramlGuy;
    [SerializeField]
    public Sprite DoubleGuys;
    [SerializeField]
    public Sprite TripleGuys;
    [SerializeField]
    public Sprite RedGuy;
    [SerializeField]
    public Sprite RedNoBlueGuy;
    [SerializeField]
    public Sprite BlueGuy;
    [SerializeField]
    public Sprite BlueNoYellow;
    [SerializeField]
    public Sprite PurpuleGuy;
    [SerializeField]
    public Sprite PurpleNoRedGuy;
    [SerializeField]
    public Sprite OldGuy;
    [SerializeField]
    public Sprite PenthouseGuy;
    [SerializeField]
    public Sprite TwoHouseGuy;
    [SerializeField]
    public Sprite ThreeHouseLGuy;
    [SerializeField]
    public Sprite FourHouseGuy;
    [SerializeField]
    public Sprite FamilyTwoGuys;
    [SerializeField]
    public Sprite Ghost;
    [SerializeField]
    public Sprite Animal;
    [SerializeField]
    public Sprite GuyWithAnimal;
    [SerializeField]
    public Sprite GuyNeedParking;
    [SerializeField]
    public Sprite GuyNeedGarden;
    [SerializeField]
    public Sprite Baby;
    [SerializeField]
    public Sprite GhostCatcher;
    [SerializeField]
    public Sprite Gangster;
    [SerializeField]
    public Sprite Wizard;


    [Header("Character's Part Sprite: ")]

    [SerializeField]
    public Sprite TwoGuys_Up;
    [SerializeField]
    public Sprite TwoGuys_Down;
    [SerializeField]
    public Sprite FourHouseGuy_Up_Left;
    [SerializeField]
    public Sprite FourHouseGuy_Up_Right;
    [SerializeField]
    public Sprite FourHouseGuy_Down_Left;
    [SerializeField]
    public Sprite FourHouseGuy_Down_Right;
    [SerializeField]
    public Sprite ThreeHouseLGuy_Up;
    [SerializeField]
    public Sprite ThreeHouseLGuy_Center;
    [SerializeField]
    public Sprite ThreeHouseLGuy_Down_Left;

    public Sprite BannedTile;

    public Sprite BlueTile;

    public Sprite RedTile;

    public Sprite PurpleTile;

    public Sprite YellowTile;

    public Sprite OldBlueTile;

    public Sprite OldRedTile;

    public Sprite OldPurpleTile;

    public Sprite OldYellowTile;

    public Sprite PentHouse;

    public Sprite Parking;

    public Sprite Terrace;

    public Sprite Garden;


    #endregion


    public void FirstHouseSpriteRandomInitialization()
    {
        BannedTile = BannedTile_Arr[UnityEngine.Random.Range(0, BannedTile_Arr.Length)];

        BlueTile = BlueTile_Arr[UnityEngine.Random.Range(0, BlueTile_Arr.Length)];

        RedTile = RedTile_Arr[UnityEngine.Random.Range(0, RedTile_Arr.Length)];

        PurpleTile = PurpleTile_Arr[UnityEngine.Random.Range(0, PurpleTile_Arr.Length)];

        YellowTile = YellowTile_Arr[UnityEngine.Random.Range(0, YellowTile_Arr.Length)];

        OldBlueTile = OldBlueTile_Arr[UnityEngine.Random.Range(0, OldBlueTile_Arr.Length)];

        OldRedTile = OldRedTile_Arr[UnityEngine.Random.Range(0, OldRedTile_Arr.Length)];

        OldPurpleTile = OldPurpleTile_Arr[UnityEngine.Random.Range(0, OldPurpleTile_Arr.Length)];

        OldYellowTile = OldYellowTile_Arr[UnityEngine.Random.Range(0, OldYellowTile_Arr.Length)];
        
        Terrace = Terrace_Arr[UnityEngine.Random.Range(0, Terrace_Arr.Length)];

        PentHouse = PentHouse_Arr[UnityEngine.Random.Range(0, PentHouse_Arr.Length)];

        //Parking = Parking_Arr[UnityEngine.Random.Range(0, Parking_Arr.Length)];

        //Garden = Garden_Arr[UnityEngine.Random.Range(0, Garden_Arr.Length)];

    }

}
