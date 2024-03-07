using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI.Table;

public class GrowPlantQuest : Quest
{
    [SerializeField] private string _description;
    [SerializeField] private List<FarmingLand> _landsList; 
    [SerializeField] private int _nToGrow;
    [SerializeField] private CropType _targetCropType;


    private HashSet<FarmingLand> _lands;
    private int _nGrown;

    protected override void Init() {
        if (_landsList == null) {
            Debug.LogError(transform.name + " GrowPlantQuest: land list is null");
        } else if (_landsList.Count == 0) {
            Debug.LogWarning(transform.name + " GrowPlantQuest: land list is empty");
        }

        _lands = new HashSet<FarmingLand>();
        foreach(FarmingLand land in _landsList) {
            _lands.Add(land);
        }

        base.Init();
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();

        //subscribe to events of lands
        foreach (FarmingLand land in _lands) {
            land.CropPlanted += OnCropPlanted;
            if (land.crop != null) {
                OnCropPlanted(land, land.crop, land.crop.isTree);
            }
        }

    }

    private void OnCropPlanted(FarmingLand farmingLand, CropBehaviour crop, bool isTree) {
        if (crop.cropType == _targetCropType) { //if crop is of required type 
            Debug.LogWarning(transform.name + " quest detected that a crop was planted");
            crop.GrowthEvent += OnCropGrowth;
            crop.CropDestroyed += OnCropDestroyed;
        }
    }

    private void OnCropGrowth(CropBehaviour crop, CropBehaviour.CropState state) {
        if (_state == QuestState.ACTIVE) {
            if (state == CropBehaviour.CropState.Harvestable) {
                crop.GrowthEvent -= OnCropGrowth;
                _nGrown++;
                Progress();
                if (_nGrown >= _nToGrow) {
                    _nGrown = _nToGrow;
                    Complete();
                    foreach (FarmingLand land in _lands) {
                        land.CropPlanted -= OnCropPlanted;
                    }
                }
            }
        } else {
            crop.GrowthEvent -= OnCropGrowth;
            crop.CropDestroyed -= OnCropDestroyed;
        }
    }

    private void OnCropDestroyed(CropBehaviour crop) {
        crop.GrowthEvent -= OnCropGrowth;
        crop.CropDestroyed -= OnCropDestroyed;
    }

    public override string GetQuestDescription() {
        return _description + " " + _nGrown + "/" + _nToGrow;
    }

    public override bool AutoComplete() {
        ForceStart();
        AutoCompletePreCheck();

        while (_nGrown < _nToGrow) {
            _nGrown++;
            Progress();
        }
        Complete();
        foreach (FarmingLand land in _lands) {
            land.CropPlanted -= OnCropPlanted;
        }
        AutoCompletePostCheck();
        return true;
    }
}
