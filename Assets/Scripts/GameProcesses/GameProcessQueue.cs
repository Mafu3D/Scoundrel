using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core
{
    public class GameProcessQueue<TGameProcess> where TGameProcess : IGameProcess
    {
        public List<TGameProcess> Queue = new();
        public Action<TGameProcess> OnNewProcessStarted;
        private int currentIndex;
        private bool currentProcessHasStarted = false;
        public bool QueueNeedsToBeResolved => Queue.Count > 0;
        private readonly bool repeatQueue;
        private bool isSuspended;

        public GameProcessQueue(bool repeatQueue=false)
        {
            this.repeatQueue = repeatQueue;
        }

        public void SuspendQueue(bool value) => isSuspended = value;

        public void Add(TGameProcess gameEvent) => Queue.Add(gameEvent);

        public void Remove(TGameProcess gameEvent) { if (Queue.Contains(gameEvent)) Queue.Remove(gameEvent); }

        private void ClearQueue()
        {
            Queue = new();
            currentIndex = 0;
        }

        public void ResolveQueue(float deltaTime, bool debug = false)
        {
            if (isSuspended) return;

            while (currentIndex < Queue.Count)
            {
                if (!currentProcessHasStarted)
                {
                    Queue[currentIndex].OnStart();
                    if (debug) LogMessage(Queue[currentIndex].StartMessage());
                    currentProcessHasStarted = true;
                    OnNewProcessStarted?.Invoke(Queue[currentIndex]);
                }
                Status status = Queue[currentIndex].OnResolve(deltaTime);
                if (status != Status.Complete)
                {
                    return;
                }

                Queue[currentIndex].OnEnd();
                if (debug) LogMessage(Queue[currentIndex].EndMessage());

                Queue[currentIndex].Reset();

                currentIndex++;
                currentProcessHasStarted = false;
            }

            if (repeatQueue)
            {
                currentIndex = 0;
            }
            else
            {
                ClearQueue();
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