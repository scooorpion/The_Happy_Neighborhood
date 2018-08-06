using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CellSelecting : MonoBehaviour {

    PlayerConnection myConnection;


    private void Start()
    {
        myConnection = GameObject.FindGameObjectWithTag("MyConnection").GetComponent<PlayerConnection>();
    }


    public void SelectCell()
    {
        if (myConnection.CardTypeSelected == CardType.NoSelection)
            return;

        myConnection.CommandToCheckSelectedCell(GetComponent<CellIndex>().cellIndex);
    }
}
