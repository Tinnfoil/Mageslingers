using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableActor : InteractableActor
{
    public override void Interact(Vector3 mouseTarget)
    {
        Debug.Log("Pick up Interact");
        base.Interact(mouseTarget);
    }
}
