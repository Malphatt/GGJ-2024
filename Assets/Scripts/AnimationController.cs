using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PunchControl()
    {
        animator.SetBool("fists", false);
    }

    public void DamageControl()
    {
        animator.SetBool("Damaged", false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            animator.SetBool("Emote", true);
        }
    }

    public void EmoteControl()
    {
        animator.SetBool("Emote", false);
    }
    public void KnifeControl()
    {
        animator.SetBool("knife", false);
    }
}
