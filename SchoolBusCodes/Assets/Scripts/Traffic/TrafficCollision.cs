using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;

public class TrafficCollision : MonoBehaviour
{
    public bool isCollide = false;
    Bus busScript;
    GameObject CrashVfxOBJ;
    ParticleSystem[] CrashVfx;
    AudioPlayer audioPlayer;


    void Start()
    {
        busScript = FindObjectOfType<Bus>();
        CrashVfxOBJ = GameObject.Find("Crash");
        CrashVfx = CrashVfxOBJ.GetComponentsInChildren<ParticleSystem>();
        audioPlayer = FindObjectOfType<AudioPlayer>();

    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            if (GameManager.hapticsSupported)
            {
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
            }
            audioPlayer.PlayCarCrashSound();

            CrashVfxOBJ.transform.position = (other.gameObject.transform.position + gameObject.transform.position) / 2;
            foreach (ParticleSystem p in CrashVfx)
            {
                p.Play();
            }

            Vector3 dest = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            GameObject cloneObject = Instantiate(other.gameObject, dest, Quaternion.identity);
            BoxCollider boxCol = cloneObject.GetComponent<BoxCollider>();
            boxCol.isTrigger = false;
            MeshRenderer[] meshes = other.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer mesh in meshes)
            {
                mesh.enabled = false;

            }
            Rigidbody carRb = cloneObject.GetComponent<Rigidbody>();
            Destroy(cloneObject.GetComponent<SplineFollower>());


            Vector3 direction = transform.position - cloneObject.transform.position;
            carRb.AddForce(-direction.normalized, ForceMode.VelocityChange);
            carRb.AddTorque(Vector3.one *100f * Time.deltaTime,ForceMode.VelocityChange);
            if (!isCollide)
            {
                busScript.StudentDrop();
            }
            isCollide = true;
            StartCoroutine(WaitAndEnableMesh(meshes, cloneObject));
        }
    }
    
    IEnumerator WaitAndEnableMesh(MeshRenderer[] other, GameObject clone)
    {
        yield return new WaitForSeconds(2);
        foreach (MeshRenderer mesh in other)
        {
            mesh.enabled = true;

        }
       
        Destroy(clone);
        isCollide = false;

    }
 
}
