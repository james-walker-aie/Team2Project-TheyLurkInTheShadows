using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerDetails : MonoBehaviour
{
   // public int level;
    public int health;
    public float[] playerPosition;


    public PlayerDetails(Player player)
    {
       // level = player.level;
        health = player.health;

        playerPosition = new float[3];

        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
        playerPosition[2] = player.transform.position.z;

    }

}
