using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMaster : MonoBehaviour
{
    private static SaveMaster instance; //static instance of the save master
    public Vector3 checkpointPos;

    void Awake()
    {
        //Checking if an instance already exists
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
