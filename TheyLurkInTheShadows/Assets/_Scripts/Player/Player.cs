﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    /*[SerializeField] Image playerHealthIcon;
    [SerializeField] bool isPlayer = false;
    int playerCurrentHealth;
    public int health = 100;
    public int level = 1; 


   /* public void SavePlayer()
    {
        SaveLoadSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerDetails details = SaveLoadSystem.LoadPlayer();

        //level = details.level;
        health = details.health;

        Vector3 position;
        position.x = details.playerPosition[0];
        position.y = details.playerPosition[1];
        position.z = details.playerPosition[2];
        transform.position = position;
    }*/

    private SaveMaster saveMas;

    void Start()
    {
        saveMas = GameObject.FindGameObjectWithTag("saveMas").GetComponent<SaveMaster>();
        transform.position = saveMas.checkpointPos;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
