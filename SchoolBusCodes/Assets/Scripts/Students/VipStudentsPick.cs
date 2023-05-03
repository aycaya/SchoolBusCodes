using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VipStudentsPick : MonoBehaviour
{
    [SerializeField] SplineFollower busFolllower;

    bool inStation = false;
    bool pickupChild = false;
    bool isCoroutineStarted = false;
    Bus busScript;
    StudentPickUpAndDropOffAnims studentPickUpAndDropOffAnims;
    GameObject player;


    public int StudentAmount = 0;
    public int MaxStudentAmount = 2;
    public float cooldownForStudentGeneration = 6;
    private float timerForStudentGeneration = 0;
    public GameObject[] VipOgrenciPrefablari;
    public GameObject[] VipOgrenciKonumlari;

    private GameObject[] VipTempStudents;
    public int vipStudentCount = 0;
    Animator animator;


    void Start()
    {
        player = GameObject.FindWithTag("Player");

        busScript = FindObjectOfType<Bus>();
        studentPickUpAndDropOffAnims = GetComponent<StudentPickUpAndDropOffAnims>();
        VipTempStudents = new GameObject[MaxStudentAmount +3];
    }

    void Update()
    {
        timerForStudentGeneration -= Time.deltaTime;
        if (timerForStudentGeneration < 0)
        {
            timerForStudentGeneration = cooldownForStudentGeneration;
            if (MaxStudentAmount > StudentAmount)
            {
                StudentAmount++;
                VipTempStudents[StudentAmount] = Instantiate(VipOgrenciPrefablari[Random.Range(0, 2)], VipOgrenciKonumlari[StudentAmount].transform.position, VipOgrenciKonumlari[StudentAmount].transform.rotation);
            }
        }
    }
    public void ChangePlayer()
    {
        player = GameObject.FindWithTag("Player");
        busFolllower = player.GetComponent<SplineFollower>();

        busScript = FindObjectOfType<Bus>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger != true && other.CompareTag("Player"))
        {
            inStation = true;
            if (!isCoroutineStarted)
            {
                isCoroutineStarted = true;
                StartCoroutine(WaitAndCheckCapacity());

            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger != true && other.CompareTag("Player"))
        {
            inStation = false;
            isCoroutineStarted = false;

        }
    }

    bool CapacityControl()
    {
        if (busScript.currentBusCapacity > 0)
            return true;
        else
            return false;
    }
    void StudentPickUp()
    {
        if (!pickupChild && StudentAmount > 0)
        {
            pickupChild = true;
            busScript.currentBusCapacity--;
            busScript.childCount++;
            studentPickUpAndDropOffAnims.PickStudent(VipTempStudents[StudentAmount], busScript.childCount);
            StudentAmount--;
            vipStudentCount++;
        }


    }
    IEnumerator WaitAndCheckCapacity()
    {
        while (inStation)
        {
         
            if (VipTempStudents[StudentAmount] == null)
            {

                yield return null;
                continue;
            }
            if (Mathf.Abs(busFolllower.followSpeed) <= 0.1f && CapacityControl())
            {
                float counter = 1f;
                Vector3 initialPos = VipTempStudents[StudentAmount].transform.position;

                Vector3 direction = (player.transform.GetChild(0).transform.position - initialPos).normalized;
                Quaternion rot = VipTempStudents[StudentAmount].transform.rotation;

                animator = VipTempStudents[StudentAmount].GetComponent<Animator>();
                animator.SetBool("isWalking", true);
                while (counter > 0f)
                {
                    if (VipTempStudents[StudentAmount] == null)
                    {

                        yield break;

                    }


                    counter -= Time.deltaTime;
                    Quaternion look = Quaternion.LookRotation((player.transform.GetChild(0).transform.position - initialPos).normalized);
                    VipTempStudents[StudentAmount].transform.rotation = look;
                    VipTempStudents[StudentAmount].transform.position = Vector3.Lerp(initialPos, player.transform.GetChild(0).transform.position, 1 -counter);

                    yield return null;
                }
                StudentPickUp();
                pickupChild = false;
                animator.SetBool("isWalking", false);

            }
            yield return null;
        }
    }
}
