using System.Collections.Generic;

namespace Project.Core
{
    public interface IGameProcess
    {
        public void OnStart();
        public Status OnResolve(float deltaTime);
        public void OnEnd();
        public void Reset();
        public string StartMessage();
        public string EndMessage();
    }

    public interface IGameProcessSuspendable
    {
        public void OnSuspend();
        public void OnUnsuspend();
        public void Suspend(bool value);
    }

    // Unused but keeping for now
    // public interface IGameProcessHierarchical
    // {
    //     public List<IGameProcess> Children { get; }
    //     public virtual IGameProcess GetNextChild() => default;
    //     public virtual void AddChild(IGameProcess child) { }
    // }
}