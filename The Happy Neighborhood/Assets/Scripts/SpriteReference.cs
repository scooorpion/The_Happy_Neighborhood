using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SpriteReference
{
<<<<<<< HEAD
    #region Sprite Refrences
=======
>>>>>>> parent of 4a1bad5... Working on multiple Chars and random sprites
    [Header("House Sprite: ")]

    [SerializeField]
    public Sprite BannedTile;
    [SerializeField]
    public Sprite EmptyTile;
    [SerializeField]
    public Sprite BlueTile;
    [SerializeField]
    public Sprite RedTile;
    [SerializeField]
    public Sprite PurpleTile;
    [SerializeField]
    public Sprite YellowTile;
    [SerializeField]
    public Sprite OldBlueTile;
    [SerializeField]
    public Sprite OldRedTile;
    [SerializeField]
    public Sprite OldPurpleTile;
    [SerializeField]
    public Sprite OldYellowTile;
    [SerializeField]
    public Sprite PentHouse;
    [SerializeField]
    public Sprite Parking;
    [SerializeField]
    public Sprite Terrace;
    [SerializeField]
    public Sprite Garden;

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

<<<<<<< HEAD

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


    //---------------- Functions -------------------

    #region BannedTileSprite(int IndexGenerated)
    public Sprite BannedTileSprite(int IndexGenerated)
    {
            return BannedTile[IndexGenerated];
    }
    #endregion

    #region BlueTileSprite(int IndexGenerated)
    public Sprite BlueTileSprite(int IndexGenerated)
    {
        return BlueTile[IndexGenerated];
    }
    #endregion

    #region RedTileSprite(int IndexGenerated)
    public Sprite RedTileSprite(int IndexGenerated)
    {
        return RedTile[IndexGenerated];
    }
    #endregion

    #region PurpleTileSprite(int IndexGenerated)
    public Sprite PurpleTileSprite(int IndexGenerated)
    {
        return PurpleTile[IndexGenerated];
    }
    #endregion

    #region YellowTileSprite(int IndexGenerated)
    public Sprite YellowTileSprite(int IndexGenerated)
    {
        return YellowTile[IndexGenerated];
    }
    #endregion

    #region OldBlueTileSprite(int IndexGenerated)
    public Sprite OldBlueTileSprite(int IndexGenerated)
    {
        return OldBlueTile[IndexGenerated];
    }
    #endregion

    #region OldRedTileSprite(int IndexGenerated)
    public Sprite OldRedTileSprite(int IndexGenerated)
    {
        return OldRedTile[IndexGenerated];
    }
    #endregion

    #region OldPurpleTileSprite(int IndexGenerated)
    public Sprite OldPurpleTileSprite(int IndexGenerated)
    {
        return OldPurpleTile[IndexGenerated];
    }
    #endregion

    #region OldYellowSprite(int IndexGenerated)
    public Sprite OldYellowSprite(int IndexGenerated)
    {
        return OldYellowTile[IndexGenerated];
    }
    #endregion

    #region PentHouseSprite(int IndexGenerated)
    public Sprite PentHouseSprite(int IndexGenerated)
    {
        return PentHouse[IndexGenerated];
    }
    #endregion

    #region GardenTileSprite(int IndexGenerated)
    public Sprite GardenTileSprite(int IndexGenerated)
    {
        return Garden[IndexGenerated];
    }
    #endregion

    #region TerraceSprite(int IndexGenerated)
    public Sprite TerraceSprite(int IndexGenerated)
    {
        return Terrace[IndexGenerated];
    }
    #endregion

    #region ParkingSprite(int IndexGenerated)
    public Sprite ParkingSprite(int IndexGenerated)
    {
        return Parking[IndexGenerated];
    }
    #endregion


    #region BannedTileSprite()
    public int BannedTileSprite()
    {
        return UnityEngine.Random.Range(0, BannedTile.Length);
    }
    #endregion

    #region BlueTileSprite()
    public int BlueTileSprite()
    {
        return (UnityEngine.Random.Range(0, BlueTile.Length));
    }
    #endregion

    #region RedTileSprite()
    public int RedTileSprite()
    {
        return UnityEngine.Random.Range(0, RedTile.Length);
    }
    #endregion

    #region PurpleTileSprite()
    public int PurpleTileSprite()
    {
        return UnityEngine.Random.Range(0, PurpleTile.Length);
    }
    #endregion

    #region YellowTileSprite()
    public int YellowTileSprite()
    {
        return UnityEngine.Random.Range(0, YellowTile.Length);
    }
    #endregion

    #region OldBlueTileSprite()
    public int OldBlueTileSprite()
    {
        return UnityEngine.Random.Range(0, OldBlueTile.Length);
    }
    #endregion

    #region OldRedTileSprite()
    public int OldRedTileSprite()
    {
        return UnityEngine.Random.Range(0, OldRedTile.Length);
    }
    #endregion

    #region OldPurpleTileSprite()
    public int OldPurpleTileSprite()
    {
        return UnityEngine.Random.Range(0, OldPurpleTile.Length);
    }
    #endregion

    #region OldYellowSprite()
    public int OldYellowSprite()
    {
        return UnityEngine.Random.Range(0, OldYellowTile.Length);
    }
    #endregion

    #region PentHouseSprite()
    public int PentHouseSprite()
    {
        return UnityEngine.Random.Range(0, PentHouse.Length);
    }
    #endregion

    #region GardenTileSprite()
    public int GardenTileSprite()
    {
        return UnityEngine.Random.Range(0, Garden.Length);
    }
    #endregion

    #region TerraceSprite()
    public int TerraceSprite()
    {
        return UnityEngine.Random.Range(0, Terrace.Length);
    }
    #endregion

    #region ParkingSprite()
    public int ParkingSprite()
    {
        return UnityEngine.Random.Range(0, Parking.Length);
    }
    #endregion

=======
>>>>>>> parent of 4a1bad5... Working on multiple Chars and random sprites
}
