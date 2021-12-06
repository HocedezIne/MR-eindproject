using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad
{
    private static string savedDataPath = Application.persistentDataPath + "/sets.gd";
    public static List<Build> savedBuilds = new List<Build>();

    public static void Save()
    {
        // check if the current build has already been saved, if so update that saves data
        int index = SaveLoad.savedBuilds.FindIndex(a => a.name.Contains(Build.current.name));
        if(SaveLoad.savedBuilds.Contains(Build.current))
        {
            SaveLoad.savedBuilds[index].state = Build.current.state;
            SaveLoad.savedBuilds[index].stepNumber = Build.current.stepNumber;
        } else
        {
            SaveLoad.savedBuilds.Add(Build.current);
        }
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savedDataPath);
        bf.Serialize(file, SaveLoad.savedBuilds);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(savedDataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savedDataPath, FileMode.Open);
            SaveLoad.savedBuilds = (List<Build>)bf.Deserialize(file);
            file.Close();
        }
    }
}
