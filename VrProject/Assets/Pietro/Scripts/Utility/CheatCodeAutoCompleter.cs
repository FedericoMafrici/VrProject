using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class CheatCodeAutoCompleter : MonoBehaviour {
    [SerializeField] private Quest _startingTutorial;
    [SerializeField] private List<Transform> _farmQuestCollections;
    [SerializeField] private List<Transform> _savannahQuestCollections;
    [SerializeField] private List<Transform> _achievementsQuestCollections;
    [SerializeField] private List<KeyCode> _completeTutorialInputs = new List<KeyCode> {KeyCode.T, KeyCode.U, KeyCode.T, KeyCode.O, KeyCode.R, KeyCode.I, KeyCode.A, KeyCode.L};
    [SerializeField] private List<KeyCode> _completeSavannahInputs = new List<KeyCode> { KeyCode.S, KeyCode.A, KeyCode.V, KeyCode.A, KeyCode.N, KeyCode.A};
    [SerializeField] private List<KeyCode> _completeFarmInputs = new List<KeyCode> { KeyCode.F, KeyCode.A, KeyCode.R, KeyCode.M };
    [SerializeField] private List<KeyCode> _completeAchievementInputs = new List<KeyCode> { KeyCode.P, KeyCode.L, KeyCode.A, KeyCode.T, KeyCode.I, KeyCode.N, KeyCode.O};
    [SerializeField] private List<KeyCode> _jumpInputs = new List<KeyCode> { KeyCode.J, KeyCode.U, KeyCode.M, KeyCode.P};

    private HashSet<Quest> _farmQuests = new HashSet<Quest>();
    private HashSet<Quest> _savannahQuests = new HashSet<Quest>();
    private HashSet<Quest> _achievementsQuests = new HashSet<Quest>();
    private int _currentInputIdx = 0;
    private bool _tutorialCompleted = false;
    private bool _farmCompleted = false;
    private bool _achievementsCompleted = false;
    private bool _savannahCompleted = false;

    private void Awake() {
        foreach (Transform collection in _farmQuestCollections) {
            GetQuestsFromParentTransform(collection, _farmQuests);
        }

        foreach (Transform collection in _savannahQuestCollections) {
            GetQuestsFromParentTransform(collection, _savannahQuests);
        }

        foreach (Transform collection in _achievementsQuestCollections) {
            GetQuestsFromParentTransform(collection, _achievementsQuests);
        }
    }

    void Update() {
        if (Input.anyKeyDown) {
            int index = _currentInputIdx;
            int tutorialResult = -1;
            int farmResult = -1;
            int savannahResult = -1;
            int achievementResult = -1;
            int jumpResult = -1;
            bool somethingTriggered = false;

            if (!_tutorialCompleted && !somethingTriggered) {
                tutorialResult = CheckCheatCodeSequence(_completeTutorialInputs, index);
                if (tutorialResult == -2) {
                    _tutorialCompleted = true;
                    somethingTriggered = true;
                    _currentInputIdx = 0;
                    if (_startingTutorial.GetState() != QuestState.COMPLETED) {
                        _startingTutorial.AutoComplete();
                    }
                }
            }
            if (!_farmCompleted && !somethingTriggered) {
                farmResult = CheckCheatCodeSequence(_completeFarmInputs, index);
                if (farmResult == -2) {
                    _farmCompleted = true;
                    somethingTriggered = true;
                    _currentInputIdx = 0;

                    foreach (Quest q in _farmQuests) {
                        if (q.GetState() != QuestState.COMPLETED) { 
                        q.AutoComplete();
                            }
                    }

                }
            }

            if (!_savannahCompleted && !somethingTriggered) {
                savannahResult = CheckCheatCodeSequence(_completeSavannahInputs, index);
                if (savannahResult == -2) {
                    _savannahCompleted = true;
                    somethingTriggered = true;
                    _currentInputIdx = 0;

                    foreach(Quest q in _savannahQuests) {
                        if (q.GetState() != QuestState.COMPLETED) {
                            q.AutoComplete();
                        }
                    }

                }
            }

            if (!_achievementsCompleted && !somethingTriggered) {
                achievementResult = CheckCheatCodeSequence(_completeAchievementInputs, index);
                if (achievementResult == -2) {
                    _achievementsCompleted = true;
                    somethingTriggered = true;
                    _currentInputIdx = 0;

                    foreach (Quest q in _achievementsQuests) {
                        if (q.GetState() != QuestState.COMPLETED) {
                            q.AutoComplete();
                        }
                    }

                }
            }


            if (!somethingTriggered) {
                jumpResult = CheckCheatCodeSequence(_jumpInputs, index);
                if (jumpResult == -2) {
                    somethingTriggered = true;
                    _currentInputIdx = 0;

                    if (InputManager.JumpIsEnabled()) {
                        InputManager.DisableJump();
                    } else {
                        InputManager.EnableJump();
                    }

                }
            }
            

            if (!somethingTriggered) {
                /*
                int maxIndex = (farmResult > tutorialResult) ? farmResult : tutorialResult;
                maxIndex = (maxIndex > savannahResult) ? maxIndex : savannahResult;
                maxIndex = (maxIndex > jumpResult) ? maxIndex : jumpResult;
                */

                int maxIndex = Mathf.Max(farmResult, tutorialResult, savannahResult, jumpResult, achievementResult);
                if (maxIndex <= -1) {
                    _currentInputIdx= 0;
                } else {
                    _currentInputIdx = maxIndex;
                }
            }

        }
    }

    private int CheckCheatCodeSequence(List<KeyCode> inputList, int index) {
        if ((inputList.Count > 0) && (index < inputList.Count) && Input.GetKeyDown(inputList[index])) { 
            if (index == inputList.Count - 1) {
                return -2;
            } else {
                return ++index;
            }
        } else {
            return -1;
        }
    }

    protected void GetQuestsFromParentTransform(Transform collection, HashSet<Quest> _questSet) {
        //HashSet<Quest> quests = new HashSet<Quest>();
        int nChildren = collection.childCount;

        for (int i = 0; i < nChildren; i++) {
            Transform child = collection.GetChild(i);
            Quest[] toAddArray = child.GetComponents<Quest>();
            foreach (Quest q in toAddArray) {
                _questSet.Add(q);
            }
        }

    }
}
