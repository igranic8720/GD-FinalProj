// FILE:    SpawnManager.cs
// DATE:    4/25/2021
// DESC:    This file facilitates the retrieval of spawnpoints.

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance; // this singleton instance

    private Spawnpoint[] spawnpoints; // the array of spawnpoints
    private int lastSpawnpoint; // the last spawnpoint given, to avoid reusing one

    // FUNCTION:    Awake
    // DESC:        Function ran when object on and enabled. Gets all spawnpoints.
    // PARAMETERS:  0
    void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<Spawnpoint>();
    }

    // FUNCTION:    GetSpawnpoint
    // DESC:        Gets the position of a spawnpoint.
    // PARAMETERS:  1
    //              int spawnpoint: The index of the spawnpoint.
    public Transform GetSpawnpoint(int spawnpoint)
    {
        return spawnpoints[spawnpoint].transform;
    }

    // FUNCTION:    GetRandomSpawnpoint
    // DESC:        Get a random spawnpoint.
    // PARAMETERS:  0
    public Transform GetRandomSpawnpoint()
    {
        while (true)
        {
            int spawnpt = 0;
            if (PhotonNetwork.IsMasterClient)
            {
                spawnpt = Random.Range(0, 9);
            }
            else
            {
                spawnpt = Random.Range(9, spawnpoints.Length);
            }

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
