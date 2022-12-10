using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPieceData : MonoBehaviour
{
    public WeaponPiece OwningWeaponPiece;
    public List<ConnectionPoint> connectionPoints = new List<ConnectionPoint>();
    public ConnectionPoint BottomConnection;

    public int ID = 2;

    public string Data;
    public void SetData(Staff owningStaff, WeaponPiece weaponPiece)
    {
        Data = weaponPiece.name;
        name = weaponPiece.name;

        OwningWeaponPiece = owningStaff;
    }

    public ConnectionPoint GetConnectionPoint(int ConnectionID)
    {
        for (int i = 0; i < connectionPoints.Count; i++)
        {
            if (connectionPoints[i].GetConnectionID() == ConnectionID) { return connectionPoints[i]; }
        }
        return null;
    }
}
