using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager instance;

    private static Player localPlayer;
    public static Player LocalPlayer { get { return localPlayer; } set { localPlayer = value;  } }
    
    
    public List<PlayerPawn> playerPawns;

    public Action<Player> OnLocalPlayerSet;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        GameManager.instance.OnSceneLoaded += HandleSceneLoaded;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();


    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
       

    }


    [Command(requiresAuthority = false)]
    public void RequestPawnData(NetworkIdentity requestingPlayer)
    {
        List<NetworkIdentity> ids = new List<NetworkIdentity>();
        List<string> data = new List<string>();
        foreach (PlayerPawn p in playerPawns)
        {
            ids.Add(p.netIdentity);
            data.Add(p.name);
        }
        RequestPawnData_TargetRpc(requestingPlayer.connectionToClient, ids.ToArray(), data.ToArray());
    }

    [TargetRpc]
    public void RequestPawnData_TargetRpc(NetworkConnection conn, NetworkIdentity[] identities, string[] data)
    {
        if (identities != null)
        {
            for (int i = 0; i < identities.Length; i++)
            {
                identities[i].GetComponent<PlayerPawn>().name = data[i];
                playerPawns.Add(identities[i].GetComponent<PlayerPawn>());
            }
        }

    }

    public void HandleSceneLoaded()
    {
        localPlayer.PlayerPawn.IntializeController();
    }

    private void OnDestroy()
    {
        GameManager.instance.OnSceneLoaded -= HandleSceneLoaded;
    }
}
