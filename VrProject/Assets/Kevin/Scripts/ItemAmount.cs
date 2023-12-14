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
        if (hotbar.items[slot])
        {
            int amount = hotbar.items[slot].amount;
            if (amount == 0 || amount == 1)
            {
                gameObject.GetComponent<TMP_Text>().text = "";
            }
            else
            {
                gameObject.GetComponent<TMP_Text>().text = amount.ToString();
            }
        }
    }
}
