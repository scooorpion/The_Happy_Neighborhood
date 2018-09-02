using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathImage : MonoBehaviour
{
    public Image DeathCharPlace;

    public void PlaceDeathCharInPic(Sprite DeadCharSprite)
    {
        DeathCharPlace.sprite = DeadCharSprite;
    }
}
