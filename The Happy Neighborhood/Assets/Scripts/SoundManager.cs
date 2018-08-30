using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource MainSoundTrackAudioSource;
    public AudioSource SFXAudioSource;
    public AudioClip Error;
    public AudioClip WrongACtion;
    public AudioClip CharactersPlacement;
    public AudioClip EndOfGame;
    public AudioClip HousePlacement;
    public AudioClip MenuButtonSFX;
    public AudioClip OldHousePlacement;
    public AudioClip SelectingCard;
    public AudioClip GhostFly;

    public void SoundTrackPlay()
    {
        MainSoundTrackAudioSource.Play();
    }

    public void SFX_WrongACtionPlay()
    {
        SFXAudioSource.clip = WrongACtion;
        SFXAudioSource.Play();
    }

    public void SFX_ErrorPlay()
    {
        SFXAudioSource.clip = Error;
        SFXAudioSource.Play();
    }


    public void SFX_CharactersPlacementPlay()
    {
        SFXAudioSource.clip = CharactersPlacement;
        SFXAudioSource.Play();
    }
    public void SFX_EndOfGamePlay()
    {
        SFXAudioSource.clip = EndOfGame;
        SFXAudioSource.Play();
    }

    public void SFX_GhostFlyPlay()
    {
        SFXAudioSource.clip = GhostFly;
        SFXAudioSource.Play();
    }

    public void SFX_HousePlacementPlay()
    {
        SFXAudioSource.clip = HousePlacement;
        SFXAudioSource.Play();
    }
    public void SFX_MenuButtonPlay()
    {
        SFXAudioSource.clip = MenuButtonSFX;
        SFXAudioSource.Play();
    }
    public void SFX_OldHousePlacementPlay()
    {
        SFXAudioSource.clip = OldHousePlacement;
        SFXAudioSource.Play();
    }
    public void SFX_SelectingCardPlay()
    {
        SFXAudioSource.clip = SelectingCard;
        SFXAudioSource.Play();
    }

}
