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
    [SerializeField] private DungeonView dungeonView;
    [SerializeField] private GameObject playerActionsButtons;
    [SerializeField] private List<GameObject> worldSpaceUI = new();

    [Header("Game Over")]
    [SerializeField] private GameOverCanvas gameOverCanvas;

    [Header("Other")]
    [SerializeField] private List<GameObject> powerUpDungeonObjects = new();
    [SerializeField] private List<GameObject> shopObjects = new();
    [SerializeField] private List<GameObject> chooseFloorObjects = new();

    void OnEnable()
    {
        gameManager.OnEnterNewFloor += ShowDuringGame;
        gameManager.OnGameOver += ShowGameOver;
        gameManager.OnExitCurrentFloor += ShowPowerUpDungeonObjects;
        gameManager.OnEnterShop += ShowShopObjects;
        gameManager.OnEnterChooseFloorPhase += ShowChooseFloorObjects;
        gameManager.DungeonController.OnGoToNextFloor += ShowNewFloorObjects;
    }

    void OnDisable()
    {
        gameManager.OnEnterNewFloor -= ShowDuringGame;
        gameManager.OnGameOver -= ShowGameOver;
        gameManager.OnExitCurrentFloor -= ShowPowerUpDungeonObjects;
        gameManager.OnEnterShop -= ShowShopObjects;
        gameManager.OnEnterChooseFloorPhase -= ShowChooseFloorObjects;
        gameManager.DungeonController.OnGoToNextFloor -= ShowNewFloorObjects;
    }

    void Start()
    {
        gameOverCanvas.gameObject.SetActive(false);
        currentRunCanvas.SetActive(false);

        dungeonView.Hide();
        playerActionsButtons.SetActive(false);

        worldSpaceUI.ForEach(go => go.SetActive(false));
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

        Debug.Log("showing");
        dungeonView.Show();
        playerActionsButtons.SetActive(true);

        worldSpaceUI.ForEach(go => go.SetActive(true));
    }

    private void ShowGameOver()
    {
        currentRunCanvas.SetActive(false);

        dungeonView.Hide();
        playerActionsButtons.SetActive(false);

        worldSpaceUI.ForEach(go => go.SetActive(false));

        gameOverCanvas.gameObject.SetActive(true);

        bool playerWon = gameManager.ScoreKeeper.HasPlayerWon();
        int finalScore = gameManager.ScoreKeeper.GetScore();
        gameOverCanvas.UpdateText(playerWon, finalScore);
    }

    private void ShowPowerUpDungeonObjects()
    {
        dungeonView.Hide();
        playerActionsButtons.SetActive(false);


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

        dungeonView.Show();
        playerActionsButtons.SetActive(true);

    }
}