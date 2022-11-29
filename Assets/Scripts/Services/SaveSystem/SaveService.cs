using System.IO;
using UnityEngine;

namespace Systems.SaveLoadSystems
{
    static class SaveService
    {
        public static string PlayerProgressFilePath = Application.persistentDataPath + "/PlayerProgress.json";

        public static void Save(SaveData data)
        {
            var json = JsonUtility.ToJson(data);
            using (var write = new StreamWriter(PlayerProgressFilePath))
            {
                write.WriteLine(json);
            }
        }

        public static SaveData Load()
        {
            string json = "";
            using (var reader = new StreamReader(PlayerProgressFilePath))
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
}