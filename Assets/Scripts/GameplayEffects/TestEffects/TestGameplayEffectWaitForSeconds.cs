using UnityEngine;

namespace Project.Core
{
    public class TestGameplayEffectWaitForSeconds : GameplayEffect
    {
        private readonly float waitTime;
        private float elapsedTime;
        private readonly float readoutTime = 0.5f;
        private float timeSinceLastReadout = 0f;

        public TestGameplayEffectWaitForSeconds(float waitTime)
        {
            this.waitTime = waitTime;
            this.elapsedTime = 0f;
        }

        public override Status OnResolve(float deltaTime)
        {
            elapsedTime += deltaTime;
            timeSinceLastReadout += deltaTime;

            if (timeSinceLastReadout >= readoutTime)
            {
                Debug.Log($"Waiting... {elapsedTime}/{waitTime}");
                timeSinceLastReadout = 0f;
            }

            if (elapsedTime >= waitTime)
            {
                elapsedTime = 0f; // Reset elapsed time for potential reuse
                return Status.Complete;
            }
            return Status.Running;
        }
    }
}