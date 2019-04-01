using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Image playerHealthIcon;
    [SerializeField] bool isPlayer = false;
    int playerCurrentHealth;
    public int health = 100;
    public int level = 1;

    public void PlayerHealthUI()
    {
        if (isPlayer)
        {
            playerHealthIcon.fillAmount = playerCurrentHealth / health;
        }
        //health icon code to go here
    }

    public void SavePlayer()
    {
        SaveLoadSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerDetails details = SaveLoadSystem.LoadPlayer();

        level = details.level;
        health = details.health;

        Vector3 position;
        position.x = details.playerPosition[0];
        position.y = details.playerPosition[1];
        position.z = details.playerPosition[2];
        transform.position = position;
    }

}
