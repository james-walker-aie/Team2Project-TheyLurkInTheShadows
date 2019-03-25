using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkParticles : MonoBehaviour
{

    public GameObject particlePrefab;
    public Transform particleTargetLocation;

    public AudioClip blinkSound;

    private AudioSource source;

    public float volume;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    /*void Update()
    {

        if(GetComponent<Blink>().invalid == false)
        {
            if (Input.GetKeyDown(KeyCode.F) == true)
            {
                if (GetComponent<Blink>().currentTime > 5 && GetComponent<Blink>().distance < GetComponent<Blink>().MaxRange)
                {
                    GameObject particleInstance;
                    particleInstance = Instantiate(particlePrefab, particleTargetLocation) as GameObject;
                    source.PlayOneShot(blinkSound, volume);
                }
            }
        }
    }*/

    public void BlinkParticleTrigger()
    {
        GameObject particleInstance;
        particleInstance = Instantiate(particlePrefab, particleTargetLocation) as GameObject;
        source.PlayOneShot(blinkSound, volume);
    }
}
