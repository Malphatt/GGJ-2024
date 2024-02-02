using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
using System.Linq;
using FishNet.Object;

public class FishPlayerController : NetworkBehaviour, IDamagable
{
    private Rigidbody rb;
    Vector2 moveInput;
    public GameObject Camera;
    public GameObject freeLookCamera;
    public GameObject CameraFacing;
 
    public GameObject playerObject;

    bool unhit;

    bool moving = false;
    bool isGrounded = false;
    bool isRunning = false;
    bool isJumping = false;

    float walkSpeed = 25.0f;
    float runSpeed = 35.0f;
    float jumpForce = 15.0f;

    float speed = 0.0f;
    float maxWalkSpeed = 10.0f;
    float maxRunSpeed = 15.0f;

    float baseTurnSpeed = 0.25f;
    float cooldownNum = 0.7f;
    float cooldown = 0.0f;

    public const float maxHealth = 10f;
    public float curHealth = maxHealth;

    private void Awake()
    {
    }

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        speed = walkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        CameraFacing.transform.rotation = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
    }

    void FixedUpdate()
    {

        
        // if the player is moving too fast, slow them down
        if (isRunning && rb.velocity.magnitude > maxRunSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * maxRunSpeed + new Vector3(0, rb.velocity.y, 0);
        }
        else if (!isRunning && rb.velocity.magnitude > maxWalkSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * maxWalkSpeed + new Vector3(0, rb.velocity.y, 0);
        }

        // if the player isn't moving and is on the ground, slow them down
        Ray groundedRay = new Ray(playerObject.transform.position, Vector3.down);

        isGrounded = Physics.Raycast(groundedRay, 1.2f);

        if (isRunning)
            speed = runSpeed;
        else
            speed = walkSpeed;

        if (!moving && isGrounded)
        {
            rb.velocity = rb.velocity * 0.9f;
        }

        if (isGrounded && isJumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
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
            playerObject.transform.rotation = Quaternion.Lerp(playerObject.transform.rotation, targetRotation, baseTurnSpeed * speed * Time.deltaTime);
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

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            //Play sfx
            isJumping = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isJumping = false;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (cooldown > cooldownNum)
            {
                cooldown = 0f;

                //Play sfx
            }
        }
    }

    public void TakeDamage(float damage, GameObject other, Vector3 position)
    {
        Vector3 velocity = (gameObject.transform.position - position) * 36f;
        velocity = new Vector3(velocity.x, Mathf.Max(15f, velocity.y), velocity.z);

        //Play sfx
    }

    public void Die()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

}
