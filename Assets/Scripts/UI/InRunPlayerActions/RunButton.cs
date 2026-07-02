using UnityEngine;
using UnityEngine.UI;

public class RunButton : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Button button;

    public void Run()
    {
        if (gameManager.Player.InteractionState != PlayerInteractionState.Full)
        {
            Debug.LogWarning("Player cannot use this button (Run) while not in full interaction state.");
            return;
        }

        gameManager.HandlePlayerRun();
    }

    void Update()
    {
        if (gameManager.Player.CanRun)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}