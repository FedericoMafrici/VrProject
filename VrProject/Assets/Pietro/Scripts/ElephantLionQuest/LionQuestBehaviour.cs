using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LionQuestBehaviour : MonoBehaviour {
    [SerializeField] private FarmingLand _farmingLand;
    [SerializeField] private CropBehaviour _crop;
    [SerializeField] private BoxCollider _finalPatrolArea;
    [SerializeField] private NPCMover _npcMover;
    private bool _isDistracted = false;

    public event Action LionDistractedEvent;

    private void Start() {
        if (_farmingLand == null) {
            Debug.LogError(transform.name + ": no Farming Land set");
        }

        if (_finalPatrolArea == null) {
            Debug.LogError(transform.name + ": no Final Patrol Area set");
        }

        if (_npcMover == null) {
            Debug.LogError(transform.name + ": no NPC Mover set");
        }

        _farmingLand.CropPlanted += OnCropPlanted;
    }

    void OnCropPlanted(CropBehaviour plantedCrop, bool isTree) {
        if (isTree) {
            _farmingLand.CropPlanted -= OnCropPlanted;
            _crop = plantedCrop;
            _crop.GrowthEvent += OnCropGrowth;
            _crop.CropDestroyed += OnCropDestroyed;
        }
    }

    void OnCropGrowth(CropBehaviour.CropState newState) {
        if (newState == CropBehaviour.CropState.Harvestable) {
            _npcMover.SetPatrolArea(_finalPatrolArea);
            _crop.GrowthEvent -= OnCropGrowth;
            _crop.CropDestroyed -= OnCropDestroyed;
            _isDistracted = true;
            if (LionDistractedEvent != null) {
                LionDistractedEvent();
            }
        }
    }

    void OnCropDestroyed() {
        if (_crop != null) {
            _crop.GrowthEvent -= OnCropGrowth;
            _crop.CropDestroyed -= OnCropDestroyed;
        }
        _farmingLand.CropPlanted += OnCropPlanted;
    }

    public bool IsDistracted() {
        return _isDistracted;
    }
}
