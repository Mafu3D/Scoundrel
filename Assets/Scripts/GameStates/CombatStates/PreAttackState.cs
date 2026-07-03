using Project.Core;
using Project.Core.StateMachineSystem;
using Project.Decks;

namespace Project.GameStates
{
    public class PreAttackState : State
    {
        private readonly Combat combat;
        private readonly Player player;
        private readonly DungeonController dungeonController;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
        private readonly AdvancedScoreKeeper scoreKeeper;

        public PreAttackState(Combat combat,
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
                player.Weapon.BuffManager.HandleOnPlayerEnterRoom();
                foreach(RuntimeCardModel card in dungeonController.CurrentRoom.RemainingCards())
                {
                    card.BuffManager.HandleOnPlayerEnterRoom();
                }
            }

            combat.TriggerPreAttackEvents();
        }

        public override void Update(float deltaTime)
        {
            if (!gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                StateMachine.SwitchState(new ProcessDamageState(combat,
                                                                StateMachine,
                                                                gameplayEffectQueue,
                                                                player,
                                                                dungeonController,
                                                                scoreKeeper));
                return;
            }

            gameplayEffectQueue.ResolveQueue(deltaTime);
        }

        public override void OnExit() { }
    }
}