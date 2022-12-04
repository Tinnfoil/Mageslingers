using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterPawn : NetworkPawn
{
    [Header("Transforms")]
    public Transform RightHand;
    public Transform LeftHand;

    public Item HeldItem;

    public BaseInventory inventory;

    [SyncVar]
    public float Health;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public void TakeDamage(float incomingDamage)
    {
        CmdTakeDamage(incomingDamage);
    }

    [Command(requiresAuthority = false)]
    public void CmdTakeDamage(float damage)
    {
        Health -= damage;
    }

}
