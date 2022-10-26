using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager instance;
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
    public List<PlayerPawn> playerPawns;

    public override void OnStartClient()
    {
        base.OnStartClient();

        RequestPawnData(netIdentity);
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
            }
        }

    }
}
