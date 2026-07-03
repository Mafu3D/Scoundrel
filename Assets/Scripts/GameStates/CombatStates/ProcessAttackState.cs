using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class ProcessDamageState : State
    {
        private readonly Combat combat;
        private readonly Player player;
        private readonly DungeonController dungeonController;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
        private readonly AdvancedScoreKeeper scoreKeeper;

        public ProcessDamageState(Combat combat,
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
            combat.ProcessDamage();
            combat.TriggerPostAttackEvents();
        }

        public override void Update(float deltaTime)
        {
            if (!gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                StateMachine.SwitchState(new ResolveCombatPreRemovalState(combat,
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