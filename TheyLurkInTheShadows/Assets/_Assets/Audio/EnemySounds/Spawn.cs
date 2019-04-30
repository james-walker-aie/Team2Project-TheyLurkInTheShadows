using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    
    public GameObject[] spawnPoints;

    public GameObject enemyType;




    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            for(int i = 0; i < spawnPoints.Length; i++)
            {
                Instantiate(enemyType, spawnPoints[i].transform.position,enemyType.transform.localRotation);
            }
        }
    }
}
