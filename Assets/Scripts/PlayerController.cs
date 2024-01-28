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

public class PlayerController : MonoBehaviour, IDamagable
{
    private Rigidbody rb;
    Vector2 moveInput;
    public PhotonView pv;
    [SerializeField] Slider slider;
    [SerializeField] Renderer glovesRenderer;

    [SerializeField] Transform scoreListContent;
    [SerializeField] GameObject scoreListPrefab;
    [SerializeField] GameObject[] accessories;
    [SerializeField] GameObject beanMaker;

    PlayerManager playerManager;

    public GameObject Camera;
    public GameObject freeLookCamera;
    public GameObject CameraFacing;
    public Item weapon;
    public GameObject playerObject;

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

    const float maxHealth = 200f;
    public float curHealth = maxHealth;

    private void Awake()
    {
        pv = transform.GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        speed = walkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        glovesRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        if (!pv.IsMine)
        {
            Destroy(Camera);
            Destroy(freeLookCamera);
            Destroy(rb);
        }
        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        pv.RPC("RPC_PlayerAccessories",RpcTarget.OthersBuffered,Launcher.instance.accessories);
        PlayerAccessories(Launcher.instance.accessories);
        UpdatePlayerList();
    }
    public void UpdatePlayerList()
    {
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 1; i < scoreListContent.childCount; i++)
        {
            Destroy(scoreListContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(scoreListPrefab, scoreListContent);
            scoreListContent.GetChild(i + 1).gameObject.GetComponent<TMP_Text>().text = players[i].NickName + " - 0";
        }
    }
    void Update()
    {
        if (!pv.IsMine) { return; }
        CameraFacing.transform.rotation = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        cooldown += Time.deltaTime;
        if (((SingleHitMelee)weapon).animator.GetBool("Punch") == false)
        {
            unhit = true;
        }
    }

    void FixedUpdate()
    {
        slider.value = curHealth;
        if (curHealth <= 0)
        {
            beanMaker.SetActive(true);
        }
        if (!pv.IsMine) { return; }
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
                cooldown = 0f;
            }

        }
    }

    public void TakeDamage(float damage, GameObject other)
    {
        animator.SetBool("Damaged", true);
        Vector3 velocity = (gameObject.transform.position - position) * 36f;
        velocity = new Vector3(velocity.x, Mathf.Max(15f,velocity.y), velocity.z);
        pv.RPC("RPC_TakeDamage", pv.Owner, damage,velocity);
    }

    void PlayerAccessories(bool[] enabledList)
    {
        if (!pv.IsMine)
        {
            return;
        }

        for (int i = 0; i < accessories.Length; i++)
        {
            accessories[i].SetActive(enabledList[i]);
        }
    }

    [PunRPC]
    void RPC_PlayerAccessories(bool[] enabledList)
    {
        if (!pv.IsMine)
        {
            for (int i = 0; i < accessories.Length; i++)
            {
                accessories[i].SetActive(enabledList[i]);
            }
        }

    }

    [PunRPC]
    void RPC_TakeDamage(float damage, Vector3 velocity, PhotonMessageInfo info)
    {

        curHealth -= damage;
        slider.value = curHealth;
        rb.velocity += velocity;

        if (curHealth <= 0)
        {
            Die();
            PlayerManager.Find(info.Sender);
        }
    }

    public void Die()
    {
        playerManager.Die();
    }

}
