using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkParticles : MonoBehaviour
{

    public GameObject particlePrefab;
    public Transform particleTargetLocation;

    void Update()
    {
            

        if (Input.GetKeyDown(KeyCode.F) == true)
        {
            GameObject particleInstance;
            particleInstance = Instantiate(particlePrefab, particleTargetLocation) as GameObject;
        }


    }
}
