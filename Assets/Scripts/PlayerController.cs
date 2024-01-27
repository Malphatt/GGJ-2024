using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour, IDamagable
{
    private Rigidbody rb;
    Vector2 moveInput;
    PhotonView pv;

    public GameObject Camera;
    public GameObject freeLookCamera;
    public GameObject CameraFacing;
    public Item weapon;
    public GameObject playerObject;

    bool unhit;

    bool moving = false;
    bool isGrounded = false;
    bool isRunning = false;

    float walkSpeed = 20.0f;
    float runSpeed = 30.0f;

    float speed = 0.0f;
    float maxSpeed = 20.0f;

    float baseTurnSpeed = 0.5f;
    float cooldownNum = 0.7f;
    float cooldown = 0.0f;

    const float maxHealth = 200f;
    float curHealth = maxHealth;

    private void Awake()
    {
        pv = transform.GetComponent<PhotonView>();
    }

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        speed = walkSpeed;

        if (!pv.IsMine)
        {
            Destroy(Camera);
            Destroy(freeLookCamera);
            Destroy(rb);
        }
    }

    void Update()
    {
        if (!pv.IsMine) { return; }
        CameraFacing.transform.rotation = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        cooldown += Time.deltaTime;
        if (((SingleHitMelee)weapon).animator.GetFloat("Punch") < 0)
        {
            unhit = true;
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
    
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (cooldown > cooldownNum)
            { 
                weapon.Use();
                cooldown = 0f;
            }

        }
    }

    public void TakeDamage(float damage)
    {
        if (((SingleHitMelee)weapon).animator.GetFloat("Punch") > 0 && unhit)
        {
            pv.RPC("RPC_TakeDamage", RpcTarget.All, damage);
            unhit = false;
        }
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!pv.IsMine)
        {
            return;
        }

        curHealth -= damage;
        Debug.Log(curHealth);
    }

}
