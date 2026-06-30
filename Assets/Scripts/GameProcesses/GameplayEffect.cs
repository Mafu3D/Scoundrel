using System;
using UnityEngine;

namespace Project.Core
{

    public abstract class GameplayEffect : IGameProcess
    {
        public virtual void OnStart() {}
        public virtual Status OnResolve(float deltaTime) { return Status.Complete; }
        public virtual void OnEnd() {}
        public virtual void Reset() {}
        public virtual string StartMessage() { return string.Empty; }
        public virtual string EndMessage() { return string.Empty; }
    }

    public class RuntimeGameplayEffect : GameplayEffect
    {
        private readonly Action onStartHandler;
        private readonly Func<float, Status> onProcessHandler;
        private readonly Action onEndHandler;
        private readonly Action resetHandler;
        private readonly Func<string> startMessageHandler;
        private readonly Func<string> endMessageHandler;

        public RuntimeGameplayEffect(
                              Func<float, Status> onProcessMethod,
                              Action onStartMethod,
                              Action onEndMethod,
                              Action resetMethod,
                              Func<string> startMessageMethod,
                              Func<string> endMessageMethod)
        {
            onProcessHandler = onProcessMethod;
            onStartHandler = onStartMethod;
            onEndHandler = onEndMethod;
            resetHandler = resetMethod;
            startMessageHandler = startMessageMethod;
            endMessageHandler = endMessageMethod;
        }

        // Acquire Target? - Instead of having target passed in, since effects may want to get their own target

        public override void OnStart() => onStartHandler();
        public override Status OnResolve(float deltaTime) => onProcessHandler(deltaTime);
        public override void OnEnd() => onEndHandler();
        public override void Reset() => resetHandler();
        public override string StartMessage() => startMessageHandler();
        public override string EndMessage() => endMessageHandler();
    }

    public class TestGameplayEffectWaitForSeconds : GameplayEffect
    {
        private readonly float waitTime;
        private float elapsedTime;

        public TestGameplayEffectWaitForSeconds(float waitTime)
        {
            this.waitTime = waitTime;
            this.elapsedTime = 0f;
        }

        public override Status OnResolve(float deltaTime)
        {
            elapsedTime += deltaTime;
            Debug.Log($"Waiting... {elapsedTime}/{waitTime}");
            if (elapsedTime >= waitTime)
            {
                elapsedTime = 0f; // Reset elapsed time for potential reuse
                return Status.Complete;
            }
            return Status.Running;
        }
    }
}