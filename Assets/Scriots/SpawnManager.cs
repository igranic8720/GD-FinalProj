using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private Spawnpoint[] spawnpoints;
    private int lastSpawnpoint;

    void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<Spawnpoint>();
    }
    
    public Transform GetSpawnpoint(int spawnpoint)
    {
        return spawnpoints[spawnpoint].transform;
    }

    public Transform GetRandomSpawnpoint()
    {
        while (true)
        {
            int spawnpt = Random.Range(0, spawnpoints.Length);
            if (spawnpt != lastSpawnpoint)
            {
                GameObject[] plys = GameObject.FindGameObjectsWithTag("Player");
                Transform pos = spawnpoints[spawnpt].transform;
                bool valid = true;
                foreach (GameObject ply in plys)
                {
                    if (Vector3.Distance(ply.transform.position, pos.position) <= 4f)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    lastSpawnpoint = spawnpt;
                    return pos;
                }
            }
        }
    }
}
