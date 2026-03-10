using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Custom Data/Inventory Data")]
public class Inventory : ScriptableObject
{
    [SerializeField] ItemDatabase itemDatabase;
    [SerializeField] List<Item> inventory = new List<Item>();

    public ItemDatabase ItemDatabase { get { return itemDatabase; } }

    public void AddItem(Item item)
    {
        inventory.Add(item);
    }

    public int CheckAmount(Item item)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].ItemId == item.ItemId)
            {
                if (inventory[i].AllowMultiple)
                {
                    return inventory[i].Amount;
                }
                else
                {
                    return 1;
                }
            }
        }
        return 0;
    }

    public void ModifyItemAmount(Item item, int amount, bool give = false)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].ItemId == item.ItemId)
            {
                if (inventory[i].AllowMultiple)
                {
                    inventory[i].ModifyAmount(give ? -amount : amount);

                    if (inventory[i].Amount <= 0 && give)
                        inventory.RemoveAt(i);
                }
                else
                {
                    inventory.RemoveAt(i);
                }
                return;
            }
        }
        Item newItem = Extensions.CopyItem(item);
        newItem.ModifyAmount(amount);
        AddItem(newItem);
    }
}