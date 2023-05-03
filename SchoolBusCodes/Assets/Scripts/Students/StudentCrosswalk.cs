using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentCrosswalk : MonoBehaviour
{
    Vector3 startPos;
    Quaternion startRot;
    public bool canMove=true;
    Rigidbody rb;
    public bool coolingdown = false;
    [SerializeField] float coolDownTime = 10f;
    public float coolDT = 10f;
    Bus busScript;
    Animator animator;
    public Coroutine WaitAndStartOverCoRoutine;
    Transform childObject;

    void Start()
    {
        childObject = transform.GetChild(0);
        canMove = true;
        startPos = transform.position;
        startRot = transform.rotation;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!coolingdown)
        {
            StudentWalkAcross();


        }
        else
        {
            if (!canMove)
            {
                return;
            }
            coolDT -= Time.deltaTime;

            if (coolDT <= 0)
            {
                coolingdown = false;
                coolDT = coolDownTime;
            }
        }
    
    }
    
    public void StudentWalkAcross()
    {
        if (!canMove)
        {
            return;
        }
        animator.SetBool("isWalking", true);
        childObject.transform.localPosition = Vector3.zero;
        transform.localPosition=Vector3.MoveTowards(transform.localPosition,transform.localPosition+transform.forward, Time.deltaTime *  .5f);
        WaitAndStartOverCoRoutine=StartCoroutine(WaitAndStartOver());
        
    }
    public IEnumerator WaitAndStartOver()
    {

        yield return new WaitForSeconds(2f);
        while (!canMove)
        {
            yield return null;
        }
        childObject.transform.localPosition = Vector3.zero;
        coolingdown = true;
        transform.position = startPos;
        transform.rotation = startRot;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        animator.SetBool("isWalking", false);

    }

    public void SetStartPos()
    {
        transform.position = startPos;
        transform.rotation = startRot;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        animator.SetBool("isWalking", false);
    }
}
