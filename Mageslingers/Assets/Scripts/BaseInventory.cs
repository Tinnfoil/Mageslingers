using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BaseInventory : NetworkBehaviour
{
    public List<PickupableActor> Items = new List<PickupableActor>();

    public virtual void Awake()
    {

    }

    public virtual void AddItem(PickupableActor item)
    {
        Items.Add(item);
    }
    public virtual void RemoveItem(PickupableActor item)
    {
        Items.Remove(item);
    }
}
