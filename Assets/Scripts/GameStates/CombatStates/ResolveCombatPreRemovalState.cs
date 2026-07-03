using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class ResolveCombatPreRemovalState : State
    {
        private readonly Combat combat;
        private readonly Player player;
        private readonly DungeonController dungeonController;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
        private readonly AdvancedScoreKeeper scoreKeeper;

        public ResolveCombatPreRemovalState(Combat combat,
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
            combat.TriggerOnDeathPreRemovalEvents();
        }

        public override void Update(float deltaTime)
        {
            if (!gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                StateMachine.SwitchState(new ResolveCombatPostRemovalState(combat,
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