using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliders : MonoBehaviour
{

    public float damage;


    void Start()
    {
        
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
            
            other.gameObject.GetComponent<EnemyController>().health -= damage;
        }
    }

}
