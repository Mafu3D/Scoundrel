using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class OpenNewRoomState : State
    {
        private readonly Player player;
        private readonly DungeonController dungeonController;
        private readonly AdvancedScoreKeeper scoreKeeper;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;

        public OpenNewRoomState(StateMachine stateMachine,
                                GameProcessQueue<GameplayEffect> gameplayEffectQueue,
                                Player player,
                                DungeonController dungeonController,
                                AdvancedScoreKeeper scoreKeeper) : base(stateMachine)
        {
            this.player = player;
            this.dungeonController = dungeonController;
            this.scoreKeeper = scoreKeeper;
            this.gameplayEffectQueue = gameplayEffectQueue;
        }

        public override void OnEnter()
        {
            // Run any logic that needs to be run when a new room is opened for the first time
            player.SetInteractionState(PlayerInteractionState.UIOnly);

            player.RoundReset();
            scoreKeeper.ResetRoomMultiplier();
            dungeonController.OpenNewRoom();
        }

        public override void Update(float deltaTime)
        {
            if (gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                gameplayEffectQueue.ResolveQueue(deltaTime);
                return;
            }

            StateMachine.SwitchState(new RoomActiveState(StateMachine, gameplayEffectQueue, player, dungeonController));
        }

        public override void OnExit() { }
    }

    public class RunFromRoomState : State
    {
        private readonly Player player;
        private readonly DungeonController dungeonController;
        private readonly AdvancedScoreKeeper scoreKeeper;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;

        public RunFromRoomState(StateMachine stateMachine,
                                GameProcessQueue<GameplayEffect> gameplayEffectQueue,
                                Player player,
                                DungeonController dungeonController,
                                AdvancedScoreKeeper scoreKeeper) : base(stateMachine)
        {
            this.player = player;
            this.dungeonController = dungeonController;
            this.scoreKeeper = scoreKeeper;
            this.gameplayEffectQueue = gameplayEffectQueue;
        }

        public override void OnEnter()
        {
            // Run any logic that needs to be run when a new room is opened for the first time
            player.SetInteractionState(PlayerInteractionState.UIOnly);

            dungeonController.RunFromRoom();

            // Handle all on run buffs here
        }

        public override void Update(float deltaTime)
        {
            if (gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                gameplayEffectQueue.ResolveQueue(deltaTime);
                return;
            }

            StateMachine.SwitchState(new OpenNewRoomState(StateMachine, gameplayEffectQueue, player, dungeonController, scoreKeeper));
        }

        public override void OnExit() { }
    }
}

