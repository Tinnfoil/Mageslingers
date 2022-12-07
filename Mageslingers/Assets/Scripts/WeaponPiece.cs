using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPiece : Item
{
    public List<ConnectionPoint> connectionPoints = new List<ConnectionPoint>();
    public ConnectionPoint BottomConnection; 
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void ConnectToPiece(WeaponPiece piece)
    {
        
    }

}
