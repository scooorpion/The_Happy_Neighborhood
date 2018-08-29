using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCutScene : MonoBehaviour {

    SplashScreenManager splashScreenManager;
    public float SkipCutSceneSpeed = 1;

    void Start()
    {
        splashScreenManager = FindObjectOfType<SplashScreenManager>();
    }


    void Update()
    {
        if(isActiveAndEnabled)
        {
            splashScreenManager.SkipCutSceneVideo(SkipCutSceneSpeed);
        }
    }
}
