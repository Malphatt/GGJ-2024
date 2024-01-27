using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomiseCharacter : MonoBehaviour
{
    Transform placement;
    Transform Tin;
    int Type;
    bool Equipped = false;
    GameObject EqupippedObj;
    private void Start()
    {
        Tin = transform.parent.GetComponent<CustomiseManager>().Tin;
        Type = transform.GetComponent<WearableType>().Type;
        placement = transform.parent.GetComponent<CustomiseManager>().wearablePlacement[Type];
    }
    public void ChooseItem(GameObject wearable)
    {
        if (!Equipped)
        {
            Quaternion rot = Quaternion.Euler(-90f, 0f, 0f);
            EqupippedObj = Instantiate(wearable, placement.position, rot, Tin);
            Equipped = true;
        }
        else
        {
            Destroy(EqupippedObj);
            Equipped = false;
        }
    }
}
