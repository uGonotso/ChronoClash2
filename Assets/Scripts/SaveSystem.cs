using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGame(GameState gameState, string sceneName)
    {
        string json = JsonUtility.ToJson(gameState);
        File.WriteAllText(Application.persistentDataPath + "/savefile_" + sceneName + ".json", json);
    }

    public static GameState LoadGame(string sceneName)
    {
        string path = Application.persistentDataPath + "/savefile_" + sceneName + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<GameState>(json);
        }
        else
        {
            return null;
        }
    }

    public static void DeleteSaveFile(string sceneName)
    {
        string path = Application.persistentDataPath + "/savefile_" + sceneName + ".json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}