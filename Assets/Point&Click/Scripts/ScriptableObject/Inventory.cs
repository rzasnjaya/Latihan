using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Custom Data/Inventory Data")]
public class Inventory : ScriptableObject
{
    [SerializeField] ItemDatabase itemDatabase;
    [SerializeField] List<Item> inventory = new List<Item>();

    public void AddItem(Item item)
    {
        inventory.Add(item);
    }

    public ItemDatabase ItemDatabase { get { return itemDatabase; } }
}
