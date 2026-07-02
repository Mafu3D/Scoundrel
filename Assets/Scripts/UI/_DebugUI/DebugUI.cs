using System;
using System.Collections;
using System.Collections.Generic;
using Mafu.DebugConsole;
using Project.Decks;
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
        [SerializeField] TMP_Text playerInteractionStateDebugText;
        [SerializeField] TMP_Text queueDebugText;
        [SerializeField] TMP_Text weaponDebugText;
        [SerializeField] TMP_Text playerStatsDebugText;
        [SerializeField] TMP_Text scoreDebugText;
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
            UpdatePlayerInteractionStateDebugText();
            UpdateQueueDebugText();
            UpdateWeaponDebugText();
            UpdatePlayerStatsDebugText();
            UpdateScoreDebugText();
        }

        private void UpdateScoreDebugText()
        {
            scoreDebugText.text = $"Score: {gameManager.ScoreKeeper.GetScore()}";
        }

        private void UpdatePlayerStatsDebugText()
        {
            string resources = $"Health: {gameManager.Player.CurrentHealth} / {gameManager.Player.MaxHealth}  -  Gold: {gameManager.Player.CurrentGold:D2}";
            string run = $"Run On CD: {gameManager.Player.RunTokenOnCooldown} ({gameManager.Player.RunCooldownCounter})  -  Run Tokens: {gameManager.Player.ExtraRunTokens:D2}";
            string playerActions = $"Can Run: {gameManager.Player.CanRun}  -  Can Go Next: {gameManager.DungeonController.CanGoToNextRoom}";
            playerStatsDebugText.text = $"{resources}\n{run}\n{playerActions}";
        }

        private void UpdateWeaponDebugText()
        {
            string message = "";
            if (gameManager.Player.Weapon != null)
            {
                message= $" Weapon: {gameManager.Player.Weapon.GetWeaponInfoString()}";
                if (gameManager.Player.Weapon.BuffManager.GetBuffs().Count > 0)
                {
                    message += "*";
                }
            }
            else
            {
                message= "Weapon: NONE";
            }
            weaponDebugText.text = message;
        }

        private void UpdateQueueDebugText()
        {
            string currentItem = gameManager.GameplayEffectQueue.QueueNeedsToBeResolved ? gameManager.GameplayEffectQueue.GetCurrentItem().GetType().Name : "Empty";
            queueDebugText.text = $"Queue: {currentItem}";
        }

        private void UpdatePlayerInteractionStateDebugText()
        {
            playerInteractionStateDebugText.text = $"Player Interaction State: {gameManager.Player.InteractionState}";
        }

        private void UpdateStateDebugText()
        {
            stateDebugText.text = $"Game State: {gameManager.DEBUG_GetCurrentGameStateString()}";
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
            // Get the room number
            string roomNumber = $"Floor: {gameManager.DungeonController.GetFloorNumber():D2} - Room: {gameManager.DungeonController.GetRoomNumber():D2}";


            string roomOutput = "";
            if (gameManager.DungeonController.CurrentRoom == null)
            {
                roomOutput = "ROOM IS NULL";
            }
            else
            {
                // Get whether the player has entered the room or not
                string entered = gameManager.Player.HasEnteredTheRoom ? "Entered" : "Not Entered";
                roomNumber += $": {entered}";

                // Get the contents of the active room
                RuntimeCardModel[] cards = gameManager.DungeonController.CurrentRoom.GetCards();
                for (int i = 0; i < cards.Length; i++)
                {
                    RuntimeCardModel card = cards[i];
                    string slotOutput = $"Slot {i} (No Mod): "; // Add modifiers later once thats implemented
                    if (card != null)
                    {
                        slotOutput += $"{card.ToString()}";
                        if (card.BuffManager.GetBuffs().Count > 0)
                        {
                            slotOutput += "*";
                        }
                    }
                    else
                    {
                        slotOutput += "NONE";
                    }
                    roomOutput += $"{slotOutput}\n";
                }
            }
            roomDebugText.text = $"{roomNumber}\n{roomOutput}";
        }
    }
}
