using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class UpgradeDungeonState : State
    {
        private readonly Player player;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
        private readonly DungeonController dungeonController;
        private readonly GameManager gameManager;

        public UpgradeDungeonState(StateMachine stateMachine,
                                   GameProcessQueue<GameplayEffect> gameplayEffectQueue,
                                   Player player,
                                   DungeonController dungeonController) : base(stateMachine)
        {
            this.player = player;
            this.gameplayEffectQueue = gameplayEffectQueue;
            this.dungeonController = dungeonController;
            ServiceLocator.Global.Get(out gameManager);
        }

        public override void OnEnter()
        {
            player.SetInteractionState(PlayerInteractionState.UIOnly);


            DeckUpgradeChoice deckUpgradeChoice = new(gameManager.DeckUpgrader, 2);

            gameManager.DeckUpgraderView.RegisterDeckUpgradeChoice(deckUpgradeChoice);
            gameManager.DeckUpgraderView.Show();
        }

        public override void Update(float deltaTime)
        {
            if (gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                gameplayEffectQueue.ResolveQueue(deltaTime);
            }
        }

        public override void OnExit()
        {
            gameManager.DeckUpgraderView.DeregisterDeckUpgradeChoice();
            gameManager.DeckUpgraderView.Hide();
        }
    }
}