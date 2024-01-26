using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField inputRoomField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Bro is connected!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Bro is in the lobby!");
        MenuManager.instance.OpenMenu("title");
    }

    public void CreatRoom()
    {
        if (string.IsNullOrEmpty(inputRoomField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(inputRoomField.text);
        MenuManager.instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        roomNameText.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        MenuManager.instance.OpenMenu("roomMenu");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("loading");

    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("title");
    }
}
