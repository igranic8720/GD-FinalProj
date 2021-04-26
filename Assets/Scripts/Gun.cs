// FILE:    Gun.cs
// DATE:    4/25/2021
// DESC:    All gun usage.

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public abstract class Gun : Item
{
    public abstract override void Use(); // to be overriden per gun
    public abstract override void UseNoCalc();
    public abstract override bool IsMultiFire();

    public GameObject bulletImpactPrefab; // the prefab to use when making bullet impacts
    public ParticleSystem muzzleFlashParticleSystem; // the PS used to make muzzle flashes
    public AudioSource gunFireAudioSource; // the audiosource for playing gunshots

    [SerializeField] private Camera cam; // camera used on player
    private PhotonView PV; // photonview the player has

    // FUNCTION:    Awake
    // DESC:        Sets needed photonview
    // PARAMETERS:  0
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // FUNCTION:    FireEffects
    // DESC:        Plays necessary effects when firing weapon.
    // PARAMETERS:  0
    public void FireEffects() // visual and sound effects when firing
    {
        muzzleFlashParticleSystem.Play();
        gunFireAudioSource.PlayOneShot(((GunInfo)itemInfo).shootSound);
    }

    // FUNCTION:    Shoot
    // DESC:        Shoots a gun, over the network
    // PARAMETERS:  0
    protected void Shoot()
    {
        PV.RPC("RPC_Fired", RpcTarget.Others); // ensures all players see it being fired
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)); // test if it hit anything
        ray.origin = cam.transform.position; 

        if (Physics.Raycast(ray, out RaycastHit hit)) // if it did hit
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage); // make that object take damage
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal); // send the RPC to spawn a bullet impact
        }
    }

    // FUNCTION:    RPC_Shoot
    // DESC:        Receive a networked impact
    // PARAMETERS:  2
    //              Vector3 hitPosition: The place where it hit
    //              Vector3 hitNormal: The normal of where it hit
    [PunRPC]
    public void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f); // get a sphere of the area it hit
        if (colliders.Length != 0) // if there are colliders
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 10f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }

    // FUNCTION:    RPC_Fired
    // DESC:        Receive a player fired event, play some effects
    // PARAMETERS:  0
    [PunRPC]
    public void RPC_Fired()
    {
        PlayerController pct = RoomPlayerInfo.roomPlayerInfo.GetEnemyPlayerGO().GetComponent<PlayerController>();
        pct.items[pct.itemIndex].UseNoCalc();
    }
}
