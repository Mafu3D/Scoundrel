using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public void NewGame()
    {
        if (gameManager.Player.InteractionState != PlayerInteractionState.Full)
        {
            Debug.LogWarning("Player cannot use this button while not in full interaction state.");
            return;
        }
        gameManager.RestartGame();
    }
}
