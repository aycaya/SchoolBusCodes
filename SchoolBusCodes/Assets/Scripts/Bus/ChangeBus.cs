using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBus : MonoBehaviour
{
    public bool playerChange = false;
    
    public bool canChange = false;
   
    [SerializeField] GameObject Bus1;
    [SerializeField] GameObject Bus2;
    [SerializeField] GameObject Bus3;
    BusStop[] busStops;
    VipStudentsPick[] vipStudentsPicks;
    StudentPickUpAndDropOffAnims[] pickUps;
    CameraFollow cameraFollow;
    SchoolStation schoolStation;
    Bus busScript;
    int whichbus;
    int temp;
    BusCapacityWriter busCapacityWriter;
    PlayerFollower playerFollower;
    private void Awake()
    {
        playerFollower = FindObjectOfType<PlayerFollower>();
    }
    void Start()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
        schoolStation = FindObjectOfType<SchoolStation>();
        busScript = FindObjectOfType<Bus>();
        busCapacityWriter = FindObjectOfType<BusCapacityWriter>();
        whichbus = PlayerPrefs.GetInt("ChangeBus", 0);
        if (whichbus > 0)
        {
            busScript = FindObjectOfType<Bus>();

            ChangeStarterPlayer(whichbus);

        }
       


    }

    void ChangeStarterPlayer(int param)
    {
        busStops = FindObjectsOfType<BusStop>();
        pickUps = FindObjectsOfType<StudentPickUpAndDropOffAnims>();
        vipStudentsPicks = FindObjectsOfType<VipStudentsPick>();
        if (param == 1)
        {
            Bus2.SetActive(true);
            Bus1.SetActive(false);
            Bus3.SetActive(false);

        }
        else if (param == 2)
        {
            Bus3.SetActive(true);

            Bus2.SetActive(false);
            Bus1.SetActive(false);


        }
        busScript = FindObjectOfType<Bus>();

        foreach (BusStop busstp in busStops)
        {
            busstp.ChangePlayer();
        }
        foreach (StudentPickUpAndDropOffAnims picks in pickUps)
        {
            picks.ChangePlayer();
        }
        foreach (VipStudentsPick vips in vipStudentsPicks)
        {
            vips.ChangePlayer();
        }

        cameraFollow.ChangePlayer();
        schoolStation.ChangePlayer();
        busCapacityWriter.ChangePlayer();
        playerFollower.ChangeBus();

    }
    public void ChangeBusFunc(int param)
    {

        busStops = FindObjectsOfType<BusStop>();
        pickUps = FindObjectsOfType<StudentPickUpAndDropOffAnims>();
        vipStudentsPicks = FindObjectsOfType<VipStudentsPick>();



        if (param == 1)
        {
            canChange = false;
             temp = busScript.childCount;

            Bus2.SetActive(true);
            Bus1.SetActive(false);
            Bus3.SetActive(false);
            //Bus4.SetActive(false);


            Bus2.transform.position = Bus1.transform.position;
        }
        else if (param == 2)
        {
            canChange = false;
             temp = busScript.childCount;

            Bus3.SetActive(true);

            Bus2.SetActive(false);
            Bus1.SetActive(false);
            //Bus4.SetActive(false);


            Bus3.transform.position = Bus2.transform.position;
        }
       
        busScript = FindObjectOfType<Bus>();

        foreach (BusStop busstp in busStops)
        {
            busstp.ChangePlayer();
        }
        foreach (StudentPickUpAndDropOffAnims picks in pickUps)
        {
            picks.ChangePlayer();
        }
        foreach (VipStudentsPick vips in vipStudentsPicks)
        {
            vips.ChangePlayer();
        }

        busScript.childCount = temp;
        busScript.currentBusCapacity -= temp;
        cameraFollow.ChangePlayer();
        schoolStation.ChangePlayer();
        busCapacityWriter.ChangePlayer();
        playerFollower.ChangeBus();
    }
   
}
