using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BusCapacityWriter : MonoBehaviour
{
    UpgradeSystem upgradeSystem;
    public TextMeshProUGUI CapacityText;
     Bus bus;
    private int maxCapacity = 1;
    public int multiplier = 1;
    GameObject player;
    [SerializeField] Vector3 offset;
    RectTransform rectTransform;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bus = FindObjectOfType<Bus>();
        rectTransform = GetComponent<RectTransform>();
        transform.SetParent(FindObjectOfType<PlayerFollower>().transform);
        maxCapacity =bus.busCapMax;
        rectTransform.anchoredPosition3D = Vector3.zero;
        CapacityText.text = bus.childCount.ToString() + "/" + PlayerPrefs.GetInt("BusCapacity", 2).ToString();

    }
   
    public void UpdateText()
    {
        CapacityText.text = bus.childCount.ToString() + "/" + PlayerPrefs.GetInt("BusCapacity", 2).ToString();
    }
    public void ChangePlayer()
    {
        
        bus = FindObjectOfType<Bus>();
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        rectTransform.anchoredPosition3D = Vector3.zero;



    }
    private void Update()
    {
        CapacityText.text = bus.childCount.ToString() + "/" + PlayerPrefs.GetInt("BusCapacity", 2).ToString();

    }
}
