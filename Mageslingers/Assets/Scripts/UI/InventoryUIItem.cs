using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour
{
    PlayerInventoryUI inventoryUI;
    private Item item;
    public Image Icon;
    private void Start()
    {
        if (item == null)
        {
            Destroy(this.gameObject);
        }
    }
    public void SetIcon(PlayerInventoryUI inventoryUI, Item item, Sprite sprite)
    {
        this.inventoryUI = inventoryUI;
        this.item = item;
        Icon.sprite = sprite;
    }

    public void SetItemActive()
    {
        inventoryUI.SetActiveItem(item);
    }
}
