using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystemUI : MonoBehaviour
{
    [SerializeField] Transform itemsParents;
    [SerializeField] InventoryItemUI itemUIprefabs;
    [SerializeField] Inventory playerInventory;

    private List<InventoryItemUI> itemUICollection = new List<InventoryItemUI>();

    // Use this for initialization
    void Start()
    {
        playerInventory.OnItemChange += Redraw;

        Init(playerInventory.GetInventory);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        playerInventory.OnItemChange -= Redraw;
    }

    void Init(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            AddItemUI(items[i]);
        }
    }

    void AddItemUI(Item item)
    {
        InventoryItemUI tempItem = Instantiate(itemUIprefabs, itemsParents);
        tempItem.Init(item, this);
        itemUICollection.Add(tempItem);
    }

    void Redraw(List<Item> items)
    {
        for (int i = 0;i < itemUICollection.Count;i++)
        {
            Destroy(itemUICollection[i].gameObject);
        }

        itemUICollection.Clear();

        Init(items);
    }

    public void ShowInventory()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}