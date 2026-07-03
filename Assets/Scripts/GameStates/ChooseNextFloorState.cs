using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class ChooseNextFloorState : State
    {
        private readonly Player player;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
        private readonly DungeonController dungeonController;

        public ChooseNextFloorState(StateMachine stateMachine,
                                    GameProcessQueue<GameplayEffect> gameplayEffectQueue,
                                    Player player,
                                    DungeonController dungeonController) : base(stateMachine)
        {
            this.player = player;
            this.gameplayEffectQueue = gameplayEffectQueue;
            this.dungeonController = dungeonController;
        }

        public override void OnEnter()
        {
            player.SetInteractionState(PlayerInteractionState.UIOnly);
        }

        public override void Update(float deltaTime)
        {
            if (gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                gameplayEffectQueue.ResolveQueue(deltaTime);
            }
        }

        public override void OnExit() { }
    }
}