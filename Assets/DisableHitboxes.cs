using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHitboxes : MonoBehaviour
{
    public BoxCollider fists;
    public BoxCollider knife;

    public void DisableHitbox(string hitboxName)
    {
        switch (hitboxName)
        {
            case "fists":
                fists.enabled = false;
                break;
            case "knife":
                knife.enabled = false;
                break;
            default:
                Debug.Log("Invalid hitbox name");
                break;
        }
    }

    public void EnableHitbox(string hitboxName)
    {
        switch (hitboxName)
        {
            case "fists":
                fists.enabled = true;
                break;
            case "knife":
                knife.enabled = true;
                break;
            default:
                Debug.Log("Invalid hitbox name");
                break;
        }
    }
}
