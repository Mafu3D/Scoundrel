using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class ExitFloorState : State
    {
        private readonly Player player;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
        private readonly DungeonController dungeonController;

        public ExitFloorState(StateMachine stateMachine,
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
            player.FloorReset();
            dungeonController.ExitCurrentFloor();
        }

        public override void Update(float deltaTime)
        {
            if (!gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                StateMachine.SwitchState(new UpgradeDungeonState(StateMachine,
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