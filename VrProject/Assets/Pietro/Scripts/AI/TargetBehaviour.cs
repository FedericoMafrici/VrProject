using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class TargetBehaviour : MovementBehaviour {
    //TODO: manage destruction of the target

    private FoodEater _foodEater;
    private Transform _target;
    private Targettable _targettable;
    private const float _rotationAngleThreshold = 15f; //if angle between agent forward vector and target direction wrt to agent is lower than this value
                                                      //then the agent stops rotating towards target
                                                      //expressed in degs

    public TargetBehaviour(Transform toMoveTransform, Transform target, FoodEater fe) : base(toMoveTransform) {
        _target = target;
        _targettable = target.GetComponent<Targettable>();
        _foodEater = fe;
        ValidateParameters();
        if(HasValidParameters) {

            if (!_targettable.CanSubscribe(_npcMover))
                Debug.LogWarning(_toMoveTransform.name + " couldn't subscribe to " + _targettable.transform.name + " but subscribed nonetheless");

            _targettable.Subscribe(_npcMover);
        }
    }

    public override void Delete() {
        DeleteTarget();
    }

    public override void Move() {
        if (!_agent.isStopped) {
            UpdateMovement();
            UpdateBehaviour();
        }
    }

    private void ValidateParameters() {
        if (_target == null) {
            Debug.LogWarning("No target set for " + _toMoveTransform.name + ", TargetBehaviour not activated");
            HasValidParameters = false;
        }

        if (_targettable == null) {
            Debug.LogWarning("Target Transform reference for " + _toMoveTransform.name + " has no targettable component, TargetBehaviour not activated");
            HasValidParameters = false;
        }
    }

    private void UpdateMovement() {
        if (HasValidParameters && _target != null) {
            if (_npcMover.GetState() == MovingState.VERY_CLOSE_TO_TARGET) {
                
                _agent.destination = _toMoveTransform.position;
                Vector3 targetDirection = _target.position - _toMoveTransform.position;

                //check if agent is not facing towards the target
                //if it's not rotate it towards the target
                //otherwise destoy the target

                if (Vector3.Angle(_toMoveTransform.forward, targetDirection) > _rotationAngleThreshold) {
                    //rotate towards target

                    float rotationStep = 10.0f * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(_toMoveTransform.forward, targetDirection, rotationStep, 0.0f);
                    newDirection.y = _toMoveTransform.forward.y;
                    Debug.DrawRay(_toMoveTransform.position, newDirection, Color.red);
                    _toMoveTransform.rotation = Quaternion.LookRotation(newDirection);

                } else {
                    //if agent is looking at food then eat it
                    //_npcMover.DestroyTarget(_target);
                    if (_foodEater != null) {
                        if (_targettable != null) {
                            ItemConsumable consumable = _targettable.GetComponent<ItemConsumable>();
                            if (consumable != null) {
                                _foodEater.EatFood(consumable);
                            }
                        }
                    }
                }

            } else {
                _agent.destination = _target.position;
            }
        }
    }

    protected override bool WantsToFollowTarget(Targettable targettable) {
        //first check if base class method allows to follow target
        bool result = base.WantsToFollowTarget(targettable);

        //if base class method returns true do additional checks
        if (result)
            result = PrefersTarget(targettable.transform);

        return result;
    }

    protected override void ManageStateUpdate(MovingState nextState, Targettable newTarget) {
        // if target a target was found only call the base class method check if new target is different from current one
        // otherwise just call the base class method

        if (newTarget != null) {
            if (newTarget.transform != _target) {
                Debug.Log(_toMoveTransform.name + " found new target");
                base.ManageStateUpdate(nextState, newTarget);
            }
        } else {
            base.ManageStateUpdate(nextState, newTarget);
        }
    }

    private bool PrefersTarget(Transform target) {

        //might be used to implement a priority system, for now do not follow a target which is different from the one you're currently following
        if (_target == target) {
            return true;
        }
        return false;
    }

    private void DeleteTarget() {
        Debug.Log("Destroying " + _npcMover.name +  " TargetBehaviour");
        if (_targettable != null) {
            _targettable.Unsubscribe(_npcMover);
            _targettable = null;
        }

        if (_target != null) {
            _target = null;
        }
    }
}
