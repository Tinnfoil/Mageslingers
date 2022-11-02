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
        AttachCamera();
    }

    public void AttachCamera()
    {
        if (hasAuthority)
        {
            CinemachineVirtualCamera camera = FindObjectOfType<CinemachineVirtualCamera>();
            if (camera != null) { camera.Follow = this.gameObject.transform; }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
