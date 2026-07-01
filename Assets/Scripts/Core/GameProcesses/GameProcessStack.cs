using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Core
{
    public class GameProcessStack<TGameProcess> where TGameProcess : IGameProcess, IGameProcessSuspendable
    {
        private List<TGameProcess> stack = new();
        public bool StackEmpty => stack.Count == 0;
        public TGameProcess CurrentProcess
        {
            get
            {
                if (stack.Count == 0)
                {
                    return default;
                }
                return stack.Last();
            }
        }

        private int currentIndex;
        private bool currentProcessHasStarted = false;
        private bool currentProcessIsEnding = false; // This was added to prevent a bug.. this feels hacky and may cause problems later

        public void Add(TGameProcess gameEvent) {
            if (!StackEmpty)
            {
                stack[currentIndex].Suspend(true);
            }
            stack.Add(gameEvent);

            if (!currentProcessIsEnding)
            {
                currentIndex = stack.Count - 1;
            }

            currentProcessHasStarted = false;
        }

        public void Remove(TGameProcess gameEvent, bool debug = false) {
            if (stack.Contains(gameEvent))
            {
                gameEvent.OnEnd();
                if (debug) LogMessage(stack[currentIndex].EndMessage());
                gameEvent.Reset();
                stack.Remove(gameEvent);
                currentIndex = stack.Count - 1;
            }
        }

        private void ForceClearStack(bool debug = false)
        {
            for (int i = stack.Count - 1; i>=0; i--)
            {
                stack[i].OnEnd();
                if (debug) LogMessage(stack[currentIndex].EndMessage());
                stack[currentIndex].Reset();
                stack.RemoveAt(currentIndex);
            }
            currentIndex = 0;
        }

        public void DebugStack()
        {
            string debugString = "";
            debugString += $"StackEmpty: {StackEmpty}\n";
            if (!StackEmpty)
            {
                debugString += "--STACK--\n";
                debugString += $"CurrentIndex: {currentIndex}\n";
                for (int i = 0; i < stack.Count; i++)
                {
                    debugString += $"{i}: {stack[i]}\n";
                }
            }
            Debug.Log(debugString);
        }

        public void ResolveStack(float deltaTime, bool debug = false)
        {
            while (stack.Count > 0)
            {
                if (!currentProcessHasStarted)
                {
                    stack[currentIndex].OnStart();
                    if (debug) LogMessage(stack[currentIndex].StartMessage());
                    currentProcessHasStarted = true;
                }
                Status status = stack[currentIndex].OnResolve(deltaTime);
                if (status != Status.Complete)
                {
                    return;
                }

                currentProcessIsEnding = true;
                stack[currentIndex].OnEnd();
                // DebugStack();
                if (debug) LogMessage(stack[currentIndex].EndMessage());

                currentProcessIsEnding = false;

                stack[currentIndex].Reset();

                stack.RemoveAt(currentIndex);
                currentIndex = Math.Clamp(currentIndex - 1, 0, 9999999);
                if (StackEmpty)
                {
                    currentProcessHasStarted = false;
                    return;
                }
                stack[currentIndex].Suspend(false);
            }
        }

        private void LogMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Debug.Log(message);
            }
        }
    }
}