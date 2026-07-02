using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextRoomButton : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Button button;

    public void Next()
    {
        if (gameManager.Player.InteractionState != PlayerInteractionState.Full)
        {
            Debug.LogWarning("Player cannot use this button (Next) while not in full interaction state.");
            return;
        }
        gameManager.GoToNextRoom();
    }

    void Update()
    {
        if (gameManager.DungeonController.CanGoToNextRoom)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
