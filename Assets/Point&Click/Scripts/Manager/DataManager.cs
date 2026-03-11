using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }
    public Inventory Inventory { get { return inventory; } }

    [SerializeField] Inventory inventory;

    public event System.Action OnSave = delegate { };
    public event System.Action OnLoad = delegate { };

    public string PrevSceneName { get; private set; }
    public LevelManager LevelManager { get; private set; }

    private SaveData saveData = new SaveData();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LevelManager = GetComponentInChildren<LevelManager>();
    }

    public void SetPrevScene(string name)
    {
        PrevSceneName = name;
    }

    public void SaveEntities(string id, EntityData data)
    {
        if (saveData.entitiesData.ContainsKey(id))
        {
            saveData.entitiesData[id] = data;

            return;
        }
        else
        {
            saveData.entitiesData.Add(id, data);
        }
    }

    public EntityData LoadEntities(string id)
    {
        if (saveData.entitiesData.ContainsKey(id))
            return saveData.entitiesData[id];
        else
            return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnLoad();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            OnSave();
        }
    }
}

[System.Serializable]
public class SaveData
{
        public Dictionary<string, EntityData> entitiesData = new Dictionary<string, EntityData>();
}
