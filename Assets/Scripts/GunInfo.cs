// FILE:    GunInfo.cs
// DATE:    4/25/2021
// DESC:    Base gun info class.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FPS/New Gun")]
public class GunInfo : ItemInfo
{
    public float damage; // how much damage does this gun do
    public AudioClip shootSound; // sound to make when firing once
}
