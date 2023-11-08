using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionPiece : MonoBehaviour
{
    Rigidbody rb;
    float startDelay = 5;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        startDelay -= Time.fixedDeltaTime;
        if (startDelay <= 0 && rb && rb.velocity.magnitude <= .01f)
        {
            Destroy(rb);
            Destroy(rb.GetComponent<Collider>());
            Destroy(this);
        }
    }
}
