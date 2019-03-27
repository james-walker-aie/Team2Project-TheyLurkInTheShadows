using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public string sceneName = string.Empty;

    public void OnButtonPressed()
    {
        SceneManager.LoadScene(sceneName);
    }
}
