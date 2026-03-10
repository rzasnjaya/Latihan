using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }
    public Inventory Inventory { get{ return inventory; } }

    [SerializeField] Inventory inventory;

    public string PrevSceneName { get; private set; }
    public LevelManager LevelManager { get; private set; }

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
}
