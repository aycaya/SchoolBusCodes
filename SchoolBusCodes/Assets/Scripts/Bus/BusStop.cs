using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField] SplineFollower busFolllower;
    
    bool inStation = false;
    bool pickupChild = false;
    bool isCoroutineStarted = false;
    Bus busScript;
    StudentPickUpAndDropOffAnims studentPickUpAndDropOffAnims;

    public int StudentAmount = 0;
    public int MaxStudentAmount = 1;
    public float cooldownForStudentGeneration = 30;
    private float timerForStudentGeneration = 0;
    public GameObject[] OgrenciPrefablari;
    public GameObject[] OgrenciKonumlari;

    private CardMenu cardMenu;

    private GameObject[] TempStudents;
    GameObject player;
    BusCapacityWriter busCapacityWriter;
    Animator animator;
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        busScript = FindObjectOfType<Bus>();
        studentPickUpAndDropOffAnims = GetComponent<StudentPickUpAndDropOffAnims>();
        cardMenu = GameObject.FindObjectOfType<CardMenu>();
        MaxStudentAmount = cardMenu.CardBuffAmount[2] + 1;
        TempStudents = new GameObject[20];
        busCapacityWriter = FindObjectOfType<BusCapacityWriter>();

    }

    void Update()
    {
        MaxStudentAmount = cardMenu.CardBuffAmount[2] + 1;
        cooldownForStudentGeneration = 30 / (cardMenu.CardBuffAmount[2] + 1);

        timerForStudentGeneration -= Time.deltaTime;
        if (timerForStudentGeneration < 0)
        {
            timerForStudentGeneration = cooldownForStudentGeneration;
            if(MaxStudentAmount > StudentAmount)
            {
                StudentAmount++;
                TempStudents[StudentAmount] = Instantiate(OgrenciPrefablari[Random.Range(0, 2)], OgrenciKonumlari[StudentAmount].transform.position, OgrenciKonumlari[StudentAmount].transform.rotation);
            }
        }
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
            studentPickUpAndDropOffAnims.PickStudent(TempStudents[StudentAmount], busScript.childCount);
            StudentAmount--;
            busCapacityWriter.UpdateText();

        }


    }
    public void ChangePlayer()
    {
        player = GameObject.FindWithTag("Player");
        busFolllower = player.GetComponent<SplineFollower>();
        busScript = FindObjectOfType<Bus>();
     
    }
    IEnumerator WaitAndCheckCapacity()
    {
        while (inStation)
        {
            if(TempStudents[StudentAmount]==null) 
            {

                yield return null;
                continue ; 
            }

            if (Mathf.Abs(busFolllower.followSpeed) <= 0.1f && CapacityControl())
            {
                float counter = 1f;
                Vector3 initialPos = TempStudents[StudentAmount].transform.position;
                Vector3 direction = (player.transform.GetChild(0).transform.position - initialPos).normalized;
                Quaternion rot = TempStudents[StudentAmount].transform.rotation;
                animator = TempStudents[StudentAmount].GetComponent<Animator>();
                animator.SetBool("isWalking", true);
                while (counter > 0f)
                {

                    if (TempStudents[StudentAmount] == null)
                    {

                        yield break;
                        
                    }
                   
                    counter -= Time.deltaTime;
                    Quaternion look = Quaternion.LookRotation((player.transform.GetChild(0).transform.position - initialPos).normalized);
                    TempStudents[StudentAmount].transform.rotation = look;
                    TempStudents[StudentAmount].transform.position = Vector3.Lerp(initialPos, player.transform.GetChild(0).transform.position, 1-counter);
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
