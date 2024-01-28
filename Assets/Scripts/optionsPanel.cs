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
        }
        else
        {
            optionsPanelObject.SetActive(true);
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("loading");
    }
}
