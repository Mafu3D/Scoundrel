using System.Collections;
using Project.Core;
using UnityEngine;

public class ScratchGameProcessTest : MonoBehaviour
{
    [SerializeField] private bool debugLoop;

    public GameProcessQueue<GameplayEffect> GameplayEffectQueue;

    private void Awake()
    {
        GameplayEffectQueue = new GameProcessQueue<GameplayEffect>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameplayEffectQueue.QueueNeedsToBeResolved)
            {
                Debug.Log("resolving queue!");
            }
            else
            {
                // GameplayEffectQueue.Add(gameEvent: new GameplayEffect(
                //     onProcessMethod: (deltaTime) =>
                //     {
                //         Debug.Log($"Processing effect");
                //         StartCoroutine(WaitForSeconds(2f));
                //         return Status.Complete;
                //     },
                //     onStartMethod: () => Debug.Log("Effect started"),
                //     onEndMethod: () => Debug.Log("Effect ended"),
                //     resetMethod: () => Debug.Log("Effect reset"),
                //     startMessageMethod: () => "Effect start message",
                //     endMessageMethod: () => "Effect end message"
                // ));
                GameplayEffectQueue.Add(new TestGameplayEffectWaitForSeconds(2f));
            }
        }
        ProcessGameplayEffectQueue(Time.deltaTime);
    }

    private Status ProcessGameplayEffectQueue(float deltaTime)
    {
        // GameplayEffectQueue.SuspendQueue(loopIsSuspended); // Temp
        if (GameplayEffectQueue.QueueNeedsToBeResolved)
        {
            GameplayEffectQueue.ResolveQueue(deltaTime, debugLoop);
            return Status.Running;
        }
        return Status.Complete;
    }
}