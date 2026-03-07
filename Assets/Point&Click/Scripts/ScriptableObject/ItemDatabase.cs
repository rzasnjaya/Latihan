using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Custom Data/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] List<Item> items = new List<Item>();
    [SerializeField] List<string> itemsNames = new List<string>();
}
