using DG.Tweening;
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

    public void AutoEatFood(AnimalFood eaten) {
        if (eaten != null && !eaten.isInPlayerHand && !eaten.isCollected && !eaten.isDeposited && !eaten.isFading && !eaten.IsConsumed() && FoodInterestsAnimal(eaten)) {
            Debug.Log("eating food");
            EatFood(eaten);
            Destroy(eaten);
        }
    }

    public void ForceEatFood(AnimalFood food) {
        if (food != null && !food.isFading) {
            EatFood(food);
        }
    }

    private void EatFood(AnimalFood food) {
        if (EatEvent != null) {
            EatEvent(this, new EatEventArgs(food, transform));
        }
        food.Consume();
        //food.StartFading();
    }

    public bool FoodInterestsAnimal(ItemConsumable food) {
        return _targetFoods.Contains(food.itemName);
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
