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

        if(PlayerManager.LocalPlayer == null)
        {
            PlayerManager.instance.OnLocalPlayerSet += SetUp;
        }
        else
        {
            localController = PlayerManager.LocalPlayer.PlayerPawn.controller;
        }
    }

    private void Start()
    {
    }

    public void SetUp(Player player)
    {
        localController = player.PlayerPawn.controller;
        PlayerManager.instance.OnLocalPlayerSet -= SetUp;
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
