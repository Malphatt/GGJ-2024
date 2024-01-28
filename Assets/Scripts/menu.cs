using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    public string menuName;
    public bool open;
    public GameObject Can;
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
        if (menuName == "Customise")
        {
            Can.SetActive(true);
        }
    }

    public void Close()
    {
        open = false;
        if (menuName == "Customise")
        {
            Can.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
