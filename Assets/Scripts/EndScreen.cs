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
    }
}
