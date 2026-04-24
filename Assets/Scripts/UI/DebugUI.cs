using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject mainContainer;
    [SerializeField] TMP_Text bottomText;
    [SerializeField] bool startEnabled;

    void Start()
    {
        if (startEnabled)
        {
            mainContainer.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Period))
        {
            ToggleUI();
        }

        if (!mainContainer.activeSelf)
        {
            return;
        }

        string message = "";
        if (!gameManager.GameHasStarted)
        {
            message = "Game has not started";
        }
        else
        {
            message += "Room: ";
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

            if (gameManager.Player.Weapon != null)
            {
                message += $" Weapon: {gameManager.Player.Weapon.GetWeaponInfoString()}";
            }
            else
            {
                message += "Weapon: NONE";
            }
        }
        bottomText.text = message;
    }

    private void ToggleUI()
    {
        if (mainContainer.activeSelf)
        {
            mainContainer.SetActive(false);
        }
        else
        {
            mainContainer.SetActive(true);
        }
    }
}
