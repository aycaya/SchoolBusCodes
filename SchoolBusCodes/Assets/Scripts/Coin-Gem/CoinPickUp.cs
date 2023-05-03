using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    int currentCoin = 0;
    int currentLevelCoin = 0;
    GameObject coinValue;
    TextMeshProUGUI coinValueText;
    
    void Start()
    {
        currentCoin = PlayerPrefs.GetInt("CoinValues",0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            currentCoin += 1;
            currentLevelCoin += 1;
            PlayerPrefs.SetInt("CoinValues", currentCoin);
           
        }
    }
}
