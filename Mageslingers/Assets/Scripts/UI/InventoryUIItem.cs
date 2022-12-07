using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour
{
    public Image Icon;

    public void SetIcon(Sprite sprite)
    {
        Icon.sprite = sprite;
    }
}
