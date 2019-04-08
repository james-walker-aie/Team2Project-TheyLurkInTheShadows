using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class RetryPlayerLoad : MonoBehaviour
{

    public void OnButtonPressed()
    {
        LoadPlayer();
    }

    public static PlayerDetails LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.progress";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerDetails details = formatter.Deserialize(stream) as PlayerDetails;
            stream.Close();

            return details;
        }
        else
        {
            Debug.LogError("Save file was not found in " + path);

            return null;
        }
    }
}
