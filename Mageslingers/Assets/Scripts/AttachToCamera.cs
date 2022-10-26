using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class AttachToCamera : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (hasAuthority)
        {
            FindObjectOfType<CinemachineVirtualCamera>().Follow = this.gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
