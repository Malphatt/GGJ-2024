using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [SerializeField] menu[] menus;

    private void Awake()
    {
        instance= this;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                OpenMenu(menus[i]);
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    private void CloseMenu(menu menuScript)
    {
        menuScript.Close();
    }

    public void OpenMenu(menu menuScript)
    {
        menuScript.Open();


    }
}
