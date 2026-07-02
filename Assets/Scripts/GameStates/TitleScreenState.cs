using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class TitleScreenState : State
    {
        public TitleScreenState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            // Show the title screen UI
        }

        public override void Update(float deltaTime) { }
        public override void OnExit() { }
    }
}