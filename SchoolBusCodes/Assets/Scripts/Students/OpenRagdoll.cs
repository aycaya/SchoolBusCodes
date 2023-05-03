using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenRagdoll : MonoBehaviour
{
    Rigidbody[] ragdollRigidbodies;
    Animator animator;
    
    bool enableBool = false;

    void Start()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = gameObject.transform.parent.gameObject.GetComponent<Animator>();
        DisableRagdoll();
    }

    public void DisableRagdoll()
    {
        animator.enabled = true;
        foreach (var ragdoll in ragdollRigidbodies)
        {
            ragdoll.isKinematic = true;
            ragdoll.useGravity = false;
        }
    }

    public void EnableRagdoll() 
    {
        animator.enabled = false;
        foreach (var ragdoll in ragdollRigidbodies)
        {
            ragdoll.isKinematic = false;
            ragdoll.useGravity = true;
        }
    }
}
