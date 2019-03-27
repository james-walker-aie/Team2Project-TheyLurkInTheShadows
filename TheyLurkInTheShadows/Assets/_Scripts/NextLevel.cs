using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }

    }
}
