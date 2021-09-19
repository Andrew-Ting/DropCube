using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    public void Drop()
    {
        if (transform.gameObject)
        {
            anim.SetTrigger("BlockFallTrigger");
            Invoke("RigidBodyDrop", 2f);
        }
    }

    private void RigidBodyDrop()
    {
        if (transform.gameObject)
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            rb.useGravity = true;
        }
    }
}
