using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySpecial : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        
        if (other.tag == "Player")
        {
            //gameObject.GetComponent<ParticleSystem>().Pause();
            gameObject.GetComponent<ParticleSystem>().Stop();
            Debug.Log("Collision");
            other.GetComponent<PController>().health -= 20;
            
            Debug.Log("Stopped");
        }
        
    }

    private void OnParticleTrigger()
    {
        
    }

}
