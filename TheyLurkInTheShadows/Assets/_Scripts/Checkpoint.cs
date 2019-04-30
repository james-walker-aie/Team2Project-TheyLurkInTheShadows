using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private SaveMaster saveMas;

    void Start()
    {
        saveMas = GameObject.FindGameObjectWithTag("saveMas").GetComponent<SaveMaster>();
    }

    public void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player"))
        {
            saveMas.checkpointPos = transform.position;
            PlayerPrefs.SetString("LastSceneLoaded", SceneManager.GetActiveScene().name);

            Debug.Log("Checkpoint hit");
        }

    }
}
