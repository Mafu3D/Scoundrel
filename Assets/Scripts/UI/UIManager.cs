using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Title")]
    [SerializeField] private List<GameObject> titleObjects = new();

    [Header("During Game")]
    [SerializeField] private List<GameObject> duringGameObjects = new();

    [Header("Game Over")]
    [SerializeField] private List<GameObject> gameOverObjects = new();
    [SerializeField] private GameOverCanvas gameOverCanvas;

    void OnEnable()
    {
        gameManager.OnStartNewGame += ShowDuringGame;
        gameManager.OnGameOver += ShowGameOver;
    }

    void OnDisable()
    {
        gameManager.OnStartNewGame -= ShowDuringGame;
        gameManager.OnGameOver -= ShowGameOver;
    }

    void Start()
    {
        foreach(GameObject go in duringGameObjects.Union(gameOverObjects))
        {
            go.SetActive(false);
        }
        foreach (GameObject go in titleObjects)
        {
            go.SetActive(true);
        }
    }

    private void ShowDuringGame()
    {
        foreach(GameObject go in titleObjects.Union(gameOverObjects))
        {
            go.SetActive(false);
        }
        foreach (GameObject go in duringGameObjects)
        {
            go.SetActive(true);
        }
    }
    private void ShowGameOver()
    {
        foreach(GameObject go in duringGameObjects.Union(titleObjects))
        {
            go.SetActive(false);
        }
        foreach (GameObject go in gameOverObjects)
        {
            go.SetActive(true);
        }

        bool playerWon = gameManager.ScoreKeeper.HasPlayerWon();
        int finalScore = gameManager.ScoreKeeper.GetScore();
        gameOverCanvas.UpdateText(playerWon, finalScore);
    }
}