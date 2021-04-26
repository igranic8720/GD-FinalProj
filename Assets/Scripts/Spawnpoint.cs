// FILE:    Spawnpoint.cs
// DATE:    4/25/2021
// DESC:    This file despawns spawn points upon creation.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    [SerializeField] private GameObject graphics; // the spawnpoint itself

    // FUNCTION:    Awake
    // DESC:        Function ran when object on and enabled. Disables spawnpoints graphics.
    // PARAMETERS:  0
    void Awake()
    {
        graphics.SetActive(false); // disables this spawnpoint
    }
}
