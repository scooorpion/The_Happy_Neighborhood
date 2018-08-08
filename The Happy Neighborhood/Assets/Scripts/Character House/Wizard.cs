using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Wizard
{
    [SerializeField]
    public Sprite OldBlueTile;

    [SerializeField]
    public Sprite OldRedTile;

    [SerializeField]
    public Sprite OldPurpleTile;

    [SerializeField]
    public Sprite OldYellowTile;


    public Sprite ShowHouse(HouseCellsType House)
    {
        switch (House)
        {
            case HouseCellsType.OldBlueTile:
                return OldBlueTile;
                break;

            case HouseCellsType.OldRedTile:
                return OldRedTile;
                break;

            case HouseCellsType.OldPurpleTile:
                return OldPurpleTile;
                break;

            case HouseCellsType.OldYellowTile:
                return OldYellowTile;
                break;

            default:
                // Vaghti bekhad charactere ro toye khoneye eshtebah az lahaz mantegh bezare k bayad in ghesmat toye server check beshe va injor 
                // ... darkhasti hichvaght etefagh nayofte ke bekhad update beshe
                return null;
                break;
        }
    }

    
}
