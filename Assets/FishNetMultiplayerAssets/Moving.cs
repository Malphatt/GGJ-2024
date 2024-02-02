using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : NetworkBehaviour
{

    [SerializeField] float moveSpeed = 5f;
    private CharacterController characterController;
    [SerializeField] GameObject myCamera;
    private Animating animating;
    // Update is called once per frame

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animating = GetComponent<Animating>();
    }
    private void Start()
    {
    }
    void Update()
    {
        if (!base.IsOwner) { return; }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 offset = new Vector3(horizontal* moveSpeed, Physics.gravity.y, vertical*moveSpeed) * Time.deltaTime;
        characterController.Move(offset);

        bool moving = (horizontal != 0f || vertical != 0f);

        animating.SetMoving(moving);
    }
}
