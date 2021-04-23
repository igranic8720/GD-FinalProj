using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private Spawnpoint[] spawnpoints;

    void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<Spawnpoint>();
    }

    public Transform GetSpawnpoint()
    {
        while (true)
        {
            bool invalid = false;
            Transform pos = spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
            GameObject[] plys = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject ply in plys)
            {
                if (Vector3.Distance(ply.transform.position, pos.position) <= 4f)
                {
                    invalid = true;
                    break;
                }
            }

            if (invalid == false) return pos;
        }
    }
}
