using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ObjectSpawner {

    public static GameObject Spawn(GameObject toSpawn, Vector3 position) {
        return Spawn(toSpawn, position, Quaternion.identity, Vector3.zero);
    }

    public static GameObject Spawn(GameObject toSpawn, Vector3 position, Quaternion rotation) {
        return Spawn(toSpawn, position, rotation, Vector3.zero);
    }

    public static GameObject Spawn(GameObject toSpawn, Vector3 position, Vector3 force) {
        return Spawn(toSpawn, position, Quaternion.identity, force);
    }

    public static GameObject Spawn(GameObject toSpawn, Vector3 position, Vector3 force, Quaternion rotation) {
        return Spawn(toSpawn, position, rotation, force);
    }

    public static GameObject Spawn(GameObject toSpawn, Vector3 position, Quaternion rotation, Vector3 force) {
        GameObject spawned = null;
        if (toSpawn != null) {
            spawned = Object.Instantiate(toSpawn, position, Quaternion.identity);
            if (force != Vector3.zero) {
                Rigidbody spawnedRigidbody = spawned.transform.GetComponent<Rigidbody>();
                if (spawnedRigidbody != null) {
                    spawnedRigidbody.AddForce(force, ForceMode.Impulse);
                }
            }
        }

        return spawned;
    }
}
