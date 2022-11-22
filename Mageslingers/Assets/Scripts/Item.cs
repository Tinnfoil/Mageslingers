using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : PickupableActor
{
    [HideInInspector] public CharacterPawn Holder; // If not null, this item is "owned" by the player
    public Transform Model;
    public bool IsHeld; // If held, it is the active item in the players hand

    public override void Interact(Vector3 mouseTarget)
    {
        base.Interact(mouseTarget);
    }
}
