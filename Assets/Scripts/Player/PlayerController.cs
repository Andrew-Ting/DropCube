using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject floor;

    private CharacterController controller;
    private Animator anim;
    private float gravity;
    private bool coroutineStarted;
    private bool movementAllowed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        gravity = 0.0f;
        coroutineStarted = false;
        movementAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (!controller.isGrounded || !IsGrounded())
        {
            gravity -= 9.8f;
            controller.Move(new Vector3(0.0f, gravity, 0.0f));
        }
        else
        {
            gravity = 0.0f;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        return Physics.Raycast(transform.position, dir, out hit, distance);
    }

    private void Move()
    {
        Vector3 movement = Vector3.zero;
        Vector3 newPosition;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement = Vector3.right * -1;
            newPosition = transform.position + new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement = Vector3.forward * -1;
            newPosition = transform.position + new Vector3(0, 0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement = Vector3.right;
            newPosition = transform.position + new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement = Vector3.forward;
            newPosition = transform.position + new Vector3(0, 0, 1);
        }
        else
        {
            return;
        }

        if (!coroutineStarted && movementAllowed)
        {
            coroutineStarted = true;
            transform.rotation = Quaternion.LookRotation(movement);
            if (anim.gameObject.activeSelf)
            {
                anim.SetTrigger("StepTrigger");
            }
            StartCoroutine(MovePlayer(transform.position, newPosition, 0.5f));
        }
    }

    private IEnumerator MovePlayer(Vector3 startPos, Vector3 newPos, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(startPos, newPos, (Time.time - startTime) / overTime);
            yield return null;
        }

        transform.position = newPos;
        if (floor.GetComponent<FloorController>().CheckPlayerTouchedReset())
        {
            movementAllowed = false;
            floor.GetComponent<FloorController>().ResetLevel();
        }
        coroutineStarted = false;
    }

    public float GetXPosition()
    {
        return transform.position.x;
    }

    public float GetZPosition()
    {
        return transform.position.z;
    }

    public void EnableMovement()
    {
        movementAllowed = true;
    }
}
