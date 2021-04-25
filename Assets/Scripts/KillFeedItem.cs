using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour
{
    [SerializeField] private Text text;

    public void Setup(string shooter, string victim, float distance)
    {
        text.text = DateTime.Now.ToString() + " "+ shooter + " killed " + victim + " (" + (int)distance + " m)";
    }
}
