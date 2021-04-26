// FILE:    RoomPlayerInfo.cs
// DATE:    4/25/2021
// DESC:    This file holds references and easy getters for useful items in the room.

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomPlayerInfo : MonoBehaviour
{
    public static RoomPlayerInfo roomPlayerInfo; // this singleton instance

    private PlayerManager localPlayerMgr; // the local player manager instance
    private PlayerManager enemyPlayerMgr; // the enemy player manager instance
    private Player localPlayer; // the local player instance
    private Player enemyPlayer; // the enemy player instance

    public int localScore, enemyScore;

    // FUNCTION:    Awake
    // DESC:        Function ran when object on and enabled. Sets the singleton instance.
    // PARAMETERS:  0
    void Awake()
    {
        roomPlayerInfo = this;
        Initialize();
    }

    // FUNCTION:    Initialize
    // DESC:        Sets variables necessary.
    // PARAMETERS:  0
    void Initialize()
    {
        // assign us and enemy player
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.IsLocal)
            {
                localPlayer = p;
            }
            else
            {
                enemyPlayer = p;
            }
        }
    }

    // FUNCTION:    GetLocalPlayer
    // DESC:        Gets the local player.
    // PARAMETERS:  0
    public Player GetLocalPlayer()
    {
        return localPlayer;
    }

    // FUNCTION:    GetEnemyPlayer
    // DESC:        Gets the enemy player
    // PARAMETERS:  0
    public Player GetEnemyPlayer()
    {
        return enemyPlayer;
    }

    // FUNCTION:    GetLocalPlayerManager
    // DESC:        Gets the local player manager instance.
    // PARAMETERS:  0
    public PlayerManager GetLocalPlayerManager()
    {
        if (localPlayerMgr == null)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlayerManager"))
            {
                if (go.GetComponent<PhotonView>().IsMine)
                {
                    localPlayerMgr = go.GetComponent<PlayerManager>();
                    Debug.Log("Registered friendly pmgr.");
                }
            }
        }

        return localPlayerMgr;
    }

    // FUNCTION:    GetEnemyPlayerManager
    // DESC:        Gets the enemy player manager instance.
    // PARAMETERS:  0
    public PlayerManager GetEnemyPlayerManager()
    {
        if (enemyPlayerMgr == null)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlayerManager"))
            {
                if (!go.GetComponent<PhotonView>().IsMine)
                {
                    enemyPlayerMgr = go.GetComponent<PlayerManager>();
                    Debug.Log("Registered enemy pmgr.");
                }
            }
        }

        return enemyPlayerMgr;
    }

    // FUNCTION:    GetEnemyPlayerGO
    // DESC:        Gets the enemy player game object
    // PARAMETERS:  0
    public GameObject GetEnemyPlayerGO()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!go.GetComponent<PhotonView>().IsMine)
            {
                return go;
            }
        }

        return null;
    }
}
