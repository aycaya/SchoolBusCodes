using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartForceForCoin : MonoBehaviour
{
    Rigidbody rb;
    public float ForceAmount;
    GameObject Target;
    float timer = 1f;
    float timerForDie;
    [HideInInspector] public float RowNumber = 1;
    bool Trigger = true;

    void Start()
    {
        Target = GameObject.Find("Player").gameObject;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(gameObject.transform.position.x + Random.Range(-ForceAmount * 1.2f, ForceAmount * 1.2f), gameObject.transform.position.y + ForceAmount * 1.5f, gameObject.transform.position.z + Random.Range(-ForceAmount * 1.2f, ForceAmount * 1.2f)));
        rb.AddTorque(transform.up * 20);
        timerForDie = 10;
    }

    void Update()
    {
        if (Trigger) { Trigger = false; timer += (RowNumber / 10f); }

        if (timer < 0 && Target.gameObject.name == "Player" && Vector3.Distance(transform.position, Target.transform.position) < 5f)
        {
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, new Vector3(Target.gameObject.transform.position.x, Target.gameObject.transform.position.y + 1.5f, Target.gameObject.transform.position.z), Time.deltaTime * 5);
            gameObject.transform.localScale = Vector3.Slerp(gameObject.transform.localScale, gameObject.transform.localScale / 2f, Time.deltaTime * 5);
            gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y + 30, gameObject.transform.localEulerAngles.z);
        }

    }
}
