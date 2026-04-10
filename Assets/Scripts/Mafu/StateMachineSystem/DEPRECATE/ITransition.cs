namespace Mafu.StateMachineSystemDeprecate
{
    public interface ITransition {
        IState To { get; }
        IPredicate Condition { get; }
    }
}