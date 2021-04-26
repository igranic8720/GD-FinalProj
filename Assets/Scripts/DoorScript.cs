// FILE:    DoorScript.cs
// DATE:    4/25/2021
// DESC:    This file facilitates the opening of doors.
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator animator;
    public PhotonView PV;
    // FUNCTION:    Start
    // DESC:        Function ran when object on and enabled. Gets animaotr of door attached.
    // PARAMETERS:  void
    private void Start()
    {
        animator = GetComponent<Animator>();
        PV = gameObject.GetComponent<PhotonView>();
    }
    // FUNCTION:    Update
    // DESC:        opens and closes door over network using the photon animator view.
    //              thus only master needs to set animator variables.
    // PARAMETERS:  void
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
