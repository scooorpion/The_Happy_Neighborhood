using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaySFX : MonoBehaviour
{
    public AudioSource CutSceneSFX;

    private void OnEnable()
    {
        CutSceneSFX.Play();
    }
}
