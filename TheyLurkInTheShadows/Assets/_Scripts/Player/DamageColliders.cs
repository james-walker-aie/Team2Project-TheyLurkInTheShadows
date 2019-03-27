using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliders : MonoBehaviour
{

    public float damage;

    [SerializeField]
    private AudioClip[] clips;

    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Collision has occured");
            if(other.GetComponent<EnemyController>().state != EnemyController.State.Dead)
            {
                other.GetComponent<EnemyController>().state = EnemyController.State.BeingAttacked;
            }

            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);

            //other.gameObject.GetComponent<EnemyController>().health -= damage;
        }
    }

    private AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

}
