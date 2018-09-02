using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterLife : MonoBehaviour {

    public RectTransform HealthBar;
    public float CharacterCreationTime;
    public float LifeTimeLeft = 0;
    public float RatioToBarWidth = 0;
    public float BarWidht = 90;

    public bool IsCharacterLifeTimeStart = false;

    GameManager gameManager;
    PlayerConnection playerConnection;
    public CharactersType Character;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerConnection = GameObject.FindGameObjectWithTag("MyConnection").GetComponent<PlayerConnection>();
    }

    void Update ()
    {

        /*
        if (IsCharacterLifeTimeStart)
        {
            if (BarWidht >= 0)
            {
                BarWidht -= (Time.deltaTime * RatioToBarWidth);
                HealthBar.sizeDelta = new Vector2(12, BarWidht);
            }
            else
            {
                IsCharacterLifeTimeStart = false;

                print("Character is Dead");
                //BarWidht = 90;

                // Must Kill The Character
                gameManager.ShowDeathChar(Character);

                if(playerConnection.MyTurnID == 1)
                {
                    playerConnection.CmdAskToRemoveDeadCharacter(Character);
                }


            }

        }
        */

    }

    #region StartCharacterLife(float CharacterCreationTime, CharactersType Character, float serverTime): When a character timer must set to true and be calculateed
    /// <summary>
    /// When a character timer must set to true and be calculateed
    /// </summary>
    /// <param name="CharacterCreationTime"></param>
    /// <param name="Character"></param>
    /// <param name="serverTime"></param>
    public void StartCharacterLife(float CharacterCreationTime, CharactersType Character, float serverTime)
    {
        print("StartCharacterLife");
        LifeTimeLeft = (CharacterCreationTime + CharLifeTime(Character)) - serverTime ;
        print("LifeTimeLeft: " + LifeTimeLeft);
        BarWidht = (LifeTimeLeft * 90) / CharLifeTime(Character);
        RatioToBarWidth = 90 / CharLifeTime(Character);
        this.Character = Character;

        if (LifeTimeLeft>0)
        {
            IsCharacterLifeTimeStart = true;
        }
    }
    #endregion

    public void UpdateCharactersHealthBar(float LifeBar)
    {
        HealthBar.sizeDelta = new Vector2(12, LifeBar);
    }

    #region CharLifeTime(CharactersType Character): Calculate Life Time of each character
    /// <summary>
    /// Calculate Life Time of each character
    /// </summary>
    /// <param name="Character"></param>
    /// <returns></returns>
    public static float CharLifeTime(CharactersType Character)
    {
        float tempLifeTime = 0;

        switch (Character)
        {
            case CharactersType.NoramlGuy:
                tempLifeTime = 40;
                break;
            case CharactersType.RedGuy:
                tempLifeTime = 60;
                break;
            case CharactersType.RedNoBlueGuy:
                tempLifeTime = 60;
                break;
            case CharactersType.BlueGuy:
                tempLifeTime = 60;
                break;
            case CharactersType.BlueNoYellow:
                tempLifeTime = 60;
                break;
            case CharactersType.PurpuleGuy:
                tempLifeTime = 60;
                break;
            case CharactersType.PurpleNoRedGuy:
                tempLifeTime = 60;
                break;
            case CharactersType.OldGuy:
                tempLifeTime = 20;
                break;
            case CharactersType.PenthouseGuy:
                tempLifeTime = 180;
                break;
            case CharactersType.TwoHouseGuy:
                tempLifeTime = 90;
                break;
            case CharactersType.ThreeHouseLGuy:
                tempLifeTime = 90;
                break;
            case CharactersType.FourHouseGuy:
                tempLifeTime = 180;
                break;
            default:
                break;
        }

        return tempLifeTime;
    }
    #endregion
}
