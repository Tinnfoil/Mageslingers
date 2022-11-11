using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Staff : Item
{
    public WeaponType WeaponType;
    public override void Interact()
    {
        base.Interact();
    }

    /// <summary>
    /// Assumes identity is a network pawn type
    /// </summary>
    [Command(requiresAuthority = false)]
    public void CmdEquip(NetworkIdentity identity)
    {
        RpcEquip(identity);
    }
    [ClientRpc]
    void RpcEquip(NetworkIdentity identity)
    {
        Equip(identity.GetComponent<PlayerPawn>());
    }

    void Equip(PlayerPawn pawn)
    {
        Holder = pawn;
        Model.transform.parent = pawn.RightHand;
        Model.transform.localPosition = Vector3.zero;
        Model.transform.localEulerAngles = new Vector3(-90, 0, 0);
        pawn.GetComponent<ThirdPersonController>().WeaponType = (int)WeaponType;
    }



}

public enum WeaponType
{
    OneHandStaff = 1,
    TwoHandStaff = 2
}
