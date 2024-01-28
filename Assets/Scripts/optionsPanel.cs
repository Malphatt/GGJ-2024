using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionsPanel : MonoBehaviour
{
    public GameObject optionsPanelObject;

    private void Update()
    {
        //If escape pressed toggle options
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggleOptions();
        }
    }

    public void toggleOptions()
    {
        if (optionsPanelObject.activeSelf)
        {
            optionsPanelObject.SetActive(false);
            //Set focus on game
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            optionsPanelObject.SetActive(true);
            //Set focus on UI
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("loading");
    }
}
