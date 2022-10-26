using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerPawn : NetworkPawn
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log(netId);
    }

}
