using UnityEngine;
using UnityEngine.UI;

public class RunButton : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Button button;

    public void Run()
    {
        gameManager.Run();
    }

    void Update()
    {
        if (gameManager.HasRunToken)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}