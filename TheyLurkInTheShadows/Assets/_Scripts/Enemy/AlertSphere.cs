using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSphere : MonoBehaviour
{
    public GameObject enemy;
    public bool completed = false;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "Bush")
        {
            Debug.Log("AlertSphere1");
            if (enemy.GetComponent<EnemyController>().hidingSpots.Count == 0)
            {
                enemy.GetComponent<EnemyController>().hidingSpots.Add(other.transform);
            }
            else
            {
                for (int i = 0; i < enemy.GetComponent<EnemyController>().hidingSpots.Count; i++)
                {
                    if (enemy.GetComponent<EnemyController>().hidingSpots[i] != other.transform)
                    {
                        
                        enemy.GetComponent<EnemyController>().hidingSpots.Add(other.transform);
                        
                    }
                }
            }

        }
        
        for (int j = 0; j < enemy.GetComponent<EnemyController>().hidingSpots.Count; j++)
        {
            for (int k = 0; k < enemy.GetComponent<EnemyController>().hidingSpots.Count; k++)
            {
                if (k != j)
                {
                    if (enemy.GetComponent<EnemyController>().hidingSpots[j] == enemy.GetComponent<EnemyController>().hidingSpots[k])
                    {
                        enemy.GetComponent<EnemyController>().hidingSpots.Remove(enemy.GetComponent<EnemyController>().hidingSpots[j]);
                    }
                }

            }
        }

        if (other.tag == "Enemy")
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
        
        if(other.tag == "Bush")
            Debug.Log("AlertSphere2");
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().alerted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Bush")
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

    

