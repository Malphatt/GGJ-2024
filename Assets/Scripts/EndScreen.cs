using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TMP_Text winner;
    private void Awake()
    {
        winner.text = PlayerManager.winner + " Wins!!";

        //After 10 seconds, return to menu
        StartCoroutine(ReturnToMenu());
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(10);
        Photon.Pun.PhotonNetwork.LoadLevel(0);
    }
}
