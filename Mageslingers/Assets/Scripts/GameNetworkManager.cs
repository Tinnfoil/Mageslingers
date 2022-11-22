using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameNetworkManager : NetworkManager
{
    public override void Awake()
    {
        base.Awake();   
    }

    public override void OnClientDisconnect()
    {
        //NetworkClient.connection;
        base.OnClientDisconnect();
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log(conn.identity.GetComponent<Player>());
        Debug.Log(conn.identity.GetComponent<Player>().PlayerPawn);
        Debug.Log(conn.identity.GetComponent<Player>().PlayerPawn.HeldItem);
        NetworkServer.Destroy(conn.identity.GetComponent<Player>().PlayerPawn.HeldItem.gameObject);

        base.OnServerDisconnect(conn);
    }

}
