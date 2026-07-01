using System;
using System.Collections;
using System.Collections.Generic;
using Mafu.DebugConsole;
using TMPro;
using UnityEngine;

namespace Project.UI.DebugUI
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] GameManager gameManager;
        [SerializeField] GameObject mainContainer;
        [SerializeField] TMP_Text roomDebugText;
        [SerializeField] TMP_Text stateDebugText;
        [SerializeField] bool startEnabled;

        DebugCommandLine debugCommandLine;

        void Awake()
        {
            debugCommandLine = GetComponent<DebugCommandLine>();
        }

        void Start()
        {
            if (startEnabled)
            {
                mainContainer.SetActive(true);
            }
        }

        void Update()
        {
            if (!mainContainer.activeSelf)
            {
                return;
            }

            UpdateStateDebugText();
            UpdateRoomDebugText();
        }

        private void UpdateStateDebugText()
        {
            stateDebugText.text = $"Current State: {gameManager.DEBUG_GetCurrentGameStateString()}";
        }

        public void OnToggleDebug()
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

        private void UpdateRoomDebugText()
        {
            string message = "";
            if (gameManager.DungeonController.CurrentRoom == null)
            {
                message = "Room is null";
            }
            else
            {
                string entered = gameManager.Player.HasEnteredTheRoom ? "Entered" : "Not Entered";
                message += $"Room ({entered}): ";
                foreach (var card in gameManager.DungeonController.CurrentRoom.GetCards())
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
            roomDebugText.text = message;
        }
    }
}
