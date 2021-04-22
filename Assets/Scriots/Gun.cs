using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    public abstract override void Use(); // to be overriden per gun

    public GameObject bulletImpactPrefab;
    public ParticleSystem muzzleFlashParticleSystem;
    public AudioSource gunFireAudioSource;

    public void FireEffects() // visual and sound effects when firing
    {
        muzzleFlashParticleSystem.Play();
        gunFireAudioSource.Play();
    }
}
