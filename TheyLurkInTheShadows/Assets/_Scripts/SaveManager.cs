using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    public Vector3 checkpointPos;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
