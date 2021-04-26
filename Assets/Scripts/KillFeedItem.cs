// FILE:    KillFeedItem.cs
// DATE:    4/25/2021
// DESC:    This file facilitates the setup of kill feed line items.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour
{
    [SerializeField] private Text text;
    // FUNCTION:    Setup
    // DESC:        setup for the line item. takes in the shooter name and the victim name
    //              and the distance.  This creates the kill message.
    // PARAMETERS:  string shooter, string victim, float distance
    public void Setup(string shooter, string victim, float distance)
    {
        text.text = DateTime.Now.ToString() + " "+ shooter + " killed " + victim + " (" + (int)distance + " m)";
    }
}
