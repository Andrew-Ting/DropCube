using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private float gravity;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gravity = 0.0f;
        moveSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
        Vector3 movement = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement = Vector3.right * -1;
            
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement = Vector3.forward * -1;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement = Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement = Vector3.forward;
        }
        else
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(movement) * Quaternion.Euler(0, 90, 0);
        Vector3 startPos = transform.position;

        while (Vector3.Distance(startPos, transform.position) < 1)
        {
            controller.Move(movement * moveSpeed * Time.deltaTime);
        }
    }
}
