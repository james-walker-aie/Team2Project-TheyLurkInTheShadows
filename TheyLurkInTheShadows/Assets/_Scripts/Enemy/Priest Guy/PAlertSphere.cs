using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAlertSphere : MonoBehaviour
{
    public GameObject enemy;
    public bool completed = false;
    private void OnTriggerEnter(Collider other)
    {
        
        
        
        

        if (other.tag == "Enemy")
        {

            if (enemy.GetComponent<PriestController>().nearByEnemies.Count == 0)
            {
                enemy.GetComponent<PriestController>().nearByEnemies.Add(other.transform);
            }
            else
            {
                for (int i = 0; i < enemy.GetComponent<PriestController>().nearByEnemies.Count; i++)
                {
                    if (enemy.GetComponent<PriestController>().nearByEnemies[i] != other.transform)
                    {

                        enemy.GetComponent<PriestController>().nearByEnemies.Add(other.transform);

                    }
                }
            }

        }

        for (int j = 0; j < enemy.GetComponent<PriestController>().nearByEnemies.Count; j++)
        {
            for (int k = 0; k < enemy.GetComponent<PriestController>().nearByEnemies.Count; k++)
            {
                if (k != j)
                {
                    if (enemy.GetComponent<PriestController>().nearByEnemies[j] == enemy.GetComponent<PriestController>().nearByEnemies[k])
                    {
                        enemy.GetComponent<PriestController>().nearByEnemies.Remove(enemy.GetComponent<PriestController>().nearByEnemies[j]);
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
        
        
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().alerted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        

        if(other.tag == "Enemy")
        {
            if (enemy.GetComponent<PriestController>().nearByEnemies.Contains(other.transform))
                enemy.GetComponent<PriestController>().nearByEnemies.Remove(other.transform);
            other.gameObject.GetComponent<EnemyController>().alerted = false;
        }
    }
}

    

