using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingGemOnRoad : MonoBehaviour
{
    GemGenerator gemGenerator;
    public GameObject Prefab;
    Transform CanvasTr;

    private void Start()
    {
        CanvasTr = GameObject.FindGameObjectWithTag("Canvas").transform;
        gemGenerator = GameObject.FindGameObjectWithTag("GemGenerator").GetComponent<GemGenerator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gemGenerator.CurrentGem--;
            Instantiate(Prefab, CanvasTr.transform);
            Destroy(gameObject);
        }
    }
}
