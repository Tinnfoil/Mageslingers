using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TestCommands : MonoBehaviour
{
    public GameObject[] arguments;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.H))
        {
            GameObject g = Instantiate(arguments[0], PlayerManager.LocalPlayer.transform.position + new Vector3(0 ,3, 0), Quaternion.identity);
            NetworkServer.Spawn(g);
        }
    }
}
