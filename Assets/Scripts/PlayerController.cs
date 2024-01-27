using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerCam;
    private Rigidbody rb;
    Vector2 moveInput;
    PhotonView pv;

    bool moving = false;
    bool isGrounded = false;
    bool isRunning = false;

    float walkSpeed = 20.0f;
    float runSpeed = 30.0f;

    float speed = 0.0f;
    float maxSpeed = 20.0f;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = walkSpeed;

        if (!pv.IsMine)
        {
            Destroy(playerCam);
            Destroy(rb);
        }
    }

    void FixedUpdate()
    {
        if(!pv.IsMine) { return; }
        // if the player is moving too fast, slow them down
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // if the player isn't moving and is on the ground, slow them down
        Ray groundedRay = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.Raycast(groundedRay, 1.2f);

        if (isRunning)
            speed = runSpeed;
        else
            speed = walkSpeed;

        if (!moving && isGrounded)
        {
            rb.velocity = rb.velocity * 0.9f;
        }

        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        rb.velocity = rb.velocity + moveDir * speed * Time.deltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.phase == InputActionPhase.Started)
        {
            moving = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moving = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isRunning = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
        }
    }


}