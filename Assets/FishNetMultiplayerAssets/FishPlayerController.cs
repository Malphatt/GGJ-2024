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
using FishNet.Component.Animating;
using FishNet.Connection;

public class FishPlayerController : NetworkBehaviour, IDamagable
{
    [SerializeField] Rigidbody rb;
    Vector2 moveInput;

    [SerializeField] TMP_Text text;
    [SerializeField] Renderer glovesRenderer;
    [SerializeField] GameObject[] accessories;
    [SerializeField] GameObject beanMaker;
    private NetworkAnimator networkAnimator;


    public GameObject Camera;
    public GameObject freeLookCamera;
    public GameObject CameraFacing;
    public GameObject playerObject;

    public Item weapon;
    public Item fist;
    public Item knife;

    public AudioClip hit;
    public AudioClip punch;
    public AudioClip jump;

    public Animator animator;

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

    public bool isMine = true;

    private void Awake()
    {
        networkAnimator = GetComponent<NetworkAnimator>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            Destroy(Camera);
            Destroy(CameraFacing);
            Destroy(freeLookCamera);
            Destroy(rb);
            isMine = false;
            Debug.Log("Not Owner");
            return;
        }
        speed = walkSpeed;
        Camera.SetActive(true);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    

    void Start()
    {
        glovesRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
    void Update()
    {
        if (!isMine) { return; }
        cooldown += Time.deltaTime;
        text.text = "Health: " + curHealth.ToString();
        CameraFacing.transform.rotation = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
    }

    void FixedUpdate()
    {
        if (!isMine) { return; }
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
                weapon.Use();
                Debug.Log(weapon.itemInfo.itemName);
                animator.SetBool(weapon.itemInfo.itemName,true);
                cooldown = 0f;

                //Play sfx
                GetComponent<AudioSource>().PlayOneShot(punch);
            }
        }
    }

    public void TakeDamage(float damage, GameObject other, Vector3 position)
    {
        networkAnimator.Play("Armature_Lid Moving");
        Vector3 velocity = (gameObject.transform.position - position) * 36f;
        velocity = new Vector3(velocity.x, Mathf.Max(15f, velocity.y), velocity.z);

        //Play sfx
        GetComponent<AudioSource>().PlayOneShot(hit);

        RpcTakeDamage(base.Owner, damage);
    }

    [TargetRpc]
    private void RpcTakeDamage(NetworkConnection conn, float damage)
    {
        //This might be something you only want the owner to be aware of.
        Debug.Log(damage);
        curHealth -= damage;
    }

    void PlayerAccessories(bool[] enabledList)
    {

        for (int i = 0; i < accessories.Length; i++)
        {
            accessories[i].SetActive(enabledList[i]);
        }
    }
    public void Die()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
    public void Pickup(string pickupName)
    {
        if (pickupName == "knife")
        {
            weapon = knife;
            fist.gameObject.SetActive(false);
            knife.gameObject.SetActive(true);
        }
        else
        {
            weapon = fist;
            fist.gameObject.SetActive(true);
            knife.gameObject.SetActive(false);
        }
    }
}
