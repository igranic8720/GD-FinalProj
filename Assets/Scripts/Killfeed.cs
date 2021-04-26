// FILE:    KillFeed.cs
// DATE:    4/25/2021
// DESC:    This file facilitates the spawning of kill feed line items.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killfeed : MonoBehaviour
{
    public GameObject KillFeedItemPrefab;
    public void newKill(string shooter, string victim, float distance)
    {
        GameObject.Instantiate(KillFeedItemPrefab, gameObject.transform).GetComponent<KillFeedItem>().Setup(shooter, victim, distance);
    }

    private void Update()
    {
        if(gameObject.transform.childCount >= 5)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
    }
}
