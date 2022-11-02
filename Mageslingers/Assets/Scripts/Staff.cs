using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Item
{
    public override void Interact()
    {
        base.Interact();
    }

    public void Equip()
    {
        
    }

}

public enum WeaponType
{
    OneHandStaff = 1,
    TwoHandStaff = 2
}
