using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using Mirror.FizzySteam;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SmithworksMatchmaking : MonoBehaviour
{
    public TransportType transport;
    private NetworkManager networkManager;
    public int AppID;

    void Start()
    {
        networkManager = GetComponent<NetworkManager>();

        switch (transport)
        {
            case TransportType.Local:
                break;
            case TransportType.Steam:
                GetComponent<SteamManager>().enabled = true;
                //SteamManager.ins.enabled = true;

                
                //SteamLobby.instance.enabled = true;
                break;
        }

    }

    public void HostLobby()
    {
        switch (transport)
        {
            case TransportType.Local:
                networkManager.StartHost();
                break;
            case TransportType.Steam:
                var serverInit = new SteamServerInit("gmod", "Garry Mode")
                {
                    GamePort = 28015,
                    Secure = true,
                    QueryPort = 28016
                };

                try
                {
                    Steamworks.SteamServer.Init(AppID, serverInit);
                }
                catch (System.Exception)
                {
                    // Couldn't init for some reason (dll errors, blocked ports)
                }
                break;
        }
    }

}

[System.Serializable]
public enum TransportType
{
    Local,
    Steam
}

#if UNITY_EDITOR
[CustomEditor(typeof(SmithworksMatchmaking))]
public class SmithworksMatchmakingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SmithworksMatchmaking matchmaking = (SmithworksMatchmaking)target;
        SteamManager sm = FindObjectOfType<SteamManager>();
        if (sm != null)
        {
            FindObjectOfType<SteamManager>().SteamEnabled = (matchmaking.transport == TransportType.Steam);
        }

    }
}
#endif
