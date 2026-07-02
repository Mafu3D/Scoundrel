using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class RoomActiveState : State
    {
        private readonly Player player;
        private readonly DungeonController dungeonController;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;

        public RoomActiveState(StateMachine stateMachine,
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
            player.SetInteractionState(PlayerInteractionState.Full);
        }

        public override void Update(float deltaTime)
        {
            dungeonController.Update();
            player.Update();

            // If the queue needs to be resolved, disable player interaction and resolve the queue
            if (gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                player.SetInteractionState(PlayerInteractionState.UIOnly);
                gameplayEffectQueue.ResolveQueue(deltaTime);
                return;
            }

            player.SetInteractionState(PlayerInteractionState.Full);
        }

        public override void OnExit() { }
    }
}