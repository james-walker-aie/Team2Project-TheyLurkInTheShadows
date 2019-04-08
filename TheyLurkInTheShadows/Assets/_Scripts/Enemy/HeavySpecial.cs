using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySpecial : MonoBehaviour
{
    bool hit;
    float timer;

    private void Update()
    {
        if (hit)
            timer -= Time.deltaTime;
        if (timer <= 0)
            hit = false;
    }

    private void OnParticleCollision(GameObject other)
    {

        

        if (other.tag == "Player")
        {
            //gameObject.GetComponent<ParticleSystem>().Pause();
            gameObject.GetComponent<ParticleSystem>().Stop();
            Debug.Log("Collision");
            if (!hit)
            {
                other.GetComponent<PController>().health -= 30;
                hit = true;
                timer = 1.5f;
            }
                
            
            Debug.Log("Stopped");
        }
        
    }

    private void OnParticleTrigger()
    {
        
    }

}
