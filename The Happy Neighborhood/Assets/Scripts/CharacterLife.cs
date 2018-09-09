using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DifficultyLevel
{
    NormalWitch,EvilWitch,CrazyWitch
}
public class CharacterLife : MonoBehaviour {

    public RectTransform HealthBar;
    public float CharacterCreationTime;
    public float LifeTimeLeft = 0;
    public float RatioToBarWidth = 0;
    public float BarWidht = 90;

    public bool IsCharacterLifeTimeStart = false;
    public static DifficultyLevel difficultyLevel = DifficultyLevel.NormalWitch;

    public static float NoramlGuy_rnd;
    public static float RedGuy_rnd;
    public static float RedNoBlueGuy_rnd;
    public static float BlueGuy_rnd;
    public static float BlueNoYellow_rnd;
    public static float PurpuleGuy_rnd;
    public static float PurpleNoRedGuy_rnd;
    public static float OldGuy_rnd;
    public static float PenthouseGuy_rnd;
    public static float TwoHouseGuy_rnd;
    public static float ThreeHouseLGuy_rnd;
    public static float FourHouseGuy_rnd;


    GameManager gameManager;
    PlayerConnection playerConnection;
    public CharactersType Character;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerConnection = GameObject.FindGameObjectWithTag("MyConnection").GetComponent<PlayerConnection>();

        InitializingCrazyWitchTime();
    }

    private void InitializingCrazyWitchTime()
    {
        NoramlGuy_rnd = UnityEngine.Random.Range(10, 30);
        RedGuy_rnd = UnityEngine.Random.Range(20, 40);
        RedNoBlueGuy_rnd = UnityEngine.Random.Range(20, 40);
        BlueGuy_rnd = UnityEngine.Random.Range(20, 40);
        BlueNoYellow_rnd = UnityEngine.Random.Range(20, 40);
        PurpuleGuy_rnd = UnityEngine.Random.Range(20, 40);
        PurpleNoRedGuy_rnd = UnityEngine.Random.Range(20, 40);
        OldGuy_rnd = UnityEngine.Random.Range(15, 35);
        PenthouseGuy_rnd = UnityEngine.Random.Range(40, 100);
        TwoHouseGuy_rnd = UnityEngine.Random.Range(40, 90);
        ThreeHouseLGuy_rnd = UnityEngine.Random.Range(40, 90);
        FourHouseGuy_rnd = UnityEngine.Random.Range(40, 90);

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

        if (difficultyLevel == DifficultyLevel.NormalWitch)
        {
            switch (Character)
            {
                case CharactersType.NoramlGuy:
                    tempLifeTime = 50;
                    break;
                case CharactersType.RedGuy:
                    tempLifeTime = 70;
                    break;
                case CharactersType.RedNoBlueGuy:
                    tempLifeTime = 70;
                    break;
                case CharactersType.BlueGuy:
                    tempLifeTime = 70;
                    break;
                case CharactersType.BlueNoYellow:
                    tempLifeTime = 70;
                    break;
                case CharactersType.PurpuleGuy:
                    tempLifeTime = 70;
                    break;
                case CharactersType.PurpleNoRedGuy:
                    tempLifeTime = 70;
                    break;
                case CharactersType.OldGuy:
                    tempLifeTime = 60;
                    break;
                case CharactersType.PenthouseGuy:
                    tempLifeTime = 190;
                    break;
                case CharactersType.TwoHouseGuy:
                    tempLifeTime = 100;
                    break;
                case CharactersType.ThreeHouseLGuy:
                    tempLifeTime = 100;
                    break;
                case CharactersType.FourHouseGuy:
                    tempLifeTime = 190;
                    break;
                default:
                    break;
            }

        }
        else if (difficultyLevel == DifficultyLevel.EvilWitch)
        {
            switch (Character)
            {
                case CharactersType.NoramlGuy:
                    tempLifeTime = 20;
                    break;
                case CharactersType.RedGuy:
                    tempLifeTime = 30;
                    break;
                case CharactersType.RedNoBlueGuy:
                    tempLifeTime = 30;
                    break;
                case CharactersType.BlueGuy:
                    tempLifeTime = 30;
                    break;
                case CharactersType.BlueNoYellow:
                    tempLifeTime = 30;
                    break;
                case CharactersType.PurpuleGuy:
                    tempLifeTime = 30;
                    break;
                case CharactersType.PurpleNoRedGuy:
                    tempLifeTime = 30;
                    break;
                case CharactersType.OldGuy:
                    tempLifeTime = 25;
                    break;
                case CharactersType.PenthouseGuy:
                    tempLifeTime = 90;
                    break;
                case CharactersType.TwoHouseGuy:
                    tempLifeTime = 45;
                    break;
                case CharactersType.ThreeHouseLGuy:
                    tempLifeTime = 45;
                    break;
                case CharactersType.FourHouseGuy:
                    tempLifeTime = 90;
                    break;
                default:
                    break;
            }

        }
        else if (difficultyLevel == DifficultyLevel.CrazyWitch)
        {
            switch (Character)
            {
                case CharactersType.NoramlGuy:
                    tempLifeTime = NoramlGuy_rnd;
                    break;
                case CharactersType.RedGuy:
                    tempLifeTime = RedGuy_rnd;
                    break;
                case CharactersType.RedNoBlueGuy:
                    tempLifeTime = RedNoBlueGuy_rnd;
                    break;
                case CharactersType.BlueGuy:
                    tempLifeTime = BlueGuy_rnd;
                    break;
                case CharactersType.BlueNoYellow:
                    tempLifeTime = BlueNoYellow_rnd;
                    break;
                case CharactersType.PurpuleGuy:
                    tempLifeTime = PurpuleGuy_rnd;
                    break;
                case CharactersType.PurpleNoRedGuy:
                    tempLifeTime = PurpleNoRedGuy_rnd;
                    break;
                case CharactersType.OldGuy:
                    tempLifeTime = OldGuy_rnd;
                    break;
                case CharactersType.PenthouseGuy:
                    tempLifeTime = PenthouseGuy_rnd;
                    break;
                case CharactersType.TwoHouseGuy:
                    tempLifeTime = TwoHouseGuy_rnd;
                    break;
                case CharactersType.ThreeHouseLGuy:
                    tempLifeTime = ThreeHouseLGuy_rnd;
                    break;
                case CharactersType.FourHouseGuy:
                    tempLifeTime = FourHouseGuy_rnd;
                    break;
                default:
                    break;
            }

        }

        return tempLifeTime;
    }
    #endregion
}
