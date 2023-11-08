using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StreamingAssets : MonoBehaviour
{

    public AssetReference[] assets;
    public AssetLabelReference labelreference;

    public List<GameObject> assetGameobjects;
    
    // Start is called before the first frame update
    void Start()
    {
        assetGameobjects = new List<GameObject>();
        Addressables.LoadAssetsAsync<Object>(labelreference, (o) => { });
        foreach (AssetReference ar in assets)
        {
            ar.InstantiateAsync().Completed += (result) => assetGameobjects.Add(result.Result);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Addressables.LoadAssetsAsync<Object>(labelreference, (o) => { });
            foreach (AssetReference ar in assets)
            {
                ar.InstantiateAsync().Completed += (result) => assetGameobjects.Add(result.Result);
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            //Addressables.Release(labelreference);

            foreach (GameObject ar in assetGameobjects)
            {
                Addressables.Release(ar);
            }
            assetGameobjects.Clear();
        }
    }
}
