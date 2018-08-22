using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SpriteReference
{
    #region Sprite Refrences Array
    [Header("House Sprite: ")]

    [SerializeField]
    public Sprite[] BannedTile;
    [SerializeField]
    public Sprite EmptyTile;
    [SerializeField]
    public Sprite[] BlueTile;
    [SerializeField]
    public Sprite[] RedTile;
    [SerializeField]
    public Sprite[] PurpleTile;
    [SerializeField]
    public Sprite[] YellowTile;
    [SerializeField]
    public Sprite[] OldBlueTile;
    [SerializeField]
    public Sprite[] OldRedTile;
    [SerializeField]
    public Sprite[] OldPurpleTile;
    [SerializeField]
    public Sprite[] OldYellowTile;
    [SerializeField]
    public Sprite[] PentHouse;
    [SerializeField]
    public Sprite[] Parking;
    [SerializeField]
    public Sprite[] Terrace;
    [SerializeField]
    public Sprite[] Garden;

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
    #endregion

    #region Sprite Refrences Array
    [Header("House Sprite: ")]

    public static Sprite BannedTile_Randomized;
    public static Sprite BlueTile_Randomized;
    public static Sprite RedTile_Randomized;
    public static Sprite PurpleTile_Randomized;
    public static Sprite YellowTile_Randomized;
    public static Sprite OldBlueTile_Randomized;
    public static Sprite OldRedTile_Randomized;
    public static Sprite OldPurpleTile_Randomized;
    public static Sprite OldYellowTile_Randomized;
    public static Sprite PentHouse_Randomized;
    public static Sprite Parking_Randomized;
    public static Sprite Terrace_Randomized;
    public static Sprite Garden_Randomized;

    #endregion

    public Sprite BannedTileSprite()
    {
        BannedTile_Randomized = BannedTile[UnityEngine.Random.Range(0, BannedTile.Length)];
        return BannedTile_Randomized;
    }

    public Sprite BlueTileSprite()
    {
        BlueTile_Randomized = BlueTile[UnityEngine.Random.Range(0, BlueTile.Length)];
        return BlueTile_Randomized;
    }

    public Sprite RedTileSprite()
    {
        RedTile_Randomized = RedTile[UnityEngine.Random.Range(0, RedTile.Length)];
        return RedTile_Randomized;
    }

    public Sprite PurpleTileSprite()
    {
        PurpleTile_Randomized = PurpleTile[UnityEngine.Random.Range(0, PurpleTile.Length)];
        return PurpleTile_Randomized;
    }

    public Sprite YellowTileSprite()
    {
        YellowTile_Randomized = YellowTile[UnityEngine.Random.Range(0, YellowTile.Length)];
        return YellowTile_Randomized;
    }
    public Sprite OldBlueTileSprite()
    {
        OldBlueTile_Randomized = OldBlueTile[UnityEngine.Random.Range(0, OldBlueTile.Length)];
        return OldBlueTile_Randomized;
    }

    public Sprite OldRedTileSprite()
    {
        OldRedTile_Randomized = OldRedTile[UnityEngine.Random.Range(0, OldRedTile.Length)];
        return OldRedTile_Randomized;
    }

    public Sprite OldPurpleTileSprite()
    {
        OldPurpleTile_Randomized = OldPurpleTile[UnityEngine.Random.Range(0, OldPurpleTile.Length)];
        return OldPurpleTile_Randomized;
    }

    public Sprite OldYellowSprite()
    {
        OldYellowTile_Randomized = OldYellowTile[UnityEngine.Random.Range(0, OldYellowTile.Length)];
        return OldYellowTile_Randomized;
    }

    public Sprite PentHouseSprite()
    {
        PentHouse_Randomized = PentHouse[UnityEngine.Random.Range(0, PentHouse.Length)];
        return PentHouse_Randomized;
    }

    public Sprite GardenTileSprite()
    {
        Garden_Randomized = Garden[UnityEngine.Random.Range(0, Garden.Length)];
        return Garden_Randomized;
    }

    public Sprite TerraceSprite()
    {
        Terrace_Randomized = Terrace[UnityEngine.Random.Range(0, Terrace.Length)];
        return Terrace_Randomized;
    }

    public Sprite ParkingSprite()
    {
        Parking_Randomized = Parking[UnityEngine.Random.Range(0, Parking.Length)];
        return Parking_Randomized;
    }

}
