using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;

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

    public static void RunActions(Actions[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act();
        }
    }

    public static List<T> FindObjectsOfTypeAll<T>()
    {
        List<T> result = new List<T>();
        SceneManager.GetActiveScene().GetRootGameObjects().ToList().ForEach(g => result.AddRange(g.GetComponentsInChildren<T>()));

        return result;
    }
}
