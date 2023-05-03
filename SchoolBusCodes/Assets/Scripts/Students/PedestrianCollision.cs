using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;

public class PedestrianCollision : MonoBehaviour
{
    float force = 5f;
    float radius = 3f;
    Bus busScript;
    bool canHit = true;
    [SerializeField] float coolDownTime = 10f;
    float coolDT = 10f;
    AudioPlayer audioPlayer;

    void Start()
    {
        busScript = GetComponent<Bus>();
        audioPlayer = FindObjectOfType<AudioPlayer>();

    }

    void Update()
    {
        if (!canHit)
        {

            coolDT -= Time.deltaTime;

            if (coolDT <= 0)
            {
                canHit = true;
                coolDT = coolDownTime;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Pedestrian"))
        {
            if (GameManager.hapticsSupported)
            {
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
            }
            audioPlayer.PlayHitPedestrianSound();

            StudentCrosswalk studentCrosswalk = collision.GetComponent<StudentCrosswalk>();
            studentCrosswalk.canMove = false;
            studentCrosswalk.coolDT = 10f;
            studentCrosswalk.coolingdown = true;
            OpenRagdoll ragdoll = collision.gameObject.GetComponentInChildren<OpenRagdoll>();
            ragdoll.EnableRagdoll();
            collision.gameObject.GetComponent<Collider>().enabled = false;
           
            if (canHit)
            {
                canHit = false;

                busScript.StudentDrop();

            }

            AddExplosionForceToRagdoll(collision.gameObject);

            StartCoroutine(WaitAndDisable(ragdoll));
        }
    }
    IEnumerator WaitAndDisable(OpenRagdoll ragdoll)
    {
        yield return new WaitForSeconds(2f);
        StudentCrosswalk studentCrosswalk = ragdoll.transform.parent.GetComponent<StudentCrosswalk>();
        studentCrosswalk.SetStartPos();
        ragdoll.transform.parent.gameObject.GetComponent<Collider>().enabled = true;
        ragdoll.DisableRagdoll();
        studentCrosswalk.canMove = true;
    }

    private void AddExplosionForceToRagdoll(GameObject pedesterian)
    {
        Vector3 direction = pedesterian.transform.position - transform.position;
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && rb.gameObject.CompareTag("Ragdoll"))
            {
                rb.AddExplosionForce(force, explosionPos, radius, 1.0F,ForceMode.Impulse);
            }
        }
    }
}
