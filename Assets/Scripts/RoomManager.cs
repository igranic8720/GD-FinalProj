// FILE:    RoomManager.cs
// DATE:    4/25/2021
// DESC:    This file facilitates the loading of room resources.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance; // this singleton instance

    // FUNCTION:    Awake
    // DESC:        Ran when the gameobject is enabled and on. Sets the singleton instance.
    // PARAMETERS:  0
    void Awake()
    {
        Instance = this;
    }

    // FUNCTION:    OnEnable
    // DESC:        Subscribes a new event when the scene loads to load a player manager.
    // PARAMETERS:  0
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // FUNCTION:    OnDisable
    // DESC:        Unsubscribes the event.
    // PARAMETERS:  0
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // FUNCTION:    OnSceneLoaded
    // DESC:        The event when the scene loads. Creates a new player manager.
    // PARAMETERS:  2
    //              Scene scene: The scene loaded.
    //              LoadSceneMode load: Used when loading a scene.
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1) // MainScene
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
    
}
