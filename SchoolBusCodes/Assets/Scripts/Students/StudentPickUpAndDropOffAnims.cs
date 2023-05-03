using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Lofelt.NiceVibrations;

public class StudentPickUpAndDropOffAnims : MonoBehaviour
{
    List<GameObject> studentsToPick ;
    GameObject objectParent;
    bool[] isSeatTaken;
    bool isCoroutineStarted = false;
    public bool canPickUp = false;
    GameObject stud;
    Transform bus;
    [SerializeField] List<GameObject> seats = new List<GameObject>();
    List<GameObject> temp = new List<GameObject>();
    BusStop busStop;
    List<GameObject> remove= new List<GameObject>();
    bool changeScale = false;
    [SerializeField] float maxSize=1.5f;
    float growFactor=20f;
    [SerializeField] float waitTime=.1f;
    AudioPlayer audioPlayer;
    GameObject SchoolKapi;
    public Vector3 OkulKapisiOffset;
    Transform Canvas;
    [SerializeField] GameObject moneyPrefab;

    public float coinUpwardsModifier = 1f;
    float flightTime = .5f;

    void Start()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
        objectParent = GameObject.FindWithTag("Player").transform.GetChild(2).gameObject;
        SchoolKapi = GameObject.Find("School kapi");
        Canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
    }
    public void RefreshSeatCount()
    {
        isSeatTaken = new bool[objectParent.transform.childCount];
    }
    public void BusStopStudents(List<GameObject> students, Transform param)
    {
        canPickUp = true;
        bus = param;
        studentsToPick = students;
    }
    public void ChangePlayer()
    {
        foreach(GameObject std in seats)
        {
            if (std.transform.childCount > 0)
            {
                temp.Add(std.transform.GetChild(0).gameObject);
            }
        }
        seats.Clear();

        objectParent = GameObject.FindWithTag("Player").transform.GetChild(3).gameObject;
        for(int i = 0; i < objectParent.transform.childCount;i++)
        {
            seats.Add(objectParent.transform.GetChild(i).gameObject);
        }
        if (temp.Count > 0)
        {
           for(int i = 0; i < temp.Count; i++)
            {
                temp[i].transform.SetParent(seats[i].transform);

                temp[i].transform.localScale = new Vector3(1, 1, 1);

                temp[i].transform.localPosition = Vector3.zero;

                temp[i].transform.localEulerAngles = Vector3.zero;

            }
        }
        temp.Clear();
       
    }
    void Update()
    {
        if (!isCoroutineStarted && canPickUp)
        {

            isCoroutineStarted = true;
        }
       
    }
    public IEnumerator StudentDropOff(int count)
    {
        if (seats[count - 1] != null)
        {
            
            Vector3 Destination = SchoolKapi.transform.position + OkulKapisiOffset;
            GameObject temp = seats[count - 1].transform.GetChild(0).gameObject;
            Vector3 initialPoint = temp.transform.position;
            Vector3 direction = (Destination - initialPoint).normalized;

            float rawInitialDistance = Vector3.Distance(Destination, initialPoint);
            Quaternion look = Quaternion.LookRotation(direction);
            temp.transform.rotation = look;
            float timer = 0f;
            while (true)
            {
                timer += Time.deltaTime;

                if (Vector3.Distance(temp.transform.position, Destination) < 0.1)
                {
                    Instantiate(moneyPrefab, Canvas.transform);
                    Destroy(temp);
                    yield break;
                }

                float actualDistance = Vector3.Distance(Destination, temp.transform.position);
                float normalizedCurrentDistance = timer / flightTime;
                float desiredHeightOffset;
                if (normalizedCurrentDistance <= 0.5f)
                {
                    desiredHeightOffset = Mathf.Lerp(0f, coinUpwardsModifier, (Mathf.Sin((normalizedCurrentDistance * 2f) * Mathf.PI / 2f)));
                }
                else
                {
                    float correctedRatio = (normalizedCurrentDistance - 0.5f) * 2f;
                    desiredHeightOffset = Mathf.Lerp(coinUpwardsModifier, 0f, correctedRatio * correctedRatio);
                }
                Vector3 desiredDestiantion = Destination + new Vector3(0f, desiredHeightOffset, 0f);
                temp.transform.position = Vector3.Lerp(initialPoint, desiredDestiantion, normalizedCurrentDistance);
                yield return null;
            }
        }

        

    }
    public IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3f);
        remove.Clear();

    }

    public void PickStudent(GameObject param,int count)
    {
       
        changeScale = true;
       

        param.transform.SetParent(seats[count-1].transform);
        param.transform.localScale = Vector3.zero;
        StartCoroutine(ChangeScale(param));
        param.transform.localPosition = Vector3.zero;

        param.transform.localEulerAngles = Vector3.zero;
        changeScale = false;


    }
    public IEnumerator ChangeScale(GameObject param)
    {
        float timer = 0f;
        while (changeScale)
        {
           

            while (maxSize >= (param.transform.localScale.x+0.0001f))
            {
                timer += Time.deltaTime;
                float yScale = param.transform.localScale.y;
                yScale += Time.deltaTime * growFactor;
                yScale = Mathf.Clamp(yScale, 1f, maxSize);
                param.transform.localScale = new Vector3(yScale, yScale, yScale);
                yield return null;
            }

            timer = 0f;
            while (1.0001f <= param.transform.localScale.x)
            {
                timer += Time.deltaTime;
                float yScale = param.transform.localScale.y;
                yScale -= Time.deltaTime * growFactor;
                yScale = Mathf.Clamp(yScale, 1f, maxSize);
                param.transform.localScale = new Vector3(yScale, yScale, yScale);
                yield return null;
            }

            timer = 0f;
            yield return new WaitForSeconds(waitTime);
        }
        audioPlayer.PlayCharacterSound();
        if (GameManager.hapticsSupported)
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
        param.transform.localScale = new Vector3(1, 1, 1);
    }
    public void NewSeatParent(GameObject parnt)
    {
        objectParent = parnt.transform.GetChild(3).gameObject;
    }
   
    }

