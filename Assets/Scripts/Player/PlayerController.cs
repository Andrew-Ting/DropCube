using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 forward, right;
    private float gravity;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gravity = 0.0f;
        moveSpeed = 20f;
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Move();
        }

        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            gravity -= 9.8f;
            controller.Move(new Vector3(0.0f, gravity, 0.0f));
        }
        else
        {
            gravity = 0.0f;
        }
    }

    private void Move()
    {
        Vector3 rightMovement = right * Input.GetAxis("HorizontalKey");
        Vector3 upMovement = forward * Input.GetAxis("VerticalKey");

        Vector3 movement = rightMovement + upMovement;

        movement.Normalize();

        controller.Move(movement * moveSpeed * Time.deltaTime);
    }
}
