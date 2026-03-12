using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    public static void Save<T>(T saveData)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = new FileStream(Application.persistentDataPath + "/savedata.dat", FileMode.Create);
        binaryFormatter.Serialize(file, saveData);
        file.Close();
#if UNITY_EDITOR
        Debug.Log("Save Success");
#endif
    }

    public static T Load<T>()
    {
        if (CheckForSave())
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = new FileStream(Application.persistentDataPath + "/savedata.dat", FileMode.Open);

            T loadData = (T)binaryFormatter.Deserialize(file);
            file.Close();
#if UNITY_EDITOR
            Debug.Log("Load Success");
#endif
            return loadData;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Save File not found");
#endif
        }

        return default(T);
    }

    public static bool CheckForSave()
    {
        return (File.Exists(Application.persistentDataPath + "/savedata.dat"));
    }
}