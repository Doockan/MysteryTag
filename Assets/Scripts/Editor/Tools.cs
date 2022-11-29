using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
  public static class Tools
  {
    private static readonly string SaveDataPath = Application.persistentDataPath + "/PlayerProgress.json";
    
    [MenuItem("Tools/Delete Save Data")]
    public static void DeleteSaveData()
    {
      File.Delete(SaveDataPath);
    }
  }
}