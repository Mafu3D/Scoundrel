using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class ResolvingActionsState : State
    {
        private readonly Player player;
        private readonly DungeonController dungeonController;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;

        public ResolvingActionsState(StateMachine stateMachine,
                                GameProcessQueue<GameplayEffect> gameplayEffectQueue,
                                Player player,
                                DungeonController dungeonController) : base(stateMachine)
        {
            this.player = player;
            this.dungeonController = dungeonController;
            this.gameplayEffectQueue = gameplayEffectQueue;
        }

        public override void OnEnter()
        {
            player.SetInteractionState(PlayerInteractionState.UIOnly);
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