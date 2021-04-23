using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator animator;
    public PhotonView PV;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PV = gameObject.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!PV.IsMine) { return; }
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject py in objects)
        {
            if (Vector3.Distance(py.transform.position, transform.position) <= 4f)
            {
                animator.SetBool("character_nearby", true);
                break;
            }
            else
            {
                animator.SetBool("character_nearby", false);
            }
        }
    }
}
