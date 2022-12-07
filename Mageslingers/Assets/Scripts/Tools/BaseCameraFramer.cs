using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCameraFramer
{
    public static void PlaceCamera(Camera InCamera, Transform CameraRig, GameObject InTarget, float FrameMargin)
    {
        if (InCamera && InTarget)
        {
            // Combine bounds to get Bounds that encapsulate entire object
            Bounds TargetBounds = EncapsulateBounds(InTarget);
            if (InCamera.orthographic)
            {
                CalculateCameraDistance_OrthoPoints(InCamera, CameraRig, InTarget.transform, TargetBounds, FrameMargin);
            }
            else
            {
                CalculateCameraDistance_Perspective(InCamera, InTarget, TargetBounds, FrameMargin);
            }
        }
    }

    private static void CalculateCameraDistance_Perspective(Camera InCamera, GameObject InTarget, Bounds TargetBounds, float FrameMargin)
    {
        if (InCamera && InTarget)
        {
            float MaxExtent = 1f;
            Vector3 TargetCenter = InTarget.transform.position;
            if (TargetBounds.extents != Vector3.zero)
            {
                MaxExtent = TargetBounds.extents.magnitude;
                TargetCenter = TargetBounds.center;
            }

            // Find distance to fit object in screen
            float MinDistance = (MaxExtent + FrameMargin) / Mathf.Sin(Mathf.Deg2Rad * InCamera.fieldOfView / 2.0f);
            InCamera.nearClipPlane = MinDistance - MaxExtent;
            InCamera.transform.position = TargetCenter;
            InCamera.transform.position = InCamera.transform.position - (InCamera.transform.forward * MinDistance);
        }
    }

    private static void CalculateCameraDistance_Orthographic(Camera InCamera, GameObject InTarget, Bounds TargetBounds, float FrameMargin)
    {
        if (InCamera && InTarget)
        {
            float MaxExtent = 1f;
            Vector3 TargetCenter = InTarget.transform.position;
            if (TargetBounds.extents != Vector3.zero)
            {
                MaxExtent = TargetBounds.extents.magnitude;
                TargetCenter = TargetBounds.center;
            }

            // Find distance to fit object in screen
            float MinDistance = (MaxExtent + FrameMargin);
            InCamera.orthographicSize = MinDistance;
            InCamera.nearClipPlane = MinDistance - MaxExtent;
            InCamera.transform.position = TargetCenter;
            InCamera.transform.position = InCamera.transform.position - (InCamera.transform.forward * MinDistance);
        }
    }

    public static Bounds EncapsulateBounds(GameObject InTarget)
    {
        Bounds CombinedBounds = new Bounds();

        MeshRenderer[] Renderers = InTarget.GetComponentsInChildren<MeshRenderer>();

        if (Renderers.Length > 0)
        {
            MeshRenderer FirstRenderer = Renderers[0];
            CombinedBounds = FirstRenderer.bounds;
            for (int i = 0; i < Renderers.Length; ++i)
            {
                if (Renderers[i] != FirstRenderer && !Renderers[i].name.Contains("Level")) // Avoid LOD renderers
                {
                    CombinedBounds.Encapsulate(Renderers[i].bounds);
                }
            }
        }
        return CombinedBounds;
    }

    public static void CalculateCameraDistance_OrthoPoints(Camera inCamera, Transform inCameraRig, Transform inTarget, Bounds bounds, float frameMargin)
    {
        Vector3 center = bounds.center;
        Vector3[] points = new Vector3[8];
        points[0] = center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
        points[1] = center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        points[2] = center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        points[3] = center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
        points[4] = center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
        points[5] = center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        points[6] = center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        points[7] = center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);

        Vector3 averagePos = new Vector3();
        for (int i = 0; i < points.Length; i++)
        {
            averagePos += points[i];
        }
        averagePos /= points.Length;
        inCameraRig.position = averagePos;

        Vector3 desiredLocalPos = inCameraRig.InverseTransformPoint(averagePos);
        float size = 0f;
        for (int i = 0; i < points.Length; ++i)
        {
            Vector3 targetLocalPos = inCameraRig.InverseTransformPoint(points[i]);
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / inCamera.aspect);
        }

        size += frameMargin;
        inCamera.orthographicSize = size;
    }
}
