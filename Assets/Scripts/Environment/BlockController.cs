using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Rigidbody rb;
    private bool useGravity = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop()
    {
        if (transform.gameObject)
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            rb.useGravity = true;
        }
    }
}
