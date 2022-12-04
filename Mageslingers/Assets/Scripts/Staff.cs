using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Staff : WeaponPiece
{
    public WeaponType WeaponType;
    public GameObject ProjectilePrefab;
    public GameObject CurrentProjectile;
    public Transform ProjectileSpawnPoint;
    Vector3 mouseTarget;
    public float FireDelay = .2f;
    public override void Interact(Vector3 mouseTarget)
    {
        base.Interact(mouseTarget);
        PrimeProjectile(mouseTarget);
        this.mouseTarget = mouseTarget;
    }

    public void PrimeProjectile(Vector3 target)
    {
        CmdSpawnProjectile(NetworkClient.connection.identity, target);
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawnProjectile(NetworkIdentity caster, Vector3 target)
    {
        Projectile proj = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity, null).GetComponent<Projectile>();
 
        NetworkServer.Spawn(proj.gameObject, PlayerManager.LocalPlayer.gameObject);

        RpcSpawnProjecile(proj.netIdentity, target);
        //TargetRpcSpawnProjectile(conn, proj.netIdentity);
    }

    [ClientRpc]
    public void RpcSpawnProjecile(NetworkIdentity projectileIdentity, Vector3 target)
    {
        projectileIdentity.GetComponent<Projectile>().IntializeProjectile(Holder, target);
        LeanTween.delayedCall(FireDelay, () => Fire(projectileIdentity.GetComponent<Projectile>(), mouseTarget, IsOwner()));
    }

    [TargetRpc]
    public void TargetRpcSpawnProjectile(NetworkConnection conn, NetworkIdentity projectileIdentity)
    {
        LeanTween.delayedCall(FireDelay, () => Fire(projectileIdentity.GetComponent<Projectile>(), mouseTarget, IsOwner()));
    }

    public virtual void Fire(Projectile proj, Vector3 target, bool isOwner)
    {
        proj.FireProjectile(ProjectileSpawnPoint.position, isOwner);
    }

    public bool IsOwner()
    {
        return Holder.gameObject == PlayerManager.LocalPlayer.PlayerPawn.gameObject;
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

    [Client]
    void Equip(CharacterPawn pawn)
    {
        if (IsHeld) return;
        IsHeld = true;
        Holder = pawn;
        Holder.HeldItem = this;
        GetComponent<Rigidbody>().isKinematic = true;
        Model.transform.parent = pawn.RightHand;
        Model.transform.localPosition = Vector3.zero;
        Model.transform.localEulerAngles = new Vector3(-90, 0, 0);
        pawn.GetComponent<ThirdPersonController>().WeaponType = (int)WeaponType;
    }

    [Command(requiresAuthority = false)]
    public void CmdUnEquip()
    {
        RpcUnEquip();
    }
    [ClientRpc]
    void RpcUnEquip()
    {
        UnEquip();
    }

    [Client]
    void UnEquip()
    {
        if (!IsHeld) return;
        IsHeld = false;
        Holder.HeldItem = null;
        Holder = null;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.CopyWorldTransform(Model.transform);
        Model.transform.parent = this.transform;
        Model.transform.localPosition = Vector3.zero;
        Model.transform.localEulerAngles = new Vector3(0, 0, 0);
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isServer)
        {
            CmdRequestData(NetworkServer.localConnection);
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

    }


    [Command(requiresAuthority = false)]
    public void CmdRequestData(NetworkConnectionToClient requester)
    {
        GetData(requester, Holder ? Holder.netIdentity : null);
    }

    [TargetRpc]
    public void GetData(NetworkConnection requester, NetworkIdentity holder)
    {
        if (holder)
        {
            Equip(holder.GetComponent<CharacterPawn>());
        }
    }

}

public enum WeaponType
{
    OneHandStaff = 1,
    TwoHandStaff = 2
}
