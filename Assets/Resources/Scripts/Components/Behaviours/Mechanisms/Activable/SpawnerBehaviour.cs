using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : ActivableBehaviour
{
    [Header("Customization (Spawner)")]

    [SerializeField] bool spawnOnDestroy;
    [SerializeField] float Delay = 0.25f;
    
    [SerializeField] GameObject spawnPrefab;
    [SerializeField] int interval = 0;
    int timer = 0;
    [SerializeField] float trackDistance = 1000;

    List<GameObject> trackedSpawns = new();

    bool skibidiBorrame = true;
    [SerializeField] bool alternativeActivation;

    protected override void Start()
    {
        base.Start();

        timer = 0;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!alternativeActivation)
        {
            
            CheckSpawnValidity();


            if (active)
            {
                if (spawnOnDestroy)
                {
                    if (trackedSpawns.Count == 0 && skibidiBorrame)
                    {
                        StartCoroutine(Spawn());
                    }
                }
                else
                {

                    timer++;
                    if (timer >= interval)
                    {
                        StartCoroutine(Spawn());
                        timer = 0;
                    }

                }
            }
            else timer = 0;
        }
    }
    
    public void DoSpawn()
    {
        active = true;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        skibidiBorrame = false;
        yield return new WaitForSecondsRealtime(Delay);
        skibidiBorrame = true;
        
        if (active)
        {
            GameObject newSpawn;

            newSpawn = Instantiate(spawnPrefab);
            newSpawn.transform.position = transform.position;
            newSpawn.transform.rotation = transform.rotation;
            trackedSpawns.Add(newSpawn);
        }
    }

    void CheckSpawnValidity()
    {
        bool delete = false;
        int index = 0;

        foreach (GameObject trackedSpawn in trackedSpawns)
        {
            if (trackedSpawn == null)
            {
                delete = true;
                index = trackedSpawns.IndexOf(trackedSpawn);
                break;
            }
            else if (Vector3.Distance(transform.position, trackedSpawn.transform.position) > trackDistance)
            {

                delete = true;
                index = trackedSpawns.IndexOf(trackedSpawn);
                Destroy(trackedSpawn);
                break;
            }
        }

        if (delete)
        {
            trackedSpawns.RemoveAt(index);
        }
    }
}
