using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishUsernameDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    private void Start()
    {
        text.text = "Mal";
    }
}
