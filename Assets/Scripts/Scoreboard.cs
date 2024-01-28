using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;

    Dictionary<Player, ScoreboardItem> scoreboardPlayerDict = new Dictionary<Player, ScoreboardItem>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
    }

    private void AddScoreboardItem(Player player)
    {
        ScoreboardItem item = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardItem>();
        item.Initialise(player);
        scoreboardPlayerDict[player] = item;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        RemoveScoreboardItem(newPlayer);
    }

    private void RemoveScoreboardItem(Player newPlayer)
    {
        Destroy(scoreboardPlayerDict[newPlayer].gameObject);
        scoreboardPlayerDict.Remove(newPlayer);
    }
}
