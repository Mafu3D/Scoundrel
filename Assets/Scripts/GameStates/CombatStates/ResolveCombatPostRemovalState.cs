using Project.Core;
using Project.Core.StateMachineSystem;
using Project.Decks;
using UnityEngine;

namespace Project.GameStates
{
    public class ResolveCombatPostRemovalState : State
    {
        private readonly Combat combat;
        private readonly Player player;
        private readonly DungeonController dungeonController;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
        private readonly AdvancedScoreKeeper scoreKeeper;

        public ResolveCombatPostRemovalState(Combat combat,
                                             StateMachine stateMachine,
                                             GameProcessQueue<GameplayEffect> gameplayEffectQueue,
                                             Player player,
                                             DungeonController dungeonController,
                                             AdvancedScoreKeeper scoreKeeper) : base(stateMachine)
        {
            this.combat = combat;
            this.player = player;
            this.dungeonController = dungeonController;
            this.gameplayEffectQueue = gameplayEffectQueue;
            this.scoreKeeper = scoreKeeper;
        }

        public override void OnEnter()
        {
            player.SetInteractionState(PlayerInteractionState.UIOnly);

            player.AddGold(1);

            // For now, all monsters die in combat
            MonsterCardModel defender = combat.combatReport.Defender;
            dungeonController.CurrentRoom.TryRemoveCard(defender);
            defender.BuffManager.CleanupChildren();

            // Add gold if its the last card in the room
            if (dungeonController.CurrentRoom.IsEmpty)
            {
                player.AddGold(2);
            }

            scoreKeeper.AddToScore(defender);
            scoreKeeper.IncRoomMultiplier();

            combat.TriggerOnDeathPostRemovalEvents();

            defender.Kill();
        }

        public override void Update(float deltaTime)
        {
            if (!gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                StateMachine.SwitchState(new RoomActiveState(StateMachine,
                                                             gameplayEffectQueue,
                                                             player,
                                                             dungeonController));
                return;
            }

            gameplayEffectQueue.ResolveQueue(deltaTime);
        }

        public override void OnExit() { }
    }
}