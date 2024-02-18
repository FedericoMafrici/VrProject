using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviourModifier : QuestEventReceiver {
    [SerializeField] private NPCMover _npcMover;
    [SerializeField] private BehaviourID _behaviourID;
    [SerializeField] private BoxCollider _patrolArea;
    [SerializeField] private bool changeSpeed = true;
    [SerializeField] private float _newSpeed = 1.0f;

    private bool _alreadyChanged = false;

    protected override void Awake() {
        if (_npcMover == null) {
            Debug.LogError(transform.name + " Behaviour Modifier, no NPC Mover reference set");
        }

        if (_behaviourID == BehaviourID.PATROL && _patrolArea == null) {
            Debug.LogError(transform.name + " Behaviour Modifier, no Patrol Area to assign to NPC");
        }

        base.Awake();
    }
    protected override void OnEventReceived(Quest quest, EventType eventType) {
        if (!_alreadyChanged) {

            if (_behaviourID == BehaviourID.PATROL) {
                Debug.LogWarning("Setting " + _npcMover + " patrol area to " + _patrolArea);
                _npcMover.SetPatrolArea(_patrolArea);
            } else {
                _npcMover.SetBehaviour(_behaviourID);
            }
            if (changeSpeed && _newSpeed > 0.0f) {
                _npcMover.SetAgentMovingSpeed(_newSpeed);
            }

            _alreadyChanged = true;
        }

        SetEventSubscription(false, quest, eventType);
    }
}
