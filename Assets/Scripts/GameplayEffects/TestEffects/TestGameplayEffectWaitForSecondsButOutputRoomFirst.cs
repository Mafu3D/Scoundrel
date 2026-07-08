using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

namespace Project.Core
{
    public class TestGameplayEffectWaitForSecondsButOutputRoomFirst : GameplayEffect
    {
        private readonly float waitTime;
        private float elapsedTime;
        private readonly float readoutTime = 0.5f;
        private float timeSinceLastReadout = 0f;

        public TestGameplayEffectWaitForSecondsButOutputRoomFirst(float waitTime)
        {
            this.waitTime = waitTime;
            this.elapsedTime = 0f;
        }

        public override void OnStart()
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            string roomOutput = "";
            if (gameManager.DungeonController.CurrentRoom == null)
            {
                roomOutput = "ROOM IS NULL";
            }
            else
            {
                // Get the contents of the active room
                RuntimeCardModel[] cards = gameManager.DungeonController.CurrentRoom.GetAllCards();
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
            Debug.Log(roomOutput);
        }

        public override Status OnResolve(float deltaTime)
        {
            elapsedTime += deltaTime;
            timeSinceLastReadout += deltaTime;

            if (timeSinceLastReadout >= readoutTime)
            {
                Debug.Log($"Waiting... {elapsedTime}/{waitTime}");
                timeSinceLastReadout = 0f;
            }

            if (elapsedTime >= waitTime)
            {
                Debug.Log($"Waiting... {elapsedTime}/{waitTime}");
                elapsedTime = 0f; // Reset elapsed time for potential reuse
                return Status.Complete;
            }
            return Status.Running;
        }
    }
}