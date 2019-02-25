using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSpots : MonoBehaviour
{
    public Vector3 rotation;

    private void Awake()
    {
        rotation = transform.rotation.eulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().inGuardSpot = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().inGuardSpot = false;
        }
    }
}
