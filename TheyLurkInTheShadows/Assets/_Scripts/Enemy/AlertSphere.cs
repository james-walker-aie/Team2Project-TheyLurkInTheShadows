using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSphere : MonoBehaviour
{
    public bool completed = false;
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Bush")
        {
            
            if (gameObject.GetComponentInParent<EnemyController>().hidingSpots.Count == 0)
            {
                gameObject.GetComponentInParent<EnemyController>().hidingSpots.Add(other.transform);
            }
            else
            {
                for (int i = 0; i < gameObject.GetComponentInParent<EnemyController>().hidingSpots.Count; i++)
                {
                    if (gameObject.GetComponentInParent<EnemyController>().hidingSpots[i] != other.transform)
                    {
                        
                        gameObject.GetComponentInParent<EnemyController>().hidingSpots.Add(other.transform);
                        
                    }
                }
            }

        }

        for (int j = 0; j < gameObject.GetComponentInParent<EnemyController>().hidingSpots.Count; j++)
        {
            for (int k = 0; k < gameObject.GetComponentInParent<EnemyController>().hidingSpots.Count; k++)
            {
                if (k != j)
                {
                    if (gameObject.GetComponentInParent<EnemyController>().hidingSpots[j] == gameObject.GetComponentInParent<EnemyController>().hidingSpots[k])
                    {
                        gameObject.GetComponentInParent<EnemyController>().hidingSpots.Remove(gameObject.GetComponentInParent<EnemyController>().hidingSpots[j]);
                    }
                }

            }
        }

        if (other.tag == "Enemy")
        {

            if (gameObject.GetComponentInParent<EnemyController>().nearByEnemies.Count == 0)
            {
                gameObject.GetComponentInParent<EnemyController>().nearByEnemies.Add(other.transform);
            }
            else
            {
                for (int i = 0; i < gameObject.GetComponentInParent<EnemyController>().nearByEnemies.Count; i++)
                {
                    if (gameObject.GetComponentInParent<EnemyController>().nearByEnemies[i] != other.transform)
                    {

                        gameObject.GetComponentInParent<EnemyController>().nearByEnemies.Add(other.transform);

                    }
                }
            }

        }

        for (int j = 0; j < gameObject.GetComponentInParent<EnemyController>().nearByEnemies.Count; j++)
        {
            for (int k = 0; k < gameObject.GetComponentInParent<EnemyController>().nearByEnemies.Count; k++)
            {
                if (k != j)
                {
                    if (gameObject.GetComponentInParent<EnemyController>().nearByEnemies[j] == gameObject.GetComponentInParent<EnemyController>().nearByEnemies[k])
                    {
                        gameObject.GetComponentInParent<EnemyController>().nearByEnemies.Remove(gameObject.GetComponentInParent<EnemyController>().nearByEnemies[j]);
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
        if(other.tag == "Enemy")
        {
            other.gameObject.GetComponentInParent<EnemyController>().alerted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Bush")
        {
            if (gameObject.GetComponentInParent<EnemyController>().hidingSpots.Contains(other.transform))
                gameObject.GetComponentInParent<EnemyController>().hidingSpots.Remove(other.transform);
        }

        if(other.tag == "Enemy")
        {
            if (gameObject.GetComponentInParent<EnemyController>().nearByEnemies.Contains(other.transform))
                gameObject.GetComponentInParent<EnemyController>().nearByEnemies.Remove(other.transform);
            other.gameObject.GetComponentInParent<EnemyController>().alerted = false;
        }
    }
}

    

