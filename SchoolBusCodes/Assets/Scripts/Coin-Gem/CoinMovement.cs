using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;


public class CoinMovement : MonoBehaviour
{
    [SerializeField] float rotSpeed = 100f;
    public float deliveryTime = 1f;
    [SerializeField] float jumpCoefficient = 2f;
    float currentCounter = 0f;
    float distance = 0f;
    float yTarget;
    bool isFlying = false;
    Transform target;
    bool isRotating = true;
    public Transform Target
    {
        get { return target; }
        set
        {
            target = value;
            isFlying = true;
        }
    }
    bool targetSet = false;
    Vector3 initialPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void Rotate()
    {
        if (!isRotating)
        {
            return;
        }
        transform.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
    }

    private void Update()
    {
        Rotate();
        if (!isFlying)
        {
            return;
        }
        if (!targetSet)
        {
            initialPos = transform.position;
        }
        if (!targetSet)
        {
            distance = Vector3.Distance(initialPos, target.position);
            yTarget = (target.position.y + (jumpCoefficient));
        }
        targetSet = true;
        currentCounter += Time.deltaTime;
        float normalizedCounter = currentCounter / deliveryTime;
        if (normalizedCounter >= 1)
        {
            targetSet = false;
            distance = 0f;
            currentCounter = 0f;
            transform.position = target.position;
            isFlying = false;
         
            return;
        }
        float xPos = Mathf.Lerp(initialPos.x, target.position.x, normalizedCounter);
        float zPos = Mathf.Lerp(initialPos.z, target.position.z, normalizedCounter);
        float yPos;
        if (normalizedCounter <= 0.5f)
        {
            yPos = Mathf.Lerp(initialPos.y, yTarget, Mathf.Sin(normalizedCounter * Mathf.PI));
        }
        else
        {
            yPos = Mathf.Lerp(yTarget, target.position.y, 1f - Mathf.Sin(normalizedCounter * Mathf.PI));
        }
        transform.position = new Vector3(xPos, yPos, zPos);
    }

   
}

