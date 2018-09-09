using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GhostHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject GhostInfoPanel;

    void Start ()
    {
        GhostInfoPanel.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        GhostInfoPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GhostInfoPanel.SetActive(false);
    }

}
