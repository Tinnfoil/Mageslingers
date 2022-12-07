using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BaseInventory : NetworkBehaviour
{
    public List<Item> Items = new List<Item>();

    public virtual void Awake()
    {

    }

    public virtual void AddItem(Item item)
    {
        Items.Add(item);
    }
    public virtual void RemoveItem(Item item)
    {
        Items.Remove(item);
    }
}
