using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PickupableActor : InteractableActor
{
    public override void Interact(Vector3 mouseTarget)
    {
        //Debug.Log("Pick up Interact");
        base.Interact(mouseTarget);
    }

    [Command(requiresAuthority = false)]
    public void Pickup(CharacterPawn pawn, BaseInventory inventory)
    {
        Pickup_ClientRpc(pawn.netIdentity);
    }

    [ClientRpc]
    public void Pickup_ClientRpc(NetworkIdentity pawn)
    {
        pawn.GetComponent<BaseInventory>().AddItem(this);
        transform.gameObject.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = true;
    }

    [Command(requiresAuthority = false)]
    public void Drop(CharacterPawn pawn, BaseInventory inventory)
    {
        Drop_ClientRpc(pawn.netIdentity);
    }
    [ClientRpc]
    public void Drop_ClientRpc(NetworkIdentity pawn)
    {
        transform.gameObject.SetActive(true);
        pawn.GetComponent<BaseInventory>().RemoveItem(this);
        transform.position = pawn.transform.position + Vector3.up + pawn.transform.forward;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
