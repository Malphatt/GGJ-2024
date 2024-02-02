using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animating : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMoving(bool value)
    {
        animator.SetBool("Moving",value);
    }

}
