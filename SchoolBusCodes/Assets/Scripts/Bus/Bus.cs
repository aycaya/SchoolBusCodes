using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : MonoBehaviour
{
    float explosionForce = 0.75f;
    float explosionRadius = 6f;
    public bool playerChange = false;
    public int childCount = 0;
    public int currentBusCapacity = 2;
    [SerializeField] List<GameObject> seats = new List<GameObject>();
    int count;
    bool canChange = false;
    StudentPickUpAndDropOffAnims studentPickUpAndDropOffAnims;
    public int studentMoney;
    public int vipMoney;
    public int busCapMax = 2;
    BusCapacityWriter busCapacityWriter;

    private void Awake()
    {
        studentMoney = PlayerPrefs.GetInt("StudentMoney", 1);
        vipMoney = PlayerPrefs.GetInt("VipMoney", 3);
        currentBusCapacity = PlayerPrefs.GetInt("BusCapacity", 2);
    }
    void Start()
    {
        busCapMax = currentBusCapacity;

        busCapacityWriter = FindObjectOfType<BusCapacityWriter>();

    }

    public void SetCurrentCapacity()
    {
        currentBusCapacity = PlayerPrefs.GetInt("BusCapacity", 2);
       currentBusCapacity -= childCount;

    }
    public void SetMoney()
    {
        studentMoney = PlayerPrefs.GetInt("StudentMoney", 1);
        vipMoney = PlayerPrefs.GetInt("VipMoney", 3);
    }
    public void StudentDrop()
    {
        if (childCount > 0)
        {
            count = childCount;
            int division= Mathf.CeilToInt((float)count / 2f);
            childCount -= division;
            currentBusCapacity += division;
            busCapacityWriter.UpdateText();
            CarAccidentExplosion(division);
        }
       
       
    }
    void CarAccidentExplosion(int param)
    {
        for(int i = 0; i < param ; i++)
        {
            int index = count - i - 1;
            if (seats[index].transform.childCount > 0)
            {
                GameObject passengerObject = seats[index].transform.GetChild(0).gameObject;
                Destroy(passengerObject, 2f);
                Rigidbody rb= passengerObject.GetComponent<Rigidbody>();
                OpenRagdoll ragdoll = passengerObject.GetComponentInChildren<OpenRagdoll>();
                ragdoll.EnableRagdoll();
                Destroy(rb);
                Destroy(passengerObject.GetComponent<Collider>());
                seats[index].transform.GetChild(0).transform.SetParent(null);
                AddExplosionForceToRagdoll(passengerObject);
            }
           
        }

        
    }
    private void AddExplosionForceToRagdoll(GameObject pedesterian)
    {
        Vector3 explosionPos = pedesterian.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && rb.gameObject.CompareTag("Ragdoll"))
            {
                rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, 1.0F, ForceMode.VelocityChange);
            }
        }
    }
}
