using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryPlayerLoad : MonoBehaviour
{

    public void OnButtonPressed()
    {
        string sceneName = PlayerPrefs.GetString("LastSceneLoaded");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Button has been pressed");
    }

   
}
