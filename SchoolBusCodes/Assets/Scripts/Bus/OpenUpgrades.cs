using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class OpenUpgrades : MonoBehaviour
{
    UpgradeSystem upgradeSystem;
    float t = 0.5f;
    public bool isItOn = false;
    public float cooldownForOpening = 0.5f;
    SplineFollower CurrentBus;
    private void Start()
    {
        upgradeSystem = GameObject.FindObjectOfType<UpgradeSystem>();
    }
    private void Update()
    {
        if (isItOn) { t -= Time.deltaTime; }

        if (isItOn && t < 0 && CurrentBus.followSpeed < 0.1f)
        {
            upgradeSystem.MenuOpen = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CurrentBus = other.gameObject.GetComponent<SplineFollower>();
            isItOn = true;
            t = cooldownForOpening;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isItOn = false;
            upgradeSystem.MenuOpen = false;
        }
    }
}
