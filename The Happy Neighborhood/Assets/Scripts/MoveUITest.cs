using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveUITest : MonoBehaviour {

    public RectTransform Item;
    public RectTransform DestinTransform;
    public float DistanceToMatch = 10;
    public float MoveSpeed = 2;

    bool ToMove = false;
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToMove = true;
        }

        if(ToMove)
        {
            //UIItem.transform.Translate(DestinTransform);
            Item.anchoredPosition = Vector2.Lerp(Item.anchoredPosition, DestinTransform.anchoredPosition, Time.deltaTime * MoveSpeed);
            if(Vector2.Distance(Item.anchoredPosition, DestinTransform.anchoredPosition) < DistanceToMatch)
            {
                Item.anchoredPosition = DestinTransform.anchoredPosition;
                ToMove = false;
                print("Done");
            }
        }
	}
}
