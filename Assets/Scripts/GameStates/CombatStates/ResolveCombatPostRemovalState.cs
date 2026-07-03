using Project.Core;
using Project.Core.StateMachineSystem;
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

            if (!player.HasEnteredTheRoom)
            {
                // Player enter room
                player.EnterNewRoom();
                // TODO: add all player enter room triggers here
            }

            player.AddGold(1);

            // For now, all monsters die in combat
            MonsterCardModel defender = combat.combatReport.Defender;
            dungeonController.CurrentRoom.TryRemoveCard(defender);

            // Add gold if its the last card in the room
            if (dungeonController.CurrentRoom.IsEmpty)
            {
                player.AddGold(2);
            }

            scoreKeeper.AddToScore(defender);
            // TEMP: Inc score keeper multiplier here
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