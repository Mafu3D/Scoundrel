using UnityEngine;
using UnityEngine.UI;

public class RunButton : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Button button;

    public void Run() => gameManager.Player.TryRun();

    void Update()
    {
        if (gameManager.Player.CanRun())
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}