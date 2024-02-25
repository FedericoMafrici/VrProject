using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FoodEater : MonoBehaviour {
    [SerializeField] private List<Item.ItemName> _targetFoodsList;
    //[SerializeField] private GameObject _foodIcon;
    [SerializeField] EmissionData hungryEmission;
    [SerializeField] ObjectEmitter emitter;
    private HashSet<Item.ItemName> _targetFoods = new HashSet<Item.ItemName>();
    private bool _isHungry = true;


    public event EventHandler<EatEventArgs> EatEvent;
    public static event EventHandler<EatEventArgs> StaticEatEvent;

    private void Start() {
        if (_targetFoodsList != null) {
            foreach (Item.ItemName name in _targetFoodsList) {
                _targetFoods.Add(name);
            }
        }
        if (emitter != null) {
            emitter.AddEmission(hungryEmission);
        }
    }

    public void AutoEatFood(AnimalFood eaten) {
        if (eaten != null && FoodInterestsAnimal(eaten) && !eaten.isInPlayerHand && !eaten.isCollected && !eaten.isDeposited && !eaten.isFading && !eaten.IsConsumed()) {
            UnityEngine.Debug.Log("eating food");
            EatFood(eaten);
            Destroy(eaten.gameObject);
        } else {
            /*
            UnityEngine.Debug.LogWarning("Not eating");
            UnityEngine.Debug.Log("In player hand: " + eaten.isInPlayerHand);
            UnityEngine.Debug.Log("Is collected: " + eaten.isCollected);
            UnityEngine.Debug.Log("Is fading: " + eaten.isFading);
            UnityEngine.Debug.Log("Is deposited: " + eaten.isDeposited);
            UnityEngine.Debug.Log("Is consumed: " + eaten.IsConsumed());
            UnityEngine.Debug.Log("Interests animal: " + FoodInterestsAnimal(eaten));
            */
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
        if (StaticEatEvent != null) {
            StaticEatEvent(this, new EatEventArgs(food, transform));
        }

        food.Consume();
        _isHungry = false;
        /*
        if (_foodIcon != null) {
            _foodIcon.gameObject.SetActive(true);
        }
        */
        if (emitter!= null) {
            emitter.RemoveEmission(hungryEmission);
        }
        AnimalAudioPlayer audioSource = GetComponent<AnimalAudioPlayer>();
        if (audioSource != null && food.usageSound != null) {
            audioSource.PlaySound(food.usageSound);
        }
        StartCoroutine(WaitBeforeHungry());
        //food.StartFading();
    }

    public bool FoodInterestsAnimal(AnimalFood food) {
        return _isHungry && _targetFoods.Contains(food.itemName) && !food.IsPlanted();
    }

    IEnumerator WaitBeforeHungry() {
        yield return new WaitForSeconds(30);
        _isHungry = true;
        /*
        if (_foodIcon != null) {
            _foodIcon.gameObject.SetActive(false);
        }
        */
        if (emitter != null) {
            emitter.AddEmission(hungryEmission);
        }
    }

    private void OnEnable() {
        _isHungry = true;
        /*
        if (_foodIcon != null) {
            _foodIcon.gameObject.SetActive(false);
        }
        */
        
        if (emitter != null) {
            emitter.AddEmission(hungryEmission);
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
