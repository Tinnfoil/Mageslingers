using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ItemDatabaseObject", menuName = "Data Assets/Item Database", order = 1)]
public class ItemDatabaseObject : ScriptableObject
{
    public ItemEntry[] Items;

    [System.Serializable]
    public struct ItemEntry
    {
        public int ID;
        public GameObject ItemPrefab;
    }
}
