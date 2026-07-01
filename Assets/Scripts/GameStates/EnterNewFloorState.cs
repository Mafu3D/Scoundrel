using Project.Core.StateMachineSystem;

namespace Project.GameStates
{

    /// <summary>
    /// This state is responsible for handling the transition to a new floor in the dungeon.
    /// It increments the floor number, resets the room number, and invokes any necessary events to notify other systems of the change.
    /// </summary>
    public class EnterNewFloorState : State
    {
        private readonly Player player;
        private readonly DungeonController dungeonController;
        private readonly AdvancedScoreKeeper scoreKeeper;

        public EnterNewFloorState(StateMachine stateMachine,
                                  Player player,
                                  DungeonController dungeonController,
                                  AdvancedScoreKeeper scoreKeeper) : base(stateMachine)
        {
            this.dungeonController = dungeonController;
            this.player = player;
            this.scoreKeeper = scoreKeeper;
        }

        public override void OnEnter()
        {
            dungeonController.GoToNextFloor();
        }

        public override void Update(float time)
        {
            // Resolve the queue

            // Should this open a new room immediately? Or should it wait for player input to open a new room?
            // For now, let's open a new room immediately.
            StateMachine.SwitchState(new OpenNewRoomState(StateMachine, player, dungeonController, scoreKeeper));
        }

        public override void OnExit() { }
    }
}