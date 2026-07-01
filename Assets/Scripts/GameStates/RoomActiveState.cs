using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class RoomActiveState : State
    {
        public RoomActiveState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter() { }
        public override void Update(float time)
        {
            // Player can run
        }
        public override void OnExit() { }
    }
}