using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticAlertSphere : MonoBehaviour
{
    public GameObject enemy;
    public bool completed = false;
    bool playerNearby;
    public GameObject blink;
    public GameObject[] particles;
    public AudioClip nearby;
    AudioSource AS;

    private void Awake()
    {
        AS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player")
        {
            AS.clip = nearby;
            AS.Play();
        }

        if (enemy.GetComponent<EnemyController>().state == EnemyController.State.Dead)
        {
            foreach(GameObject part in particles)
            {
                part.SetActive(false);
            }
        }

        

        if (other.tag == "Enemy")
        {
            if(other.gameObject != enemy)
            {
                if (enemy.GetComponent<EnemyController>().nearByEnemies.Count == 0)
                {
                    enemy.GetComponent<EnemyController>().nearByEnemies.Add(other.transform);
                }
                else
                {
                    for (int i = 0; i < enemy.GetComponent<EnemyController>().nearByEnemies.Count; i++)
                    {
                        if (enemy.GetComponent<EnemyController>().nearByEnemies[i] != other.transform)
                        {

                            enemy.GetComponent<EnemyController>().nearByEnemies.Add(other.transform);

                        }
                    }
                }
            }
            

        }

        for (int j = 0; j < enemy.GetComponent<EnemyController>().nearByEnemies.Count; j++)
        {
            for (int k = 0; k < enemy.GetComponent<EnemyController>().nearByEnemies.Count; k++)
            {
                if (k != j)
                {
                    if (enemy.GetComponent<EnemyController>().nearByEnemies[j] == enemy.GetComponent<EnemyController>().nearByEnemies[k])
                    {
                        enemy.GetComponent<EnemyController>().nearByEnemies.Remove(enemy.GetComponent<EnemyController>().nearByEnemies[j]);
                    }
                }

            }
        }
        if (!completed)
        {
            completed = true;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            
            if (other.gameObject.GetComponent<Blink>().isBlinking == true && enemy.GetComponent<EnemyController>().state != EnemyController.State.Dead)
            {
                
                
                Vector3 dir = (this.transform.position - other.gameObject.transform.position).normalized;
                enemy.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, other.gameObject.transform.position.z) - dir*2;

                RaycastHit hit;

                if(Physics.Raycast(enemy.transform.position,Vector3.down,out hit, Mathf.Infinity))
                {
                    
                    enemy.transform.position = new Vector3(enemy.transform.position.x, hit.point.y, enemy.transform.position.z);

                }
                

                blink.GetComponent<ParticleSystem>().Play();
                enemy.GetComponent<EnemyController>().state = EnemyController.State.Combat;
                foreach(GameObject part in particles)
                {
                    part.transform.position = enemy.transform.position;
                }
                other.gameObject.GetComponent<Blink>().isBlinking = false;
                enemy.GetComponent<EnemyController>().canAttack = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AS.Stop();
        }

        if (other.tag == "Bush")
        {
            if (enemy.GetComponent<EnemyController>().hidingSpots.Contains(other.transform))
                enemy.GetComponent<EnemyController>().hidingSpots.Remove(other.transform);
        }

        if(other.tag == "Enemy")
        {
            if (enemy.GetComponent<EnemyController>().nearByEnemies.Contains(other.transform))
                enemy.GetComponent<EnemyController>().nearByEnemies.Remove(other.transform);
            other.gameObject.GetComponent<EnemyController>().alerted = false;
        }
    }
}

    

