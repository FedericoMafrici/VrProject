using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemAmount : MonoBehaviour
{
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private int slot;

    // Update is called once per frame
    void Update()
    {
        if (hotbar.itemWrappers != null && hotbar.itemWrappers[slot] != null)
        {
            int amount = hotbar.itemWrappers[slot].amount;
            if (amount == 0 || amount == 1)
            {
                gameObject.GetComponent<TMP_Text>().text = "";
            }
            else
            {
                gameObject.GetComponent<TMP_Text>().text = amount.ToString();
            }
        }
        else
        {
            gameObject.GetComponent<TMP_Text>().text = "";
        }
    }
}
