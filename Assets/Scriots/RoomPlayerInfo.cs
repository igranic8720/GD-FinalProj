using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomPlayerInfo : MonoBehaviour
{
    public static RoomPlayerInfo roomPlayerInfo;

    private PlayerManager localPlayerMgr;
    private PlayerManager enemyPlayerMgr;
    private Player localPlayer;
    private Player enemyPlayer;

    public int localScore, enemyScore;

    void Awake()
    {
        roomPlayerInfo = this;
        Initialize();
    }

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

    public Player GetLocalPlayer()
    {
        return localPlayer;
    }

    public Player GetEnemyPlayer()
    {
        return enemyPlayer;
    }

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
