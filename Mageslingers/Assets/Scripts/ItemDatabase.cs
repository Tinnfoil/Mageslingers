using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private ItemDatabaseObject itemDBObject;
    public Dictionary<int, GameObject> Items;
    public void InitializeDatabase()
    {
        Items = new Dictionary<int, GameObject>();
        for (int i = 0; i < itemDBObject.Items.Length; i++)
        {
            Items.Add(itemDBObject.Items[i].ID, itemDBObject.Items[i].ItemPrefab);
            GameNetworkManager.singleton.spawnPrefabs.Add(itemDBObject.Items[i].ItemPrefab);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
