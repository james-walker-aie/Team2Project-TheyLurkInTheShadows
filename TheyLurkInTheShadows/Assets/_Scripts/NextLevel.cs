using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player"))
        {
           // SceneManager.LoadScene(sceneName);
        }

    }
}
