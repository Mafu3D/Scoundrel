using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public void NewGame()
    {
        gameManager.StartNewGame();
    }
}
