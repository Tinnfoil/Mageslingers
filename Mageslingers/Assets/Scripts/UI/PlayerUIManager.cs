using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    public PlayerInventoryUI playerInventoryUI;

    public bool Crafting = false;

    public ThirdPersonController localController;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

    }

    private void Start()
    {
        if (PlayerManager.LocalPlayer == null)
        {
            PlayerManager.instance.OnLocalPlayerPawnSet += SetUp;
        }
        else
        {
            localController = PlayerManager.LocalPlayer.PlayerPawn.controller;
        }
    }

    public void SetUp(PlayerPawn playerpawn)
    {
        localController = playerpawn.controller;
        PlayerManager.instance.OnLocalPlayerPawnSet -= SetUp;
    }

    public void AddItem(Item item)
    {
        playerInventoryUI.AddItem(item);
    }

    public void RemoveItem(Item item)
    {
        playerInventoryUI.RemoveItem(item);
    }

    public void BeginCraft()
    {
        Crafting = !Crafting;
        if (Crafting)
        {
            PlayerManager.LocalPlayer.PlayerPawn.SetPlayerState(PlayerState.CRAFTING);
        }
        else
        {
            PlayerManager.LocalPlayer.PlayerPawn.SetPlayerState(PlayerState.COMBAT);
        }
    }

    public void StopCraft()
    {
    }
}
