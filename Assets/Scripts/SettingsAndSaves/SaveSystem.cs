using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGameData(GameData gameData) {
        string path = Application.persistentDataPath + "/player.save";
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        
        FileStream fileStream = new FileStream(path, FileMode.Create);
        binaryFormatter.Serialize(fileStream, gameData);
        fileStream.Close();
    }
    
    public static GameData LoadGameData() {
        string path = Application.persistentDataPath + "/player.save";
        
        if (File.Exists(path)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            GameData data = binaryFormatter.Deserialize(fileStream) as GameData;
            fileStream.Close();
            
            return data;
        } else {
            return new GameData();
        }
    }
}
