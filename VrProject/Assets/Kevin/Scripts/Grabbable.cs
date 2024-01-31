using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private bool isGrabbedFromGround = true;
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;
    [HideInInspector] public bool isInPlayerHand = false;

    public event Action GrabEvent;
    public event Action ReleaseEvent;

    private void Awake()
    {
        if (objectRigidBody == null)
            objectRigidBody = GetComponent<Rigidbody>();
    }

    public void Grab(Transform objectGrabPointTransform, bool isGrabbedFromGround)
    {
        if (objectRigidBody == null) {
            objectRigidBody = GetComponent<Rigidbody>();
        }

        isInPlayerHand = true;
        objectRigidBody.useGravity = false;
        objectRigidBody.isKinematic = true;
        this.isGrabbedFromGround = isGrabbedFromGround;
        this.objectGrabPointTransform = objectGrabPointTransform;

        if (GrabEvent != null) {
            GrabEvent();
        }
    }

    public void Drop()
    {
        isInPlayerHand = false;
        this.objectGrabPointTransform = null;
        objectRigidBody.useGravity = true;
        objectRigidBody.isKinematic = false;

        if (ReleaseEvent!= null) {
            ReleaseEvent();
        }
    }

    public void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            if (isGrabbedFromGround)
            {
                float lerpSpeed = 10;
                Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
                objectRigidBody.MovePosition(newPosition);
            }
            else
            {
                objectRigidBody.MovePosition(objectGrabPointTransform.position);
            }
        }
    }
}
