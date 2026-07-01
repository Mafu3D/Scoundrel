using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class OpenNewRoomState : State
    {
        private readonly Player player;
        private readonly DungeonController dungeonController;
        private readonly AdvancedScoreKeeper scoreKeeper;

        public OpenNewRoomState(StateMachine stateMachine,
                                Player player,
                                DungeonController dungeonController,
                                AdvancedScoreKeeper scoreKeeper) : base(stateMachine)
        {
            this.player = player;
            this.dungeonController = dungeonController;
            this.scoreKeeper = scoreKeeper;
        }

        public override void OnEnter()
        {
            // Run any logic that needs to be run when a new room is opened for the first time
            player.RoundReset();
            scoreKeeper.ResetRoomMultiplier();
            dungeonController.OpenNewRoom();
        }

        public override void Update(float time)
        {
            // Resolve the queue

            StateMachine.SwitchState(new RoomActiveState(StateMachine));
        }

        public override void OnExit() { }
    }
}