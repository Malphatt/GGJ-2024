using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardItem : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text scoreText;

    public void Initialise(Player player)
    {
        usernameText.text = player.NickName;
    }
}
