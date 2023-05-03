using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class BusMovement : MonoBehaviour
{
    float scaleChangeSpeed = 7.5f;
    float interval = 0.75f;
    float scaleRate = 0.5f;
    SplineFollower follower;
    public float busSpeed = 20f;
    [SerializeField] float breakCoeff = 5f;
    CameraFollow cameraFollow;
    bool offsetPlus = false;
    bool offsetMinus = false;
    bool offsetNormal = false;
    float speed = 10f;
    Vector3 offsetMin;
    Vector3 offsetPls;
    Vector3 offsetNrml;
    float step;
    Vector3 followOffset;
    float desiredYScale = 1f;
    float originalYScale;
    Vector3 originalScaleVector;
    bool isMoving = false;
    float scaleCounter = 0f;
    PlayerController playerController;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    void Start()
    {
        originalYScale = transform.localScale.y;
        originalScaleVector = transform.localScale;
        desiredYScale = originalYScale;
        follower = GetComponent<SplineFollower>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        busSpeed = PlayerPrefs.GetFloat("Speed", busSpeed);
        breakCoeff = busSpeed * 5;
        offsetMin = new Vector3(-0.5f, cameraFollow.offset.y, cameraFollow.offset.z);
        offsetPls = new Vector3(0.5f, cameraFollow.offset.y, cameraFollow.offset.z);
        offsetNrml = new Vector3(0, cameraFollow.offset.y, cameraFollow.offset.z);
    }

    void Update()
    {
        float absoluteSpeed = Mathf.Abs(follower.followSpeed);
        if (playerController.isPressed)
        {
            isMoving = true;
            follower.followSpeed = busSpeed;
        }
        else
        {
            if (absoluteSpeed > 0.1f)
            {
                isMoving = true;
                if (follower.followSpeed > 0F)
                {
                    follower.followSpeed -= Time.deltaTime * breakCoeff;
                    follower.followSpeed = Mathf.Clamp(follower.followSpeed, 0.0001f, 999f);
                }
                if (absoluteSpeed <= 0.1f)
                {
                    StopBus();
                }
            }
            else
            {
                isMoving = false;
                StopBus();
            }
        }
        DoMoveAnim();
    }

    private void DoMoveAnim()
    {
        scaleCounter += Time.deltaTime;
        float scaleCounterMod = scaleCounter % (interval);
        if (scaleCounterMod < (interval / 2f) && isMoving)
        {
            desiredYScale = originalYScale + scaleRate;
        }
        else if (isMoving)
        {
            desiredYScale = originalYScale - scaleRate;
        }
        else
        {
            desiredYScale = originalYScale;
        }
        Vector3 desiredScale = new Vector3(originalScaleVector.x, desiredYScale, originalScaleVector.z);
        desiredScale=Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * scaleChangeSpeed);
        transform.localScale = desiredScale;
    }

    private void LateUpdate()
    {
        step += Time.deltaTime * speed;
        if (offsetPlus)
        {
            cameraFollow.offset.x = Mathf.SmoothStep(cameraFollow.offset.x, 0.6f, step);

        }


        if (offsetMinus)
        {
            cameraFollow.offset.x = Mathf.SmoothStep(cameraFollow.offset.x, -0.6f, step);

        }



        if (offsetNormal)
        {
            cameraFollow.offset.x = Mathf.SmoothStep(cameraFollow.offset.x, 0f, step);

        }

        step = 0;
    }
    void WaitAndFalse()
    {
        offsetNormal = false;
        offsetMinus = false;
        offsetPlus = false;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OffsetPlus"))
        {
            offsetPlus = true;

            offsetNormal = false;
            offsetMinus = false;
        }
        else if (other.CompareTag("OffsetMinus"))
        {


            offsetMinus = true;
            offsetNormal = false;
            offsetPlus = false;

        }
        else if (other.CompareTag("OffsetNormal"))
        {

            offsetNormal = true;
            offsetMinus = false;
            offsetPlus = false;

        }
    }
    public void UpdateSpeedVal()
    {
        busSpeed = PlayerPrefs.GetFloat("Speed", busSpeed);
        breakCoeff = busSpeed * 5;
    }

    private void StopBus()
    {

       follower.followSpeed = 0.0001f;
    }
}
