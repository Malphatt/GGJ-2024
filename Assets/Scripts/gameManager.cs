using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField] int roundNumber;
    [SerializeField] float roundTimer;
    PhotonView pv;
    
    [SerializeField] int maxRounds = 3;
    [SerializeField] float maxTime = 180f;

    [SerializeField] List<GameObject> players = new List<GameObject>();
    [SerializeField] List<GameObject> winners = new List<GameObject>();

    [SerializeField] TMP_Text roundTimerText;
    [SerializeField] TMP_Text roundNumberText;


    private void Awake()
    {
        pv = transform.GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        roundNumber = 1;
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("NewRound", RpcTarget.All, roundNumber);
        }
    }

    // FixedUpdate
    void FixedUpdate()
    {

        //Decrease Timer
        roundTimer -= Time.deltaTime;
        roundTimerText.text = roundTimer.ToString("F2");
        players.Clear();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }

        //Check if timer is 0 or all players are dead
        if (roundTimer <= 0 || players.Count <= 1)
        {
            //Check if all rounds are done
            if (roundNumber >= maxRounds)
            {
                //End Game
                Debug.Log("Game Over");

                //Disable the timer
                roundTimerText.text = "";
                roundNumberText.text = "";
            }
            else
            {
                //End Round
                Debug.Log("Round Over");
                roundNumber++;
                if (PhotonNetwork.IsMasterClient) {
                    pv.RPC("NewRound", RpcTarget.All, roundNumber);
                }
                foreach(GameObject player in players)
                {
                    if (pv.IsMine)
                    {
                        player.GetComponent<PlayerController>().Die();
                    }
                }
            }
        }

        //Add the winners name to a list
        for (int i = 0; i < players.Count; i++)
        {
            winners.Add(players[i]);
        }

    }

    [PunRPC]
    public void NewRound(int roundNum)
    {
        if (pv.IsMine)
        {

        }
    }

}
