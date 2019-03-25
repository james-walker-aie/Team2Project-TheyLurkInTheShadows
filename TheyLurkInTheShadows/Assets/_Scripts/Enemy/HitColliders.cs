using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColliders : MonoBehaviour
{

    public bool Heavy2;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GetComponentInParent<EnemyController>().hit = true;
        }

        if(GetComponentInParent<EnemyController>().Class == EnemyController.Type.Heavy)
        {
            Debug.Log("InHeavy");
            if (Heavy2)
            {

                GetComponentInParent<EnemyController>().hit = true;

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<EnemyController>().hit = true;
        }
        if (GetComponentInParent<EnemyController>().Class == EnemyController.Type.Heavy)
        {
            if (Heavy2)
            {
                if (Heavy2)
                {

                    GetComponentInParent<EnemyController>().hit = true;

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<EnemyController>().hit = false;
        }
        if (GetComponentInParent<EnemyController>().Class == EnemyController.Type.Heavy)
        {
            if (Heavy2)
            {
                if (Heavy2)
                {

                    GetComponentInParent<EnemyController>().hit = true;

                }
            }
        }
    }
}
