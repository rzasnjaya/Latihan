using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveEntity : MonoBehaviour, ISaveAble
{
    [SerializeField] string instanceID;
    private EntityData entityData = new EntityData();

    void Reset()
    {
        instanceID = gameObject.name + gameObject.GetInstanceID();
    }

    public void LoadState()
    {
        entityData = DataManager.instance.LoadEntities(instanceID);

        if (entityData == null)
            return;

        transform.position = entityData.GetPosition();
        transform.rotation = entityData.GetRotation();

        for (int i = 0; i <transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(entityData.childActiveStatus[i]);
        }
    }

    public void SaveState()
    {
        entityData.SetPosition(transform.position);
        entityData.SetRotation(transform.rotation);

        for (int i = 0; i < transform.childCount; i++)
        {
            entityData.childActiveStatus.Add(i, transform.GetChild(i).gameObject.activeInHierarchy);
        }

        DataManager.instance.SaveEntities(instanceID, entityData);
    }

    // Use this for initialization
    void Start()
    {
        DataManager.instance.OnSave += SaveState;
        DataManager.instance.OnLoad += LoadState;
    }    

    void OnDestroy()
    {
        DataManager.instance.OnSave -= SaveState;
        DataManager.instance.OnLoad -= LoadState;
    }
}