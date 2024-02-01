using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDepositCounter : MonoBehaviour
{
    public PlayerPickUpDrop player;
    public int counter = 0;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        transform.Rotate(0, 180, 0);
        if (counter > 0)
        {
            GetComponent<TMP_Text>().text = counter.ToString();
        }
        else
        {
            GetComponent<TMP_Text>().text = "";
        }
    }
}
