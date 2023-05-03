using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class GemGenerator : MonoBehaviour
{
    public GameObject prefab;
    public SplineComputer WhichSplineNow;
    public int MaxGem = 10;
    [HideInInspector] public int CurrentGem = 0;
    public float CooldownTemp = 1f;
    private float Cooldown = 1f;
    private float t = 0;

    private void Start()
    {
        MaxGem = PlayerPrefs.GetInt("RoadGemAmount", 10);
    }
    void Update()
    {
        Cooldown = (1f/MaxGem) * CooldownTemp;

        t -= Time.deltaTime;
        if(CurrentGem < MaxGem && t < 0)
        {
            GameObject temp = Instantiate(prefab);
            temp.transform.position = new Vector3(9999, 9999, 9999);
            temp.GetComponent<RandomPointForSpline>().Spline(WhichSplineNow);
            t = Cooldown;
            CurrentGem++;
        }
    }
}
