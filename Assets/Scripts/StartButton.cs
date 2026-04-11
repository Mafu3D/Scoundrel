using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] GameManager gameManager;


    public void StartGame()
    {
        gameManager.StartNewGame();
        this.gameObject.SetActive(false);
    }
}
