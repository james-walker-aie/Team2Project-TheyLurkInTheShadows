using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveProgress : MonoBehaviour
{

    public void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player"))
        {
            SavePlayer();
        }

    }

    public void SavePlayer()
    {
        //SaveLoadSystem.SavePlayer();
    }


}
