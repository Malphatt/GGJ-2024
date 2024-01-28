using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomiseCharacter : MonoBehaviour
{
    Transform placement;
    Transform Tin;
    int Type;
    bool Equipped = false;
    GameObject EqupippedObj;
    Image ThisImage;
    private void Start()
    {
        ThisImage = GetComponent<Image>();
        Tin = transform.parent.GetComponent<CustomiseManager>().Tin;
        Type = transform.GetComponent<WearableType>().Type;
        placement = transform.parent.GetComponent<CustomiseManager>().wearablePlacement[Type];
    }
    public void ChooseItem(GameObject wearable)
    {
        if (!Equipped)
        {
            ThisImage.color = Color.green;
            Quaternion rot = Quaternion.Euler(-90f, 0f, 0f);
            EqupippedObj = Instantiate(wearable, placement.position, rot, Tin);
            Equipped = true;
        }
        else
        {
            ThisImage.color = Color.white;
            Destroy(EqupippedObj);
            Equipped = false;
        }
    }
}
