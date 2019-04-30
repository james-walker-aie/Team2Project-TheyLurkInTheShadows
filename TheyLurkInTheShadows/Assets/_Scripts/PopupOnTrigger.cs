using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupOnTrigger : MonoBehaviour
{

    public GameObject spawnedObject;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            spawnedObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            spawnedObject.SetActive(false);
        }
    }

}
