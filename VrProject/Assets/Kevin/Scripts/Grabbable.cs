using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Grabbable : MonoBehaviour
{
    private bool isGrabbedFromGround = true;
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;
    [HideInInspector] public bool isInPlayerHand = false;
    [SerializeField] private Vector3 grabRotation;

    public event Action<Grabbable> GrabEvent;
    public event Action<Grabbable> ReleaseEvent;
    private  float maxDistFromGrabPoint = 0.2f;

    private void Awake()
    {
        if (objectRigidBody == null)
            objectRigidBody = GetComponent<Rigidbody>();
    }

    public virtual void Grab(Transform objectGrabPointTransform, bool isGrabbedFromGround)
    {
        if (objectRigidBody == null) {
            objectRigidBody = GetComponent<Rigidbody>();
        }

        Grabbable[] itemComponents = GetComponents<Grabbable>();
        foreach (Grabbable itemComponent in itemComponents) {
            itemComponent.isInPlayerHand = true;
        }
        
        objectRigidBody.useGravity = false;
        objectRigidBody.isKinematic = true;
        Collider coll = GetComponent<Collider>();
        if (coll != null) {
            coll.excludeLayers = GetUserLayerMask();
        }
        this.isGrabbedFromGround = isGrabbedFromGround;
        this.objectGrabPointTransform = objectGrabPointTransform;
        this.transform.parent = null;

        ThrowGrabEvent();
    }

    public void Drop()
    {
        Grabbable[] itemComponents = GetComponents<Grabbable>();
        foreach (Grabbable itemComponent in itemComponents) {
            itemComponent.isInPlayerHand = false;
        }
        this.objectGrabPointTransform = null;
        
        
        Collider coll = GetComponent<Collider>();
        if (coll != null) {
            coll.excludeLayers = 0;
        }
        objectRigidBody.useGravity = true;
        objectRigidBody.isKinematic = false;
        this.transform.parent = SpawnedObjectsParentGetter.GetParent();

        ThrowReleaseEvent();
    }

    public void Update() {
        /*
        if (objectGrabPointTransform != null && !isGrabbedFromGround) {
            if (Vector3.Distance(objectGrabPointTransform.position, transform.position) > maxDistFromGrabPoint) {
                Vector3 direction = objectGrabPointTransform.position - transform.position;
                direction.Normalize();
                transform.position = objectGrabPointTransform.position - (direction*maxDistFromGrabPoint);
            }
        }
        */
        
        if (objectGrabPointTransform != null) {
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * 8);
            transform.position = newPosition;
            transform.rotation = objectGrabPointTransform.rotation * Quaternion.Euler(grabRotation);
        }
        
    }
    
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {

            // Calculate the desired position for the grabbed object
            //Vector3 targetPosition = objectGrabPointTransform.position + objectGrabPointTransform.forward;

            // Smoothly move the grabbed object towards the target position
            /*
            objectRigidBody.MovePosition(Vector3.Lerp(transform.position, objectGrabPointTransform.position, 5 * Time.deltaTime));
            if (isGrabbedFromGround && Vector3.Distance(objectGrabPointTransform.position, transform.position) <= maxDistFromGrabPoint) {
                foreach (Item i in GetComponents<Item>()) {
                    i.isGrabbedFromGround = false;
                }
            }
            */
            
            /*
            if (isGrabbedFromGround) {
                float lerpSpeed = 10;
                Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
                objectRigidBody.MovePosition(newPosition);
            
                objectRigidBody.MoveRotation(objectGrabPointTransform.rotation * Quaternion.Euler(grabRotation));
            }
            else
            {
                objectRigidBody.MovePosition(objectGrabPointTransform.position);
                objectRigidBody.MoveRotation(objectGrabPointTransform.rotation * Quaternion.Euler(grabRotation));
            }
            */
            
            
        }
    }

    private LayerMask GetUserLayerMask() {
        LayerMask userLayersMask = 0; // Start with an empty mask

        // Iterate through all layers
        for (int i = 0; i < 32; i++) {
            // Check if the layer is in use (user-settable)
            if (Physics.GetIgnoreLayerCollision(0, i) == false) {
                // Add the layer to the layer mask
                userLayersMask |= 1 << i;
            }
        }
        return userLayersMask;
    }

    public void ThrowGrabEvent() {
        if (GrabEvent != null) {
            GrabEvent(this);
        }
    }

    public void ThrowReleaseEvent() {
        if (ReleaseEvent != null) {
            ReleaseEvent(this);
        }
    }
}
