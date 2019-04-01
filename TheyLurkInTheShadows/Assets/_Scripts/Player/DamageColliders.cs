using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliders : MonoBehaviour
{

    public float damage;
    public GameObject damageCollider;


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

            if (GetComponentInParent<PController>().isBackstabbing == true)
            {
                other.gameObject.GetComponent<EnemyController>().health -= damage;
                damageCollider.SetActive(false);

            }

        }
    }

    private AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

}
