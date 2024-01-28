using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHitMelee : Weapon
{
    public Animator animator;

    public override void Use()
    {
        Debug.Log(itemInfo.itemName);
        animator.SetBool(itemInfo.itemName, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.gameObject.GetComponent<PlayerController>().pv.IsMine)
            {
                Debug.Log("Hit a player!" + other.gameObject.name);
                other.gameObject.GetComponent<IDamagable>()?.TakeDamage(((WeaponInfo)itemInfo).weaponDamage, other.gameObject, gameObject.transform.position);
            }
        }
    }
}
