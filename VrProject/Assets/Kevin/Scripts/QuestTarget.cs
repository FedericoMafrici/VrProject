using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public class QuestTarget : Quest
{
    [SerializeField] private string description;
    [SerializeField] private GameObject targetObj;
    [SerializeField] private int numTotalTargets = 10;
    [SerializeField] private int numSpawnedTargets = 0;
    [SerializeField] private Animal.AnimalName animal;
    [SerializeField] private List<GameObject> emptyObjects;
    private Camera questCamera;
    private GameObject localTarget;
    private List<Vector3> relativePositions;

    protected override void Init()
    {
        questCamera = gameObject.GetComponent<Camera>();
        
        switch (animal)
        {
            case Animal.AnimalName.Cow:
                for (int i = 0; i < numTotalTargets; i++)
                {
                    int rand = (new Random()).Next(0, emptyObjects.Count);
                    relativePositions.Add(emptyObjects[rand].transform.position);
                }
                break;
            
            case Animal.AnimalName.Horse:
                if (emptyObjects.Count != numTotalTargets)
                {
                    Debug.Log("ERRORE quest cavallo: numero di empty objects minore del numero " + numTotalTargets+ " di target richiesti.");
                }
                else
                {
                    for (int i = 0; i < numTotalTargets; i++)
                    {
                        relativePositions.Add(emptyObjects[i].transform.position);
                    }
                }
                break;
            
            default:
                break;
        }
        //Target.OnTargetClicked += CheckTargetList;
        
        base.Init();
    }

    public override string GetQuestDescription()
    {
        return description;
    }

    public void Begin()
    {
        // CHECK SECCHIO IN MANO
        // RIMUOVI SECCHIO DALLA MANO
        // POSIZIONA IL SECCHIO
        // REGOLAZIONE CAMERA

        numSpawnedTargets = 0;
        CheckTargetList();
    }

    private void SpawnTarget(Vector3 pos)
    {
        localTarget = Instantiate(targetObj, new Vector3(), new Quaternion(), transform);
        localTarget.transform.localPosition = pos;
        localTarget.transform.LookAt(questCamera.transform.position);
        
        numSpawnedTargets++;
    }

    private void CheckTargetList()
    {
        if (numSpawnedTargets < numTotalTargets)
        {
            if(numSpawnedTargets != 0)
                Progress();
            SpawnTarget(relativePositions[numSpawnedTargets]);
        }
        else
        {
            Complete();
        }
    }
}
