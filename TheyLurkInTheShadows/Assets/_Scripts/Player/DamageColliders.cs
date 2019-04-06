using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliders : MonoBehaviour
{

    public float damageConst;
    public float damage;
    public GameObject damageCollider;


    [SerializeField]
    private AudioClip[] clips;

    private AudioSource audioSource;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    private AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (GetComponentInParent<PController>().isBackstabbing == true)
            {
                other.gameObject.GetComponent<EnemyController>().health = 0;
            }

            Debug.Log("Collision has occured");
            if(other.GetComponent<EnemyController>().state != EnemyController.State.Dead && !GetComponentInParent<PController>().isBackstabbing)
            {
                other.GetComponent<EnemyController>().state = EnemyController.State.BeingAttacked;
            }

            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);

            

            damage = 0;

        }
    }


}
