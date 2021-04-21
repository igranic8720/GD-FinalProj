using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] private Camera cam;

    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {

    }
}
