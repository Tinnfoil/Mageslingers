using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableActor : NetworkActor
{
    public virtual void Interact()
    {
        Debug.Log("Interact");
    }

}
