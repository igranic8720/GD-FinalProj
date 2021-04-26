// FILE:    PlayerManager.cs
// DATE:    4/25/2021
// DESC:    This file is responsible for managing background functionality of players, such as respawning.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    private PhotonView PV; // the photonview of this pmanager
    private GameObject controller; // the controller this playermanager manages

    // FUNCTION:    Awake
    // DESC:        Gets the PhotonView.
    // PARAMETERS:  0
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // FUNCTION:    Start
    // DESC:        Creates a controller if necessary.
    // PARAMETERS:  0
    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    // FUNCTION:    CreateController
    // DESC:        Creates a controller and registers the reference.
    // PARAMETERS:  0
    public void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetRandomSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
    }

    // FUNCTION:    Respawn
    // DESC:        Gets rid of the current controller and makes a new one.
    // PARAMETERS:  0
    public void Respawn()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }

    // FUNCTION:    GetController
    // DESC:        Returns the held controller.
    // PARAMETERS:  0
    public GameObject GetController()
    {
        return controller;
    }
}
