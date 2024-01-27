using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;

    [SerializeField] TMP_InputField inputRoomField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] GameObject startButton;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Bro is connected!");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Bro is in the lobby!");
        MenuManager.instance.OpenMenu("title");
        PhotonNetwork.NickName = "Bean " + Random.Range(0, 10000).ToString("0000");
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

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.instance.OpenMenu("error");
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            Debug.Log(roomList[i].Name);
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);

        }
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("loading");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
    }
}