using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : ActivableBehaviour
{    
    [Header("Customization (Spawner)")]
    [SerializeField] GameObject spawnPrefab;
    [SerializeField] int interval = 0;
    int counter = 0;
    [SerializeField] float trackDistance = 1000;

    List<GameObject> trackedSpawns = new();

    protected void Start()
    {
        counter = 0;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckSpawnProximity();

        if (active)
        {
            counter++;
            if (counter >= interval)
            {
                Spawn();
                counter = 0;
            }
        }
        else counter = 0;
    }

    void Spawn()
    {
        Debug.Log("SPAWNED");

        GameObject newSpawn = Instantiate(spawnPrefab);
        newSpawn.transform.position = transform.position;
        newSpawn.transform.rotation = transform.rotation;
        trackedSpawns.Add(newSpawn);
    }

    void CheckSpawnProximity()
    {
        bool delete = false;
        int index = 0;

        foreach (GameObject trackedSpawn in trackedSpawns)
        {
            if (trackedSpawn == null)
            {
                Debug.Log("NULL");

                delete = true;
                index = trackedSpawns.IndexOf(trackedSpawn);
                return;
            }
            else if (Vector3.Distance(transform.position, trackedSpawn.transform.position) > trackDistance)
            {
                Debug.Log("TOO FAR");

                delete = true;
                index = trackedSpawns.IndexOf(trackedSpawn);
                Destroy(trackedSpawn);
                break;
            }
        }

        if (delete) trackedSpawns.RemoveAt(index);
    }
}
