using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
    private void Awake()
    {
        try
        {
            Steamworks.SteamClient.Init(2145460);
        }
        catch
        {
            Debug.Log("Could not initialize Steam Client");
        }
        DontDestroyOnLoad(this.gameObject);
    }


    private void OnDisable()
    {
        Steamworks.SteamClient.Shutdown();
    }

    // Update is called once per frame
    void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }
}
