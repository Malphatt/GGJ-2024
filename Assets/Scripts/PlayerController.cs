using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerCam;
    private Rigidbody rb;
    Vector2 moveInput;
    PhotonView pv;

    public GameObject Camera;
    public GameObject CameraFacing;

    bool moving = false;
    bool isGrounded = false;
    bool isRunning = false;

    float walkSpeed = 20.0f;
    float runSpeed = 30.0f;

    float speed = 0.0f;
    float maxSpeed = 20.0f;

    float baseTurnSpeed = 0.5f;

    private void Awake()
    {
        pv = transform.parent.GetComponent<PhotonView>();
    }

    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        speed = walkSpeed;

        if (!pv.IsMine)
        {
            Destroy(playerCam);
            Destroy(rb);
        }
    }

    void Update()
    {
        CameraFacing.transform.rotation = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
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

        Vector3 forward = new Vector3(CameraFacing.transform.forward.x, 0, CameraFacing.transform.forward.z);
        Vector3 right = new Vector3(CameraFacing.transform.right.x, 0, CameraFacing.transform.right.z);
        Vector3 moveDir = forward * moveInput.y + right * moveInput.x;

        rb.velocity = rb.velocity + moveDir * speed * Time.deltaTime;

        // if the player is moving, start rotating them in the direction they're moving
        if (moving)
        {
            Vector3 targetDir = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, baseTurnSpeed * speed * Time.deltaTime);
        }
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

    public void OnLook(InputAction.CallbackContext context)
    {
        // Vector2 lookInput = context.ReadValue<Vector2>();

        // playerCameraRig.m_XAxis.Value += lookInput.x;
        // playerCameraRig.m_YAxis.Value += lookInput.y;
        // Check if the input is a mouse input and alter the cinemachine camera rig for the x-axis speed to input value gain instead of max speed
        // if (context.control.device.name == "Mouse")
        // {
        //     playerCameraRig.m_XAxis.m_MaxSpeed = 300;
        // }
        // else
        // {
        //     playerCameraRig.m_XAxis.m_MaxSpeed = 0;
        // }
    }


}
