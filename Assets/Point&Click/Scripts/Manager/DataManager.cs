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

    private int saveDataId = 0;
    private List<SaveData> saveDatas = new List<SaveData>();


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

    public bool HasSaveData()
    {
        if (saveDatas != null)
        {
            return (saveDatas.Count > 0);
        }
        else
        {
            return false;
        }
    }

    public void Save()
    {
        SaveSystem.Save(saveDatas);
    }

    public void Load()
    {
        saveDatas = SaveSystem.Load<List<SaveData>>();
    }

    public void SetPrevScene(string name)
    {
        PrevSceneName = name;
    }

    public void SavaDataEntry()
    {
        if (saveDatas.Count == 0)
            saveDatas.Add(new SaveData());

        OnSave();
        saveDatas[saveDataId].currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SaveInventory();
        Save();
    }

    public void LoadDataEntry(int id)
    {
        Load();
        saveDataId = id;
        LoadInventory();
        OnLoad();
    }

    public void SaveEntities(string id, EntityData data)
    {
        if (saveDatas[saveDataId].entitiesData.ContainsKey(id))
        {
            saveDatas[saveDataId].entitiesData[id] = data;

            return;
        }
        else
        {
            saveDatas[saveDataId].entitiesData.Add(id, data);
        }
    }

    public EntityData LoadEntities(string id)
    {
        if (saveDatas[saveDataId].entitiesData.ContainsKey(id))
            return saveDatas[saveDataId].entitiesData[id];
        else
            return null;
    }

    public void SaveInventory()
    {
        saveDatas[saveDataId].inventoryItemsId.SaveItemsToId(inventory.GetInventory);
    }

    public void LoadInventory()
    {
        inventory.UpdateInventory(saveDatas[saveDataId].inventoryItemsId);
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
    public string currentScene;
        public Dictionary<string, EntityData> entitiesData = new Dictionary<string, EntityData>();
    public List<int> inventoryItemsId = new List<int>();
}
