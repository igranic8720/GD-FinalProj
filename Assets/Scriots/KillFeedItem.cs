using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour
{
    [SerializeField] private Text text;

    public void Setup(string shooter, string victim)
    {
        text.text = shooter + " killed " + victim;
    }
}
