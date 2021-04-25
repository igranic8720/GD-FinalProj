using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MultiShotGun : Gun
{
    public override void Use()
    {
        Shoot();
        FireEffects();
    }

    public override void UseNoCalc()
    {
        FireEffects();
    }

    public override bool IsMultiFire()
    {
        return true;
    }

    
}