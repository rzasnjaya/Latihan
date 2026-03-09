using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] int itemId;
    [SerializeField] string itemName;
    [SerializeField] string itemDescription;
    [SerializeField] Sprite itemSprite;
    [SerializeField] bool allowMultiple;
    [SerializeField] int amount;

    public Item(int itemId, string name, string desc)
    {
        this.itemId = itemId;
        this.itemName = name;
        this.itemDescription = desc;
    }
}
