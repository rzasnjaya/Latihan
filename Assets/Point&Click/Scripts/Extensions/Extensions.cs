using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Extensions
{
    public static bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static Item CopyItem(Item item)
    {
        Item newItem = new Item(item.ItemId, item.ItemName, item.ItemDesc, item.ItemSprite, item.AllowMultiple);
        return newItem;
    }
}
