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

    private void Update()
    {
        if (transform.position.y <= -10f)
        {
            Destroy(transform.gameObject);
        }
    }

    public void Drop(float dropTime)
    {
        if (transform.gameObject)
        {
            anim.SetTrigger("BlockFallTrigger");
            Invoke("RigidBodyDrop", dropTime);
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
