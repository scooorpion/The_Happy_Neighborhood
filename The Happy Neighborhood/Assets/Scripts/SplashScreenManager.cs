using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SplashScreenManager : MonoBehaviour
{

    enum SplashScreen { GameLogo, TeamLogo, Cutscene, ForeGround }
    enum ScreenState { Disappeared, ToAppear, ToDisapear, Appeared }

    public Image TeamLogo;
    public Image GameLogo;
    public Image CutScenePanel;
    public Image ForeGround;
    public Image Background;

    public AudioSource CutsceneMusic;
    public AudioSource TeamLogo_SFX;
    public AudioSource GameLogo_SFX;
    public AudioSource CutsceneFrame_SFX;
    public float SkipCutSceneSpeed = 1;


    ScreenState TeamLogo_State;
    ScreenState Cutscene_State;
    ScreenState GameLogo_State;

    [Header("Wait Time Between Splash Screens: ")]
    [Range(0.1f, 4)]
    public float WaitTime_TeamLogo = 1.2f;
    [Range(0.1f, 4)]
    public float WaitTime_GameLogo = 1.2f;
    [Range(0.1f, 4)]
    public float WaitTime_Cutscene = 1.2f;


    [Header("Team Logo Time Properties: ")]

    [Range(0.1f, 4)]
    public float FadeInSpeed_TeamLogo = 1.2f;
    [Range(0.1f, 4)]
    public float ShowTime_TeamLogo = 1.2f;
    [Range(0.1f, 4)]
    public float FadeOutSpeed_TeamLogo = 1.2f;


    [Header("Game Logo Time Properties: ")]

    [Range(0.1f, 4)]
    public float FadeInSpeed_GameLogo = 1.2f;
    [Range(0.1f, 4)]
    public float ShowTime_GameLogo = 1.2f;
    [Range(0.1f, 4)]
    public float FadeOutSpeed_GameLogo = 1.2f;

    [Header("Frame Time Properties: ")]

    [Range(0.1f, 4)]
    public float FadeInSpeed_Frame = 1.2f;
    [Range(0.1f, 4)]
    public float ShowTime_Frame = 1.2f;
    [Range(0.1f, 4)]
    public float FadeOutSpeed_Frame = 1.2f;

    float TimeCounter;
    float WaitTimeCounter;

    private Animator anim;


    void Start()
    {
        ChangeAlpha(SplashScreen.TeamLogo, 0);
        ChangeAlpha(SplashScreen.GameLogo, 0);
        ChangeAlpha(SplashScreen.Cutscene, 0);
        ChangeAlpha(SplashScreen.Cutscene, 0);

        TeamLogo_State = ScreenState.ToAppear;
        GameLogo_State = ScreenState.Disappeared;
        Cutscene_State = ScreenState.Disappeared;

        anim = GetComponent<Animator>();
    }


    void Update()
    {
        #region When TeamLogo is going to appear
        if (TeamLogo_State == ScreenState.ToAppear)
        {
            WaitTimeCounter += Time.deltaTime;
            if (WaitTimeCounter > WaitTime_TeamLogo)
            {
                TimeCounter += Time.deltaTime;

                ChangeAlpha(SplashScreen.TeamLogo, TimeCounter * FadeInSpeed_TeamLogo);

                if (TeamLogo.color.a > 0.9f)
                {
                    ChangeAlpha(SplashScreen.TeamLogo, 1);
                    TimeCounter = 0;
                    WaitTimeCounter = 0;
                    TeamLogo_SFX.Play();
                    TeamLogo_State = ScreenState.Appeared;
                }
            }
        }
        #endregion

        #region When TeamLogo is appeared
        else if (TeamLogo_State == ScreenState.Appeared)
        {
            TimeCounter += Time.deltaTime;
            if (TimeCounter > ShowTime_TeamLogo)
            {
                TimeCounter = 1;
                TeamLogo_State = ScreenState.ToDisapear;
            }
        }
        #endregion

        #region When TeamLogo is going to disappear
        else if (TeamLogo_State == ScreenState.ToDisapear)
        {
            TimeCounter -= Time.deltaTime;

            ChangeAlpha(SplashScreen.TeamLogo, TimeCounter * FadeOutSpeed_TeamLogo);

            if (TeamLogo.color.a < 0.1f)
            {
                ChangeAlpha(SplashScreen.TeamLogo, 0);
                TimeCounter = 0;
                TeamLogo_State = ScreenState.Disappeared;
                GameLogo_State = ScreenState.ToAppear;
            }

        }
        #endregion

        #region When GameLogo is going to appear
        else if (GameLogo_State == ScreenState.ToAppear)
        {
            WaitTimeCounter += Time.deltaTime;
            if (WaitTimeCounter > WaitTime_GameLogo)
            {
                TimeCounter += Time.deltaTime;

                ChangeAlpha(SplashScreen.GameLogo, TimeCounter * FadeInSpeed_GameLogo);

                if (GameLogo.color.a > 0.9f)
                {
                    ChangeAlpha(SplashScreen.GameLogo, 1);
                    TimeCounter = 0;
                    WaitTimeCounter = 0;
                    GameLogo_SFX.Play();
                    GameLogo_State = ScreenState.Appeared;
                }
            }

        }
        #endregion

        #region When GameLogo is appeared
        else if (GameLogo_State == ScreenState.Appeared)
        {
            TimeCounter += Time.deltaTime;
            if (TimeCounter > ShowTime_GameLogo)
            {
                Background.color = new Color(0.99f,0.98f,0.47f,1);

                TimeCounter = 1;
                GameLogo_State = ScreenState.ToDisapear;
            }

        }
        #endregion

        #region When GameLogo is going to disappear
        else if (GameLogo_State == ScreenState.ToDisapear)
        {
            TimeCounter -= Time.deltaTime;

            ChangeAlpha(SplashScreen.GameLogo, TimeCounter * FadeOutSpeed_GameLogo);

            if (GameLogo.color.a < 0.1f)
            {
                ChangeAlpha(SplashScreen.GameLogo, 0);
                TimeCounter = 0;
                GameLogo_State = ScreenState.Disappeared;
                Cutscene_State = ScreenState.ToAppear;
            }
        }
        #endregion

        #region When Cutscene is going to appear
        else if (Cutscene_State == ScreenState.ToAppear)
        {
            WaitTimeCounter += Time.deltaTime;

            if (WaitTimeCounter > WaitTime_Cutscene)
            {
                WaitTimeCounter = 0;
                TimeCounter = 0;

                ChangeAlpha(SplashScreen.Cutscene, 1);
                CutsceneMusic.Play();
                Cutscene_State = ScreenState.Appeared;
            }
        }
        #endregion

        #region When Cutscene is appeared
        else if (Cutscene_State == ScreenState.Appeared)
        {
            anim.SetBool("CutscenePlay", true);
            Cutscene_State = ScreenState.ToDisapear;
        }
        #endregion
        #region When Cutscene is to disappear
        else if (Cutscene_State == ScreenState.ToDisapear)
        {

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            {
                Cutscene_State = ScreenState.Disappeared;
            }
        }
        #endregion
        #region When Cutscene is disappeared
        else if (Cutscene_State == ScreenState.Disappeared)
        {
            SkipCutSceneVideo(SkipCutSceneSpeed);
        }
        #endregion
    }

    #region SkipCutSceneVideo(float Speed)
    public void SkipCutSceneVideo(float Speed)
    {
        anim.enabled = false;
        CutsceneMusic.volume = Mathf.Lerp(CutsceneMusic.volume, 0, Time.deltaTime * Speed);

        Color tempColor = ForeGround.color;
        tempColor.a = Mathf.Lerp(tempColor.a, 1, Time.deltaTime * Speed);
        ForeGround.color = tempColor;

        if (CutsceneMusic.volume < 0.2f)
        {
            CutsceneMusic.volume = 0;

            tempColor = ForeGround.color;
            tempColor.a = 1;
            ForeGround.color = tempColor;

            StartCoroutine(LoadScene(1f));
        }
    }
    #endregion

    IEnumerator LoadScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(1);
    }


    #region ChangeAlpha(SplashScreen Screen, float Alpha)
    private void ChangeAlpha(SplashScreen Screen, float Alpha)
    {
        if (Screen == SplashScreen.GameLogo)
        {
            Color tempColor = GameLogo.color;
            tempColor.a = Alpha;
            GameLogo.color = tempColor;
        }
        else if (Screen == SplashScreen.TeamLogo)
        {
            Color tempColor = TeamLogo.color;
            tempColor.a = Alpha;
            TeamLogo.color = tempColor;
        }
        else if (Screen == SplashScreen.Cutscene)
        {
            Color tempColor = CutScenePanel.color;
            tempColor.a = Alpha;
            CutScenePanel.color = tempColor;
        }
        else if (Screen == SplashScreen.ForeGround)
        {
            Color tempColor = ForeGround.color;
            tempColor.a = Alpha;
            ForeGround.color = tempColor;
        }

    }
    #endregion

    public void SkipButton()
    {
        Cutscene_State = ScreenState.Disappeared;
    }
}
