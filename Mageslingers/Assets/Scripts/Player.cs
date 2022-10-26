using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkActor
{
    public GameObject PlayerPawnPrefab;

    public PlayerPawn PlayerPawn;
    public override void Start()
    {
        if (!hasAuthority) return;

        base.Start();



    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!hasAuthority) return;

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

        SpawnPlayerPawn_ClientRpc(g.GetComponent<NetworkIdentity>());

    }


    [ClientRpc]
    public void SpawnPlayerPawn_ClientRpc(NetworkIdentity pawn)
    {
        PlayerPawn = pawn.GetComponent<PlayerPawn>();
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

    /*
    [TargetRpc]
    public void SpawnPlayerPawn_TargetRpc(NetworkConnection conn)
    {

    }
    */
}
