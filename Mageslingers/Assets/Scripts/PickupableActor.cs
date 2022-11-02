using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableActor : InteractableActor
{
    public override void Interact()
    {
        Debug.Log("Pick up Interact");
        base.Interact();
    }
}
