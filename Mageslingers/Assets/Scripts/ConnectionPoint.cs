using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    public bool Bottom = false;
    public bool Open = true;
    public ConnectionType connectionType = ConnectionType.BOTTOM;

    [Header("-Runtime-")]
    public ConnectionPoint connection;

    public void Connect(ConnectionPoint targetPoint)
    {
        Open = false;
        targetPoint.Open = false;
        connection = targetPoint;
        targetPoint.connection = this;
    }

    public void Disconnect()
    {
        connection.Open = true;
        connection.connection = null;
        connection = null;
        Open = true;
    }

    public WeaponPieceData GetOwningWeaponData()
    {
        return GetComponentInParent<WeaponPieceData>();
    }

    public int GetConnectionID()
    {
        return ((int)connectionType);
    }
}

public enum ConnectionType : byte
{
    BOTTOM = 0,
    LEFT = 1,
    UP = 2,
    RIGHT = 3
}
