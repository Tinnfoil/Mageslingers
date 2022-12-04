using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClassExtentions
{
    //Even though they are used like normal methods, extension
    //methods must be declared static. Notice that the first
    //parameter has the 'this' keyword followed by a Transform
    //variable. This variable denotes which class the extension
    //method becomes a part of.
    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }
    public static void CopyWorldTransform(this Transform trans, Transform targetTransform)
    {
        trans.position = targetTransform.position;
        trans.rotation = targetTransform.rotation;
        trans.localScale = targetTransform.localScale;
    }

}
