using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HouseType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public HouseCellsType houseCellsType;

    private HouseDeckManager houseDeckManager;

    private Sprite HelpSprite;

    [SerializeField]
    public SpriteReference_HouseHelp spriteReference_HouseHelp;

    private void Start()
    {
        houseDeckManager = FindObjectOfType<HouseDeckManager>();
        ShowCharacterInfo();
    }

    public void ShowCharacterInfo()
    {

        switch (houseCellsType)
        {
            case HouseCellsType.BlueTile:
                HelpSprite = spriteReference_HouseHelp.BlueTile_Help;
                break;
            case HouseCellsType.RedTile:
                HelpSprite = spriteReference_HouseHelp.RedTile_Help;
                break;
            case HouseCellsType.PurpleTile:
                HelpSprite = spriteReference_HouseHelp.PurpleTile_Help;
                break;
            case HouseCellsType.YellowTile:
                HelpSprite = spriteReference_HouseHelp.YellowTile_Help;
                break;
            case HouseCellsType.OldBlueTile:
                HelpSprite = spriteReference_HouseHelp.OldBlueTile_Help;
                break;
            case HouseCellsType.OldRedTile:
                HelpSprite = spriteReference_HouseHelp.OldRedTile_Help;
                break;
            case HouseCellsType.OldPurpleTile:
                HelpSprite = spriteReference_HouseHelp.OldPurpleTile_Help;
                break;
            case HouseCellsType.OldYellowTile:
                HelpSprite = spriteReference_HouseHelp.OldYellowTile_Help;
                break;
            case HouseCellsType.PentHouse:
                HelpSprite = spriteReference_HouseHelp.PentHouse_Help;
                break;
            case HouseCellsType.Parking:
                HelpSprite = spriteReference_HouseHelp.Parking_Help;
                break;
            case HouseCellsType.Terrace:
                HelpSprite = spriteReference_HouseHelp.Terrace_Help;
                break;
            case HouseCellsType.Garden:
                HelpSprite = spriteReference_HouseHelp.Garden_Help;
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowCharacterInfo();
        houseDeckManager.ShowHouseInfo(HelpSprite);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        houseDeckManager.HideHouseInfo();
    }
}
