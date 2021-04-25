using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    private PhotonView PV;
    private GameObject controller;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    
    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    public void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetRandomSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
    }

    public void Respawn()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }

    public GameObject GetController()
    {
        return controller;
    }
}