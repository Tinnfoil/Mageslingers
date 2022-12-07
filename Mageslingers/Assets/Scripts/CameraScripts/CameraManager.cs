using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public CinemachineVirtualCamera vcam;
    CinemachineFramingTransposer transposer;

    public float OriginalCameraDistance, xOffset, yOffset;
    public Vector3 FollowOffset;

    private void Awake()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        OriginalCameraDistance = transposer.m_CameraDistance;
        xOffset = transposer.m_ScreenX;
        yOffset = transposer.m_ScreenY;
        FollowOffset = transposer.m_TrackedObjectOffset;

        if(instance == null)
        {
            instance = this;
        }
    }
    public void CraftCamera()
    {
        transposer.m_CameraDistance = 2;
        transposer.m_ScreenX = .2f;
        transposer.m_ScreenY = 1;
        transposer.m_TrackedObjectOffset = new Vector3(0, 1, 0);
    }

    public void ReturnToOriginal()
    {
        transposer.m_CameraDistance = OriginalCameraDistance;
        transposer.m_ScreenX = xOffset;
        transposer.m_ScreenY = yOffset;
        transposer.m_TrackedObjectOffset = FollowOffset;

    }
}
