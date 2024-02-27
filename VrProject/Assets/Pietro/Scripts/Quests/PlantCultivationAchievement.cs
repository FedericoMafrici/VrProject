using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class PlantCultivationAchievement : Quest
{
    [SerializeField] private string _description;
    [SerializeField] private List<FarmingLand> _landsList;
    [SerializeField] private List<CropType> _cropTypeList;
    [SerializeField] List<int> _toReachValues = new List<int>();
    [SerializeField] private HashSet<FarmingLand> _lands = new HashSet<FarmingLand>();
    private int _valuesIndex = 0;
    private int _nGrown;

    protected override void Init() {
        _lands = _landsList.ToHashSet();

        //subscribe to events of lands
        foreach (FarmingLand land in _lands) {
            land.CropPlanted += OnCropPlanted;
            if (land.crop != null) {
                OnCropPlanted(land, land.crop, land.crop.isTree);
            }
        }

        _autoStart = true;

        base.Init();
    }

    private void OnCropPlanted(FarmingLand farmingLand, CropBehaviour crop, bool isTree) {
        if (_cropTypeList.Contains(crop.cropType)) { //if crop is of required type 
            crop.GrowthEvent += OnCropGrowth;
            crop.CropDestroyed += OnCropDestroyed;
        }
    }

    private void OnCropGrowth(CropBehaviour crop, CropBehaviour.CropState state) {
        if (_state == QuestState.ACTIVE) {
            if (state == CropBehaviour.CropState.Harvestable) {
                crop.GrowthEvent -= OnCropGrowth;
                crop.CropDestroyed -= OnCropDestroyed;

                AdvanceCounter();
            } 

        } else {
            crop.GrowthEvent -= OnCropGrowth;
            crop.CropDestroyed -= OnCropDestroyed;
        }
    }

    private void AdvanceCounter() {
        _nGrown++;
        Debug.LogWarning("<color=cyan>" + this + ": progress for achievement, id: " + GetID() + "</color>");

        if (_nGrown >= _toReachValues[_valuesIndex]) {
            string color = _valuesIndex == 0 ? "green" : (_valuesIndex == 1 ? "yellow" : "red");

            Debug.LogWarning("<color=" + color + ">" + this + ": achievement unlocked" + " tier : " + _valuesIndex + 1 + ", id: " + GetID() + "</color>");
            Progress();

            if (_valuesIndex < _toReachValues.Count - 1) {
                _valuesIndex++;
            } else {
                Complete();
                foreach (FarmingLand land in _lands) {
                    land.CropPlanted -= OnCropPlanted;
                }
            }
        }
    }

    private void OnCropDestroyed(CropBehaviour crop) {
        crop.GrowthEvent -= OnCropGrowth;
        crop.CropDestroyed -= OnCropDestroyed;
    }

    public override string GetQuestDescription() {
        return _description + " " + _nGrown + "/" + _toReachValues[_valuesIndex];
    }

    public override bool AutoComplete() {
        AutoCompletePreCheck();

        while (_state != QuestState.COMPLETED) {
            AdvanceCounter();
        }

        AutoCompletePostCheck();

        return true;
    }
}
