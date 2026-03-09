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

    public Item CurrentItem { get; private set; }

    public ItemDatabase ItemDatabase { get { return itemDatabase; } }

    public void ChangeItem(Item item)
    {
        if (CurrentItem.ItemId == itemId)
            return;

        if (ItemDatabase !=null) 
            CurrentItem = Extensions.CopyItem(item);
    }

    public override void Act()
    {
        
    }
}
