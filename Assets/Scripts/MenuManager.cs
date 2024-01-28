using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public GameObject Can;
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
                if(menuName == "Customise")
                {
                    Can.SetActive(true);
                }
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
