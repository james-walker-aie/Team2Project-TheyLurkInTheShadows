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
                if (other.GetComponent<PriestController>())
                {
                    other.gameObject.GetComponent<PriestController>().health = 0;
                }
                else
                {
                    other.gameObject.GetComponent<EnemyController>().health = 0;
                }
                
            }

            if (other.GetComponent<PriestController>())
            {
                other.GetComponent<PriestController>().health -= 35;
                other.GetComponent<PriestController>().hit = true;
                other.GetComponent<PriestController>().state = PriestController.State.Run;
            }
            else
            if(other.GetComponent<EnemyController>().state != EnemyController.State.Dead)
            {
                if (other.GetComponent<EnemyController>().state == EnemyController.State.Guard || other.GetComponent<EnemyController>().state == EnemyController.State.AlertTime || 
                    other.GetComponent<EnemyController>().state == EnemyController.State.Patrol || other.GetComponent<EnemyController>().state == EnemyController.State.Hit)
                {
                    other.GetComponent<EnemyController>().playOnce = true;
                    other.GetComponent<EnemyController>().state = EnemyController.State.Hit;
                }
                else
                if (!GetComponentInParent<PController>().isBackstabbing)
                {
                    other.GetComponent<EnemyController>().state = EnemyController.State.BeingAttacked;
                }
            }
  

            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);

            

            damage = 0;

        }
    }


}
