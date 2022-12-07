using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerPawn : CharacterPawn
{
    public ThirdPersonController controller;
    public AttachToCamera cameraAttach;

    public PlayerState playerState = PlayerState.COMBAT;

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
        //Debug.Log(netId);
    }

    public void IntializeController()
    {
        controller.Initialize();
        cameraAttach.AttachCamera();
        transform.position = new Vector3(0, 2, 0);
    }

    public void SetPlayerState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.COMBAT:
                CameraManager.instance.ReturnToOriginal();
                playerState = PlayerState.COMBAT;
                break;
            case PlayerState.CRAFTING:
                CameraManager.instance.CraftCamera();
                playerState = PlayerState.CRAFTING;
                break;
        }
    }

}



public enum PlayerState : byte
{
    COMBAT,
    CRAFTING
}
