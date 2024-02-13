using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LionQuestBehaviour : MonoBehaviour {
    [SerializeField] private FarmingLand _farmingLand;
    private CropBehaviour _crop;
    [SerializeField] private NPCMover _npcMover;
    private bool _isDistracted = false;

    public event Action LionDistractedEvent;

    private void Awake() {
        if (_farmingLand == null) {
            Debug.LogError(transform.name + ": no Farming Land set");
        }

        if (_npcMover == null) {
            Debug.LogError(transform.name + ": no NPC Mover set");
        }

        if (_farmingLand.crop == null && !_isDistracted) {
            _farmingLand.CropPlanted += OnCropPlanted;
        }
    }

    void OnCropPlanted(FarmingLand land, CropBehaviour plantedCrop, bool isTree) {
        if (plantedCrop.cropType == CropType.ACACIA) {
            _farmingLand.CropPlanted -= OnCropPlanted;
            _crop = plantedCrop;
            _crop.GrowthEvent += OnCropGrowth;
            _crop.CropDestroyed += OnCropDestroyed;
        }
    }

    void DistractLion() {
        _crop.GrowthEvent -= OnCropGrowth;
        _crop.CropDestroyed -= OnCropDestroyed;
        _isDistracted = true;
        if (LionDistractedEvent != null) {
            LionDistractedEvent();
        }
    }

    void OnCropGrowth(CropBehaviour crop, CropBehaviour.CropState newState) {
        if (newState == CropBehaviour.CropState.Harvestable) {
            DistractLion();
        }
    }

    void OnCropDestroyed(CropBehaviour crop) {
        if (_crop != null) {
            _crop.GrowthEvent -= OnCropGrowth;
            _crop.CropDestroyed -= OnCropDestroyed;
        }
        _farmingLand.CropPlanted += OnCropPlanted;
    }

    public bool IsDistracted() {
        return _isDistracted;
    }

    private void CheckCrop() {
        if (_farmingLand.crop != null) {
            OnCropPlanted(_farmingLand, _farmingLand.crop, _farmingLand.tree);
            if (_farmingLand.tree) {
                OnCropGrowth(_crop, _crop.cropState);
            }
        }
    }

    private void OnEnable() {
        CheckCrop();
    }
}
