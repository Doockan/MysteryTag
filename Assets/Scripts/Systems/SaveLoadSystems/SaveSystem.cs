using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace MysteryTag
{
    static class SaveSystem
    {
        static string _filePath = Application.persistentDataPath + "/Save.json";

        public static void Save(SaveData data)
        {
            var json = JsonUtility.ToJson(data);
            using (var write = new StreamWriter(_filePath))
            {
                write.WriteLine(json);
            }
        }

        public static SaveData Load()
        {
            string json = "";
            using (var reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    json += line;
                }
            }

            if (string.IsNullOrEmpty(json))
            {
                return new SaveData(); 
            }

            return JsonUtility.FromJson<SaveData>(json);
        }
    }

    [Serializable]
    public class SaveData
    {
        public List<Level> Levels;
    }

    [Serializable]
    public class Level
    {
        public Availability Available;
    }
}