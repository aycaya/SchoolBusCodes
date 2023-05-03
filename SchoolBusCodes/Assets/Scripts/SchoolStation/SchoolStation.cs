using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;

public class SchoolStation : MonoBehaviour
{
    [SerializeField] SplineFollower busFolllower;
   
    bool inStation = false;
    bool dropChild = false;
    Bus busScript;
    bool isCoroutineStarted = false;
   [SerializeField] GameObject moneyPrefab;

    Transform Canvas;
    StudentPickUpAndDropOffAnims studentPickUpAndDropOffAnims;
    BusCapacityWriter busCapacityWriter;
    AudioPlayer audioPlayer;

    void Start()
    {
        busScript = FindObjectOfType<Bus>();
        Canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        studentPickUpAndDropOffAnims = GetComponent<StudentPickUpAndDropOffAnims>();
        busCapacityWriter = FindObjectOfType<BusCapacityWriter>();
        audioPlayer = FindObjectOfType<AudioPlayer>();

    }

    void Update()
    {
      
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger != true && other.CompareTag("Player"))
        {
            inStation = true;
            if (!isCoroutineStarted)
            {
                isCoroutineStarted = true;
                StartCoroutine(WaitAndDropStudents());
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
    public void ChangePlayer()
    {
        busScript = FindObjectOfType<Bus>();
        busFolllower = GameObject.FindGameObjectWithTag("Player").GetComponent<SplineFollower>();


    }
    IEnumerator WaitAndDropStudents()
    {
       yield return new WaitForSeconds(.5f);

        while (inStation)
        {
           
          
                if (Mathf.Abs(busFolllower.followSpeed) <= 0.1f && !dropChild&& busScript.childCount>0)
                {
                  
                    dropChild = true;
                    StartCoroutine(studentPickUpAndDropOffAnims.StudentDropOff(busScript.childCount));

                    busScript.currentBusCapacity++;

                    busScript.childCount--;
                    audioPlayer.PlayCharacterSound();

                    if (GameManager.hapticsSupported)
                    {
                        HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
                    }

                    busCapacityWriter.UpdateText();

                    yield return new WaitForSeconds(.5f);
                    dropChild = false;
                }

            

            yield return null;
        }
    }

}