using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPetterActivator : QuestEventReceiver {
    Petter _petter;
    [SerializeField] bool _startDisabled = true;

    bool _alreadyEnabled = false;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        _petter = GetComponent<Petter>();
        if (_petter == null) {
            Debug.LogError(transform.name + " no Petter component found");
        }

        if (_startDisabled) {
            _petter.enabled = false;
        }
    }

    protected override void OnEventReceived(Quest quest, EventType eventType) {
        if (!_alreadyEnabled) {
            _petter.enabled = true;
        }

        SetEventSubscription(false, quest, eventType);
    }

}
