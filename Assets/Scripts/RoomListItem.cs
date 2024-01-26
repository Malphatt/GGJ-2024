using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    RoomInfo info;
    public void Setup(RoomInfo _info)
    {
        text.text = _info.Name;
        info = _info;
    }
    public void OnClick()
    {
        Launcher.instance.JoinRoom(info);
    }
}
