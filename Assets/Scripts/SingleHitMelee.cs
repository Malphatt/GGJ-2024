using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHitMelee : Weapon
{
    public Animator animator;
    public override void Use()
    {
        Debug.Log("Punch");
        animator.Play("Punch");
    }
}
