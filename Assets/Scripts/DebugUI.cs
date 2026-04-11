using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] GameManager gameManager;

    void Update()
    {
        string message = "";
        if (!gameManager.GameHasStarted)
        {
            message = "Game has not started";
        }
        else
        {
            foreach (var card in gameManager.CurrentRoom.Cards)
            {
                if (card != null)
                {
                    message += $"{card.ToString()} / ";
                }
                else
                {
                    message += "NONE / ";
                }
            }
        }
        text.text = message;
    }
}
