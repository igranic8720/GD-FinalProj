// FILE:    PlayerGroundCheck.cs
// DATE:    4/25/2021
// DESC:    This file manages the ground check component of the movement controller.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private PlayerController playerController; // movement controller

    // FUNCTION:    Awake
    // DESC:        Sets the movement controller.
    // PARAMETERS:  0
    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    // FUNCTION:    OnTriggerEnter
    // DESC:        Event when collider collides with something.
    // PARAMETERS:  1
    //              Collider other: Other object being collided with.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(true);
    }

    // FUNCTION:    OnTriggerExit
    // DESC:        Event when collider stops colliding with something.
    // PARAMETERS:  1
    //              Collider other: Other object being collided with.
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(false);
    }

    // FUNCTION:    OnTriggerStay
    // DESC:        Event when collider continues colliding with something
    // PARAMETERS:  1
    //              Collider other: Other object being collided with.
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(true);
    }

    // FUNCTION:    OnCollisionEnter
    // DESC:        Event when collider collides with something.
    // PARAMETERS:  1
    //              Collider other: Other object being collided with.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(true);
    }

    // FUNCTION:    OnCollisionExit
    // DESC:        Event when collider stops colliding with something.
    // PARAMETERS:  1
    //              Collider other: Other object being collided with.
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(false);
    }

    // FUNCTION:    OnCollisionStay
    // DESC:        Event when collider continues colliding with something.
    // PARAMETERS:  1
    //              Collider other: Other object being collided with.
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject) return;
        playerController.SetGroundedState(true);
    }
}
