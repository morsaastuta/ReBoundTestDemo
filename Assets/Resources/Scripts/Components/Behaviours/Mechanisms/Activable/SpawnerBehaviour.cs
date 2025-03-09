using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : ActivableBehaviour
{
    [Header("Customization (Spawner)")]

    [SerializeField] bool spawnOnDestroy;

    [SerializeField] GameObject spawnPrefab;
    [SerializeField] int interval = 0;
    int timer = 0;
    [SerializeField] float trackDistance = 1000;

    List<GameObject> trackedSpawns = new();


    GameObject newSpawn;

    protected void Start()
    {
        timer = 0;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckSpawnProximity();
        
        
        if (active) 
        { 
            if (spawnOnDestroy)
            {
                if (newSpawn == null)
                {
                    Spawn();
                }
            }
            else
            {
            
                timer++;
                if (timer >= interval)
                {
                    Spawn();
                    timer = 0;
                }
            
            }
        }
        else timer = 0;
    }
    
    void Spawn()
    {

        newSpawn = Instantiate(spawnPrefab);
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

                delete = true;
                index = trackedSpawns.IndexOf(trackedSpawn);
                return;
            }
            else if (Vector3.Distance(transform.position, trackedSpawn.transform.position) > trackDistance)
            {

                delete = true;
                index = trackedSpawns.IndexOf(trackedSpawn);
                Destroy(trackedSpawn);
                break;
            }
        }

        if (delete) trackedSpawns.RemoveAt(index);
    }
}
