using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject py in objects)
        {
            if (Vector3.Distance(py.transform.position, transform.position) <= 4f)
            {
                animator.SetBool("character_nearby", true);
            }
            else
            {
                animator.SetBool("character_nearby", false);
            }
        }
    }
}
