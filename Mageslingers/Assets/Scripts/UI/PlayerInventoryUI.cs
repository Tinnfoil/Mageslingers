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
        g.GetComponent<InventoryUIItem>().SetIcon(this, item, item.Icon);
        Items.Add(item, g);
    }

    public void RemoveItem(Item item)
    {
        Destroy(Items[item]);
        Items.Remove(item);
    }

    public void SetActiveItem(Item item)
    {
        PlayerUIManager.instance.localController.ActiveUIItem = item.transform;
        item.gameObject.SetActive(true);
        item.ColliderContainer.gameObject.SetActive(false);
    }
    public void StopActiveItem(Item item)
    {
        PlayerUIManager.instance.localController.ActiveUIItem = null;
        item.gameObject.SetActive(false);
        item.ColliderContainer.gameObject.SetActive(true);
    }


}
