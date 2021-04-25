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

    public GameObject bulletImpactPrefab;
    public ParticleSystem muzzleFlashParticleSystem;
    public AudioSource gunFireAudioSource;

    [SerializeField] private Camera cam;
    private PhotonView PV;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public void FireEffects() // visual and sound effects when firing
    {
        muzzleFlashParticleSystem.Play();
        gunFireAudioSource.PlayOneShot(((GunInfo)itemInfo).shootSound);
    }

    protected void Shoot()
    {
        PV.RPC("RPC_Fired", RpcTarget.Others);
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }

    [PunRPC]
    public void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 10f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }

    [PunRPC]
    public void RPC_Fired()
    {
        PlayerController pct = RoomPlayerInfo.roomPlayerInfo.GetEnemyPlayerGO().GetComponent<PlayerController>();
        pct.items[pct.itemIndex].UseNoCalc();
    }
}
