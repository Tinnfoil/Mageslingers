using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Player : NetworkActor
{
    public GameObject PlayerPawnPrefab;

    [HideInInspector] public PlayerPawn PlayerPawn;

    [Header("TESTING")]
    public GameObject Staff;

    public Action<PlayerPawn> OnPlayerPawnSet;

    public override void Start()
    {
        if (!hasAuthority) return;

        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if (isLocalPlayer && PlayerManager.instance) { PlayerManager.LocalPlayer = this; PlayerManager.instance.OnLocalPlayerSet?.Invoke(this); PlayerManager.instance.RequestPawnData(netIdentity); }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!hasAuthority) return;

        SpawnPlayerPawn();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        PlayerManager.instance.playerPawns.Remove(PlayerPawn);

    }

    public void SpawnPlayerPawn()
    {
        SpawnPlayerPawn(gameObject);
    }

    [Command]
    public void SpawnPlayerPawn(GameObject requestingPlayer)
    {
        GameObject g = Instantiate(PlayerPawnPrefab);
        g.name = name + "'s Pawn";
        PlayerPawn = g.GetComponent<PlayerPawn>();
        PlayerManager.instance.playerPawns.Add(PlayerPawn);
        NetworkServer.Spawn(g);
        g.GetComponent<NetworkIdentity>().AssignClientAuthority(requestingPlayer.GetComponent<NetworkIdentity>().connectionToClient);

        GameObject staff = Instantiate(Staff);
        NetworkServer.Spawn(staff);
        staff.GetComponent<Staff>().CmdEquip(PlayerPawn.netIdentity);

        SpawnPlayerPawn_ClientRpc(g.GetComponent<NetworkIdentity>());

    }


    [ClientRpc]
    public void SpawnPlayerPawn_ClientRpc(NetworkIdentity pawn)
    {
        PlayerPawn = pawn.GetComponent<PlayerPawn>();
        OnPlayerPawnSet?.Invoke(PlayerPawn) ;
        if (isLocalPlayer) { PlayerManager.instance.InvokePlayerPawnSet(PlayerPawn); }
    }



    [Command]
    public void RequestPawnData(GameObject requestingPlayer)
    {
        Debug.Log(name);
        RequestPawnData_TargetRpc(requestingPlayer.GetComponent<NetworkIdentity>().connectionToClient, PlayerPawn.name);
    }

    [TargetRpc]
    public void RequestPawnData_TargetRpc(NetworkConnection conn, string data)
    {
        PlayerPawn.name = data;
    }

    [Command]
    public void CmdEquipWeapon()
    {
        GameObject staff = Instantiate(Staff);
        NetworkServer.Spawn(staff);
        LeanTween.delayedCall(1, () => staff.GetComponent<Staff>().CmdEquip(PlayerPawn.netIdentity));

    }

    /*
    [TargetRpc]
    public void SpawnPlayerPawn_TargetRpc(NetworkConnection conn)
    {

    }
    */
}
