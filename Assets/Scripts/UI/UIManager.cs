using System;
using System.Collections.Generic;
using System.Linq;
using Mafu.UnityServiceLocator;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Title")]
    [SerializeField] private List<GameObject> titleObjects = new();

    [Header("During Game")]
    [SerializeField] private List<GameObject> currentRunUI = new();
    [SerializeField] private List<GameObject> cardsUI = new();

    [Header("Game Over")]
    [SerializeField] private List<GameObject> gameOverObjects = new();
    [SerializeField] private GameOverCanvas gameOverCanvas;

    [Header("Other")]
    [SerializeField] private List<GameObject> powerUpDungeonObjects = new();
    [SerializeField] private List<GameObject> shopObjects = new();
    [SerializeField] private List<GameObject> chooseFloorObjects = new();

    void Awake()
    {
        ServiceLocator.Global.Register(this);
    }

    void OnEnable()
    {
        gameManager.OnStartNewGame += ShowDuringGame;
        gameManager.OnGameOver += ShowGameOver;
        gameManager.OnEnterPowerUpDungeonPhase += ShowPowerUpDungeonObjects;
        gameManager.OnEnterShopPhase += ShowShopObjects;
        gameManager.OnEnterChooseFloorPhase += ShowChooseFloorObjects;
        gameManager.OnGoToNextFloor += ShowNewFloorObjects;
    }

    void OnDisable()
    {
        gameManager.OnStartNewGame -= ShowDuringGame;
        gameManager.OnGameOver -= ShowGameOver;
        gameManager.OnEnterPowerUpDungeonPhase -= ShowPowerUpDungeonObjects;
        gameManager.OnEnterShopPhase -= ShowShopObjects;
        gameManager.OnEnterChooseFloorPhase -= ShowChooseFloorObjects;
        gameManager.OnGoToNextFloor -= ShowNewFloorObjects;
    }

    void Start()
    {
        gameOverObjects.ForEach(go => go.SetActive(false));
        currentRunUI.ForEach(go => go.SetActive(false));
        cardsUI.ForEach(go => go.SetActive(false));
        powerUpDungeonObjects.ForEach(go => go.SetActive(false));
        shopObjects.ForEach(go => go.SetActive(false));
        chooseFloorObjects.ForEach(go => go.SetActive(false));

        titleObjects.ForEach(go => go.SetActive(true));
    }

    private void ShowDuringGame()
    {
        titleObjects.ForEach(go => go.SetActive(false));
        gameOverObjects.ForEach(go => go.SetActive(false));

        currentRunUI.ForEach(go => go.SetActive(true));
        cardsUI.ForEach(go => go.SetActive(true));
    }

    private void ShowGameOver()
    {
        currentRunUI.ForEach(go => go.SetActive(false));
        cardsUI.ForEach(go => go.SetActive(false));

        gameOverObjects.ForEach(go => go.SetActive(true));

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