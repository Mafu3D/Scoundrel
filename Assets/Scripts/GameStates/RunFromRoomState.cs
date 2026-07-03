using Project.Core;
using Project.Core.StateMachineSystem;
using Project.Decks;

namespace Project.GameStates
{
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

            player.BuffManager.HandleOnPlayerRun();
            player.Weapon?.BuffManager.HandleOnPlayerRun();
            foreach(RuntimeCardModel card in dungeonController.CurrentRoom.RemainingCards())
            {
                card.BuffManager.HandleOnPlayerRun();
            }
        }

        public override void Update(float deltaTime)
        {
            if (gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                gameplayEffectQueue.ResolveQueue(deltaTime);
                return;
            }

            dungeonController.RunFromRoom();
            StateMachine.SwitchState(new OpenNewRoomState(StateMachine, gameplayEffectQueue, player, dungeonController, scoreKeeper));
        }

        public override void OnExit() { }
    }
}

