using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class TargetMinigame : MonoBehaviour
{
    public enum Animal
    {
        Horse,
        Cow
    }

    [SerializeField] private GameObject targetObj;
    [SerializeField] private int numTotalTargets = 10;
    [SerializeField] private int numSpawnedTargets = 0;
    [SerializeField] private Animal animal;
    [SerializeField] private List<GameObject> relativePositions = new List<GameObject>();
    private GameObject localTarget;

    // Start is called before the first frame update
    void Start()
    {
        switch (animal)
        {
            case Animal.Cow:
                
                break;
            
            case Animal.Horse:
                
                break;
            
            default:
                break;
        }
        Target.OnTargetClicked += CheckTargetList;
    }

    // Update is called once per frame
    private void StartMinigame()
    {
        numSpawnedTargets = 0;
        CheckTargetList();
    }

    public void EndMinigame()
    {
        
    }

    private void SpawnTarget(Vector3 pos)
    {
        localTarget = Instantiate(targetObj, new Vector3(), new Quaternion(), transform);
        localTarget.transform.localPosition = pos;
        numSpawnedTargets++;
    }

    private void CheckTargetList()
    {
        if (numSpawnedTargets < numTotalTargets)
        {
            SpawnTarget(relativePositions[numSpawnedTargets].transform.position);
        }
        else
        {
            EndMinigame();
        }
    }
}
