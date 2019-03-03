using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.progress";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        PlayerDetails details = new PlayerDetails(player);

        formatter.Serialize(stream, details);
        stream.Close();

    }

    public static PlayerDetails LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.progress";

        if(File.Exists(path))
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
