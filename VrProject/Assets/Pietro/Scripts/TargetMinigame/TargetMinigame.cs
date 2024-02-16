using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class TargetMinigame : MonoBehaviour {
    [Header("Target settings and placement")]
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private List<GameObject> emptyObjects = new List<GameObject>();
    [SerializeField] private int numTotalTargets = 10;
    [Header("Animal type")]
    [SerializeField] private Animal.AnimalName animal;
    [Header("Spawnable object settings")]
    [SerializeField] private Transform toSpawnAtStart;
    [Header("Item that will be held after minigame")]
    [SerializeField] private GameObject resultItemPrefab;
    [Header("Reference to Animator (if needed)")]
    [SerializeField] private Animator _animator;
    private float _animationPrevSpeed;

    //[Header("Minigame camera reference")]
    private Camera minigameCamera;

    private GameObject spawnedItem = null;
    private Target currentTarget = null;
    private int numSpawnedTargets = 0;
    private Camera mainCamera = null;
    private NPCMover _npc;
    private PlayerPickUpDrop playerPickUp = null;
    private bool minigameRunning = false;

    private List<Vector3> relativePositions = new List<Vector3>();

    private Action updateFunction = () => { }; //function to call during update
    public Action CompleteEvent;

    void Awake() {
        minigameCamera= GetComponentInChildren<Camera>();
        //verify and disable camera
        if (minigameCamera == null) {
            Debug.LogError(transform.name + ": TargetMinigame, no minigame camera set");
        }

        if (minigameCamera.gameObject.activeSelf) {
            minigameCamera.gameObject.SetActive(false);
            /*
            AudioListener al = minigameCamera.GetComponent<AudioListener>();
            al.enabled = false;
            minigameCamera.enabled = false;
            */
        }

        _npc = GetComponent<NPCMover>();
        if (_npc == null) {
            Debug.LogError(transform.name + ": Target Minigame should be attached to a NPCMover GameObject");
        }

        switch (animal) {
            case Animal.AnimalName.Cow:
                Random rng = new Random();
                for (int i = 0; i < numTotalTargets; i++) {
                    int rand = rng.Next(0, emptyObjects.Count);
                    relativePositions.Add(emptyObjects[rand].transform.localPosition);
                }
                break;

            case Animal.AnimalName.Horse:
                if (emptyObjects.Count != numTotalTargets) {
                    Debug.LogError("ERRORE quest cavallo: numero di empty objects minore del numero " + numTotalTargets + " di target richiesti.");
                } else {
                    for (int i = 0; i < numTotalTargets; i++) {
                        relativePositions.Add(emptyObjects[i].transform.localPosition);
                    }
                }
                break;

            default:
                Debug.LogError(transform.name + ": Target Minigame, illegal animal type specified");
                break;
        }

        //hide additional item if it's provided
        SetAdditionalItem(false);
        //Target.OnTargetClicked += CheckTargetList;

    }

    public void BeginMinigame(Camera playerCamera, PlayerPickUpDrop pickUp) {
        minigameRunning = true;

        playerPickUp = pickUp;

        //make NPC stop moving
        _npc.StopMoving();

        //animate
        if (_animator!= null) {
            _animator.SetBool("minigameRunning", true);
            _animationPrevSpeed = _animator.speed;
            _animator.speed = 1;
        }

        //disable NPC collider
        Collider coll = _npc.GetComponent<Collider>();
        if (coll != null) {
            coll.enabled = false;
        }

        //disable player inputs
        InputManager.DisableInteractions();
        InputManager.DisableMovement();
        InputManager.DisableMenu();

        //change camera
        this.mainCamera = Camera.main;
        SwitchCamera(mainCamera, minigameCamera);

        //spawn additional item if needed
        SetAdditionalItem(true);

        //activate update function
        ChangeUpdateBehaviour(true);

        UnlockCursor();

        //actual beginning
        currentTarget = null;
        numSpawnedTargets = 0;
        if (animal == Animal.AnimalName.Cow) {
            ShuffleList(relativePositions); //shuffle targets positions
        }

        CheckTargetList();
    }

    private void SpawnTarget(Vector3 pos) {

        GameObject newTargetGameObj = Instantiate(targetPrefab, new Vector3(), new Quaternion(), transform);
        newTargetGameObj.transform.SetParent(this.transform, false);
        Debug.LogWarning("Instantiated: " + newTargetGameObj.name);
        currentTarget = newTargetGameObj.GetComponent<Target>();
        if (currentTarget == null) {
            Debug.LogError(transform.name + ": " + targetPrefab.name + " has no \"Target\" component");
        }

        currentTarget.transform.localPosition = pos;
        //currentTarget.transform.LookAt(minigameCamera.transform.position);
        currentTarget.transform.forward = - minigameCamera.transform.forward;
        

        //subscribe to current target click event
        currentTarget.OnTargetClicked += CheckTargetList;

        numSpawnedTargets++;
    }

    private void CheckTargetList() {
        //unsubscribe from current target click event
        if (currentTarget != null) {
            currentTarget.OnTargetClicked -= CheckTargetList;
        }

        if (numSpawnedTargets < numTotalTargets) {
                SpawnTarget(relativePositions[numSpawnedTargets]);
            Debug.LogWarning("Targets: " + numSpawnedTargets + "/" + numTotalTargets);
        } else {
            EndMinigame();
        }
    }

    private void EndMinigame(bool success = true) {
        Debug.LogWarning("Ending minigame");
        LockCursor();

        //disable update function
        ChangeUpdateBehaviour(false);

        //remove additional item
        SetAdditionalItem(false);

        //reset camera
        SwitchCamera(minigameCamera, mainCamera);
        mainCamera = null;

        //enable inputs
        InputManager.EnableInteractions();
        InputManager.EnableMovement();
        InputManager.EnableMenu();

        //enable NPC collider
        Collider coll = _npc.GetComponent<Collider>();
        if (coll != null) {
            coll.enabled = true;
        }

        //animate
        if (_animator != null) {
            _animator.SetBool("minigameRunning", false);
            _animator.speed = _animationPrevSpeed;
            _animationPrevSpeed = _animator.speed;
        }

        //make NPC move again
        _npc.StartMoving();

        //if success remove held item
        if (success) {
            playerPickUp.deposit.AddItem(playerPickUp.hotbar.activeItemObj.itemName);
            RemoveHeldItem();
            if (resultItemPrefab != null) {
                GameObject spawned = Spawner.Spawn(resultItemPrefab, new Vector3());
                Item heldItem = spawned.GetComponent<Item>();
                if (heldItem != null) {
                    if (!playerPickUp.PickUpAndSelect(heldItem)) {
                        Debug.LogWarning("Could not add item to hotbar");
                        heldItem.transform.position = _npc.transform.position;
                    }
                }
            }
        }

        playerPickUp = null;

        minigameRunning = false;

        if (CompleteEvent != null) {
            CompleteEvent();
        }
    }

    private void Update() {
        updateFunction();
    }

    private void SwitchCamera(Camera oldCamera, Camera cam) {
        AudioListener audioListener;
        bool alEnabled = true;
        if (oldCamera != null) {
            audioListener = oldCamera.GetComponent<AudioListener>();
            alEnabled = (audioListener != null && audioListener.enabled);
            oldCamera.gameObject.SetActive(false);
            /*
            audioListener = oldCamera.GetComponent<AudioListener>();
            if (audioListener != null) {
                audioListener.enabled = false;
            }
            oldCamera.enabled = false;
            */

        }

        if (cam != null) {

            cam.gameObject.SetActive(true);
            audioListener = cam.GetComponent<AudioListener>();
            if (audioListener != null && !alEnabled) {
                audioListener.enabled = false;
            }

        }
        /*
        cam.enabled = true;
        audioListener = cam.GetComponent<AudioListener>();
        if (audioListener != null) {
            audioListener.enabled = true;
        }
        */
    }

    private void SetAdditionalItem(bool activate) {
        //spawn object if needed (e.g. bucket)
        if (toSpawnAtStart != null) {
            toSpawnAtStart.gameObject.SetActive(activate);
        }
    }

    public Animal.AnimalName GetAnimalType() {
        return animal;
    }

    static void ShuffleList<T>(List<T> list) {
        Random rng = new Random();

        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void ChangeUpdateBehaviour(bool activateUpdate) {
        if (activateUpdate) {
            updateFunction = () => {
                if(Input.GetKeyDown(KeyCode.Backspace)) {
                    //force exit minigame
          
                    if (currentTarget != null) {
                        currentTarget.OnTargetClicked -= CheckTargetList;
                        if (!currentTarget.IsAlreadyClicked()) {
                            Destroy(currentTarget.gameObject);
                        }
                        currentTarget = null;
                    }

                    //exit minigame with success = false
                    EndMinigame(false);
                }
            };

        } else {
            updateFunction = () => { };
        }
    }

    void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public bool IsMinigameRunning() {
        return minigameRunning;
    }

    private void RemoveHeldItem() {
        playerPickUp.hotbar.RemoveHeldItem();
    }
}
