using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActions : Actions
{
    [SerializeField] ItemDatabase itemDatabase;
    [SerializeField] bool giveItem;
    [SerializeField] int amount;
    [SerializeField] Actions[] yesActions, noActions;
    public int itemId;
    [SerializeField] Item currentItem;
    public Item CurrentItem { get { return currentItem; } }
    public ItemDatabase ItemDatabase { get { return itemDatabase; } }

    public void ChangeItem(Item item)
    {
        // Fix: check for null before accessing CurrentItem.ItemId
        if (CurrentItem != null && CurrentItem.ItemId == itemId)
            return;
        if (ItemDatabase != null)
            currentItem = Extensions.CopyItem(item);
    }

    public override void Act()
    {
        if (CurrentItem == null)
        {
            Debug.LogWarning($"ItemActions on {gameObject.name}: CurrentItem is null.");
            return;
        }

        // Fix: guard against null DataManager or Inventory
        if (DataManager.instance == null || DataManager.instance.Inventory == null)
        {
            Debug.LogWarning($"ItemActions on {gameObject.name}: DataManager or Inventory is null.");
            return;
        }

        if (giveItem)
        {
            int itemOwned = DataManager.instance.Inventory.CheckAmount(CurrentItem);
            if (itemOwned > 0)
            {
                if (CurrentItem.AllowMultiple && amount <= itemOwned)
                {
                    DataManager.instance.Inventory.ModifyItemAmount(CurrentItem, amount, true);
                    Extensions.RunActions(yesActions);
                }
                else if (!CurrentItem.AllowMultiple && itemOwned == 1)
                {
                    DataManager.instance.Inventory.ModifyItemAmount(CurrentItem, itemOwned, true);
                    Extensions.RunActions(yesActions);
                }
                else
                {
                    Extensions.RunActions(noActions);
                }
            }
        }
        else
        {
            if (CurrentItem.AllowMultiple)
            {
                DataManager.instance.Inventory.ModifyItemAmount(CurrentItem, amount);
            }
            else if (!CurrentItem.AllowMultiple)
            {
                if (DataManager.instance.Inventory.CheckAmount(CurrentItem) == 1)
                {
                    Extensions.RunActions(noActions);
                }
                else
                {
                    DataManager.instance.Inventory.ModifyItemAmount(CurrentItem, 1);
                    Extensions.RunActions(yesActions);
                }
            }
        }
    }
}