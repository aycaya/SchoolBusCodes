using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Transform target;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
        transform.position = target.position;
    }

    public void ChangeBus()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
