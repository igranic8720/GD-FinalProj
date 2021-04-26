// FILE:    MultiShotGun.cs
// DATE:    4/25/2021
// DESC:    This file facilitates multi-fire weapons and their functionality.

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MultiShotGun : Gun
{
    // FUNCTION:    Use
    // DESC:        Uses this weapon.
    // PARAMETERS:  0
    public override void Use()
    {
        Shoot();
        FireEffects();
    }

    // FUNCTION:    UseNoCalc
    // DESC:        Uses this weapon but does not send any networked events involving calculations.
    // PARAMETERS:  0
    public override void UseNoCalc()
    {
        FireEffects();
    }

    // FUNCTION:    IsMultiFire
    // DESC:        Returns whether this weapon uses multifire abilities or not.
    // PARAMETERS:  0
    public override bool IsMultiFire()
    {
        return true;
    }

    
}