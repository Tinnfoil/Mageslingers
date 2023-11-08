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

    public List<ConnectionPoint> OpenConnections = new List<ConnectionPoint>();
    private Transform lastParent;

    public Dictionary<int, WeaponPieceData> WeaponPiecesData = new Dictionary<int, WeaponPieceData>();
    private int CurrentID = 0;

    public string StaffData;


    public override void Start()
    {
        OpenConnections.AddRange(weaponData.connectionPoints);
        WeaponPiecesData.Add(weaponData.ID, weaponData);
        base.Start();
    }

    public override void Update()
    {
        //Debug.Log("Update");
        base.Update();
    }

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
    void Equip(PlayerPawn pawn)
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

        pawn.OnPlayerStateChanged += HandlePlayerStateChanged;
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

        (Holder as PlayerPawn).OnPlayerStateChanged -= HandlePlayerStateChanged;

        IsHeld = false;
        Holder.HeldItem = null;
        Holder = null;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.CopyWorldTransform(Model.transform);
        Model.transform.parent = this.transform;
        Model.transform.localPosition = Vector3.zero;
        Model.transform.localEulerAngles = new Vector3(0, 0, 0);


    }

    public void HandlePlayerStateChanged(PlayerState state) { if (state == PlayerState.CRAFTING) { CraftMode(); } else if (state == PlayerState.COMBAT) { UnCraftMode(); } }
    public void CraftMode()
    {
        lastParent = Model.transform.parent;
        Model.transform.parent = null;
        Model.transform.position = Holder.transform.position + Vector3.forward / 2f + Vector3.right / 2f + Vector3.up;
        Model.transform.localEulerAngles = new Vector3(45, 0, -30);
    }

    public void UnCraftMode()
    {
        Model.transform.parent = lastParent;
        Model.transform.localPosition = Vector3.zero;
        Model.transform.localEulerAngles = new Vector3(-90, 0, 0);
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
            Equip(holder.GetComponent<PlayerPawn>());
            Instantiate(GameManager.instance.ItemDB.Items[0].GetComponent<WeaponPiece>().Model);
        }
    }

    public ConnectionPoint GetClosestConnectionPoint(Vector3 point)
    {
        float minDist = float.MaxValue;
        ConnectionPoint cp = null;
        for (int i = 0; i < OpenConnections.Count; i++)
        {
            float dist = Vector3.Distance(OpenConnections[i].transform.position, point);
            if (dist < minDist)
            {
                minDist = dist;
                cp = OpenConnections[i];
            }
        }
        return cp;
    }


    public void ConnectPiece(ConnectionPoint cp, WeaponPiece weaponPiece)
    {
        //Debug.Log("command");
        ConnectPiece_Command(cp.GetOwningWeaponData().ID, cp.GetConnectionID(), weaponPiece.netIdentity);
    }

    [Command(requiresAuthority = false)]
    public void ConnectPiece_Command(int id, int connection, NetworkIdentity weaponPieceIdentity)
    {
        //Debug.Log("commandFire");
        ConnectPiece_Implementation(id, connection, weaponPieceIdentity.GetComponent<WeaponPiece>());
        ConnectPiece_ClientRPC(id, connection, weaponPieceIdentity);
    }
    [ClientRpc]
    public void ConnectPiece_ClientRPC(int id, int connection, NetworkIdentity weaponPieceIdentity)
    {
        if (!isServer)
        {
            ConnectPiece_Implementation(id, connection, weaponPieceIdentity.GetComponent<WeaponPiece>());
        }
    }

    public void ConnectPiece_Implementation(int id, int connection, WeaponPiece weaponPiece)
    {
        //Debug.Log("implementation");
        WeaponPieceData weaponData = WeaponPiecesData[id];
        ConnectionPoint cp = weaponData.GetConnectionPoint(connection);
        //weaponPiece.Model.transform.parent = cp.transform;
        weaponPiece.Model.transform.rotation = cp.transform.rotation;
        weaponPiece.Model.transform.position = cp.transform.position;
        weaponPiece.Model.transform.parent = Model;

        weaponPiece.weaponData.BottomConnection.Connect(cp);

        weaponPiece.ColliderContainer.gameObject.SetActive(true);
        weaponPiece.HandlePieceConnected(this);
        int newID = -1;
        int.TryParse((id != -1 ? id.ToString() : string.Empty) + connection.ToString(), out newID);
        WeaponPiecesData.Add(newID, weaponPiece.weaponData);

        OpenConnections.AddRange(weaponPiece.weaponData.connectionPoints);
        OpenConnections.Remove(cp);

        if (isServer)
        {
            NetworkServer.Destroy(weaponPiece.gameObject);
        }
    }

    /*
    public void ConnectPiece(ConnectionPoint cp, WeaponPiece weaponPiece)
    {

        //weaponPiece.Model.transform.parent = cp.transform;
        weaponPiece.Model.transform.rotation = cp.transform.rotation;
        weaponPiece.Model.transform.position = cp.transform.position;
        weaponPiece.Model.transform.parent = Model;

        weaponPiece.weaponData.BottomConnection.Connect(cp);

        weaponPiece.ColliderContainer.gameObject.SetActive(true);
        weaponPiece.HandlePieceConnected(this);
        WeaponPiecesData.Add(CurrentID++, weaponPiece.weaponData);

        OpenConnections.AddRange(weaponPiece.weaponData.connectionPoints);
        OpenConnections.Remove(cp);

        Destroy(weaponPiece);
    }
    */

}

public enum WeaponType
{
    OneHandStaff = 1,
    TwoHandStaff = 2
}
