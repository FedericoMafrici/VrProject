using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEater : MonoBehaviour {
    [SerializeField] private List<Item.ItemName> _targetFoodsList;
    private HashSet<Item.ItemName> _targetFoods = new HashSet<Item.ItemName>();

    public event EventHandler<EatEventArgs> EatEvent;

    private void Start() {
        if (_targetFoodsList != null) {
            foreach (Item.ItemName name in _targetFoodsList) {
                _targetFoods.Add(name);
            }
        }
    }

    public void EatFood(ItemConsumable eaten) {
        if (eaten != null && !eaten.isCollected && !eaten.isDeposited && !eaten.isFading) {
            if (EatEvent != null) {
                EatEvent(this, new EatEventArgs(eaten, transform));
            }

            eaten.Consume();
            eaten.StartFading();
        }
    }
}

public class EatEventArgs : EventArgs {
    public ItemConsumable eaten;
    public Transform animal;

    public EatEventArgs(ItemConsumable eaten, Transform animal) {
        this.eaten = eaten;
        this.animal = animal;
    }
}
