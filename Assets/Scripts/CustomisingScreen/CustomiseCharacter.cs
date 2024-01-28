using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomiseCharacter : MonoBehaviour
{
    Transform placement;
    Transform Tin;
    int Type;
    string[] names = { "bowtie", "tie", "Chain", "Moustache", "DarkGlasses", "PinkGlasses", "PropelerHat", "TopHat" };
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
        int index = Array.IndexOf(names,wearable.name);
        if (!Equipped)
        {
            ThisImage.color = Color.green;
            Quaternion rot = Quaternion.Euler(-90f, 0f, 0f);
            EqupippedObj = Instantiate(wearable, placement.position, rot, Tin);
            Equipped = true;
            Launcher.instance.accessories[index] = true;
        }
        else
        {
            ThisImage.color = Color.white;
            Destroy(EqupippedObj);
            Equipped = false;
            Launcher.instance.accessories[index] = false;
        }
    }
}
