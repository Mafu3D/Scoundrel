using UnityEngine;
using UnityEngine.UI;

public class RunButton : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Button button;

    public void Run()
    {
        if (gameManager.CanRun())
        {
            gameManager.Run();
        }
    }

    void Update()
    {
        if (gameManager.CanRun())
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}