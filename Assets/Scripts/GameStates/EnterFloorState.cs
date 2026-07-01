using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class EnterFloorState : State
    {
        public EnterFloorState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter() { }
        public override void Update(float time) { }
        public override void OnExit() { }
    }
}