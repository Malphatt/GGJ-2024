using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField] int roundNumber;
    [SerializeField] float roundTimer;
    
    [SerializeField] int maxRounds = 3;
    [SerializeField] float maxTime = 60f;

    [SerializeField] List<GameObject> players = new List<GameObject>();
    
    [SerializeField] TMP_Text roundTimerText;
    [SerializeField] TMP_Text roundNumberText;

    [SerializeField] GameObject roundWinnerTextObj;
    [SerializeField] TMP_Text roundWinnerText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        roundNumber = 1;
        roundTimer = maxTime;
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        //Decrease Timer
        roundTimer -= Time.deltaTime;
        roundTimerText.text = "Time: " + roundTimer.ToString("F2");

        //Remove any players who curHealth is 0
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].GetComponent<PlayerController>().curHealth <= 0)
            {
                players.Remove(players[i]);
            }
        }
        
        //Check if timer is 0 or all players are dead
        if (roundTimer <= 0 || players.Count <= 1)
        {
            //Check if all rounds are done
            if (roundNumber >= maxRounds)
            {
                //End Game
                Debug.Log("Game Over");
            }
            else
            {
                //End Round
                Debug.Log("Round Over");
                roundNumber++;
                roundTimer = maxTime;
                roundNumberText.text = "Round: " + roundNumber;
            }
        }
    }
}
