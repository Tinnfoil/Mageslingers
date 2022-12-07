using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : BaseUI
{
    public GameObject ItemIconPrefab;
    public Transform InventoryContainer;

    public Dictionary<Item, GameObject> Items = new Dictionary<Item, GameObject>();

    public void AddItem(Item item)
    {
        GameObject g = Instantiate(ItemIconPrefab, InventoryContainer);
        g.GetComponent<InventoryUIItem>().SetIcon(item.Icon);
        Items.Add(item, g);
    }

    public void RemoveItem(Item item)
    {
        Destroy(Items[item]);
        Items.Remove(item);
    }

}
