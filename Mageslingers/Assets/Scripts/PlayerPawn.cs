using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerPawn : CharacterPawn
{
    public ThirdPersonController controller;
    public AttachToCamera cameraAttach;

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

    public void IntializeController()
    {

        controller.Initialize();
        cameraAttach.AttachCamera();
        transform.position = new Vector3(0, 2, 0);

    }

}
