using System;
using System.Collections.Generic;
using System.Linq;
using Mafu.UnityServiceLocator;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Title")]
    [SerializeField] private TitleScreenCanvas titleScreenCanvas;

    [Header("During Game")]
    [SerializeField] private GameObject currentRunCanvas;
    [SerializeField] private List<GameObject> cardsUI = new();

    [Header("Game Over")]
    [SerializeField] private GameOverCanvas gameOverCanvas;

    [Header("Other")]
    [SerializeField] private List<GameObject> powerUpDungeonObjects = new();
    [SerializeField] private List<GameObject> shopObjects = new();
    [SerializeField] private List<GameObject> chooseFloorObjects = new();

    void OnEnable()
    {
        gameManager.OnStartNewGame += ShowDuringGame;
        gameManager.OnGameOver += ShowGameOver;
        gameManager.OnEnterPowerUpDungeonPhase += ShowPowerUpDungeonObjects;
        gameManager.OnEnterShopPhase += ShowShopObjects;
        gameManager.OnEnterChooseFloorPhase += ShowChooseFloorObjects;
        gameManager.DungeonController.OnGoToNextFloor += ShowNewFloorObjects;
    }

    void OnDisable()
    {
        gameManager.OnStartNewGame -= ShowDuringGame;
        gameManager.OnGameOver -= ShowGameOver;
        gameManager.OnEnterPowerUpDungeonPhase -= ShowPowerUpDungeonObjects;
        gameManager.OnEnterShopPhase -= ShowShopObjects;
        gameManager.OnEnterChooseFloorPhase -= ShowChooseFloorObjects;
        gameManager.DungeonController.OnGoToNextFloor -= ShowNewFloorObjects;
    }

    void Start()
    {
        gameOverCanvas.gameObject.SetActive(false);
        currentRunCanvas.SetActive(false);
        cardsUI.ForEach(go => go.SetActive(false));
        powerUpDungeonObjects.ForEach(go => go.SetActive(false));
        shopObjects.ForEach(go => go.SetActive(false));
        chooseFloorObjects.ForEach(go => go.SetActive(false));

        titleScreenCanvas.gameObject.SetActive(true);
    }

    private void ShowDuringGame()
    {
        titleScreenCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);

        currentRunCanvas.SetActive(true);
        cardsUI.ForEach(go => go.SetActive(true));
    }

    private void ShowGameOver()
    {
        currentRunCanvas.SetActive(false);
        cardsUI.ForEach(go => go.SetActive(false));

        gameOverCanvas.gameObject.SetActive(true);

        bool playerWon = gameManager.ScoreKeeper.HasPlayerWon();
        int finalScore = gameManager.ScoreKeeper.GetScore();
        gameOverCanvas.UpdateText(playerWon, finalScore);
    }

    private void ShowPowerUpDungeonObjects()
    {
        // currentRunUI.ForEach(go => go.SetActive(false));
        cardsUI.ForEach(go => go.SetActive(false));

        powerUpDungeonObjects.ForEach(go => go.SetActive(true));
    }

    private void ShowShopObjects()
    {
        powerUpDungeonObjects.ForEach(go => go.SetActive(false));
        shopObjects.ForEach(go => go.SetActive(true));
    }

    private void ShowChooseFloorObjects()
    {
        shopObjects.ForEach(go => go.SetActive(false));
        chooseFloorObjects.ForEach(go => go.SetActive(true));
    }

    private void ShowNewFloorObjects()
    {
        chooseFloorObjects.ForEach(go => go.SetActive(false));
        cardsUI.ForEach(go => go.SetActive(true));
    }
}