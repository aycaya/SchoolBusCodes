using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    [SerializeField] Vector3 defaultEulerAngles;
    Camera sceneCamera;
    GameObject[] enemyList;
    Vector3 desiredPosition;
    Vector3 desiredAngles;
    Vector3 initialCameraPos;
    Vector3 minCameraPosOffset;
    Vector3 maxCameraPosOffset;
    Vector3 minEulerAngles;
    Vector3 maxEulerAngles;
    float humanPercent = 1f;
    public bool isGameFinished = false;
    float initialFOV = 60f;
    float maximumFOV;
   

    private void Awake()
    {
        sceneCamera = Camera.main;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        initialCameraPos = transform.position;
    }
    public void ChangePlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }
  
    void LateUpdate()
    {
        desiredAngles = defaultEulerAngles;
        desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        Quaternion quaternion = Quaternion.Euler(desiredAngles.x, desiredAngles.y, desiredAngles.z);
        Quaternion smoothedQuat = Quaternion.Lerp(transform.rotation, quaternion, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        transform.rotation = smoothedQuat;
    }
}
