// FILE:    Item.cs
// DATE:    4/25/2021
// DESC:    Base item class.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemInfo itemInfo; // what iteminfo is this item
    public GameObject itemGameObject; // what gameobject is this item

    public abstract void Use(); // what to do when using this item?
    public abstract void UseNoCalc(); // using except no calculations
    public abstract bool IsMultiFire(); // is this item used multiple times in quick successions?
}
