using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveProgress : MonoBehaviour
{
    [SerializeField]
    public Player player;

    public void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player"))
        {
            SavePlayer(player);
        }

    }

    public void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.progress";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerDetails details = new PlayerDetails(player);

        formatter.Serialize(stream, details);
        stream.Close();

    }


}
