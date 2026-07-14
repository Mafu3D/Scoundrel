using System;
using UnityEngine;

public class DungeonView : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RoomView roomView;
    [SerializeField] private WeaponCardView weaponView;
    [SerializeField] private NextCardsView nextCardsView;

    private DungeonController dungeonController => gameManager.DungeonController;

    void OnEnable()
    {
        gameManager.OnStartNewGame += OnStartNewGame;
        dungeonController.OnNewRoomOpened += OnOpenNewRoom;
        gameManager.OnGameOver += OnGameOver;
        gameManager.Player.OnWeaponChanged += UpdateWeaponView;
    }

    void OnDisable()
    {
        gameManager.OnStartNewGame -= OnStartNewGame;
        dungeonController.OnNewRoomOpened -= OnOpenNewRoom;
        gameManager.OnGameOver -= OnGameOver;
        gameManager.Player.OnWeaponChanged -= UpdateWeaponView;
    }

    public void Show()
    {
        roomView.gameObject.SetActive(true);
        weaponView.gameObject.SetActive(true);
        UpdateWeaponView();
    }

    public void Hide()
    {
        roomView.gameObject.SetActive(false);
        weaponView.gameObject.SetActive(false);
    }

    private void OnStartNewGame()
    {
        roomView.OnStartNewGame();
        weaponView.OnStartNewGame();
        nextCardsView.OnStartNewGame();
        UpdateWeaponView();
    }

    private void OnOpenNewRoom()
    {
        roomView.DeregisterRoom();
        roomView.RegisterRoom(gameManager.DungeonController.CurrentRoom);

        if (gameManager.ShowNextCards)
        {
            nextCardsView.RegisterNextCards(gameManager.DungeonController.GetNextCards(gameManager.AmountOfNextCardsToShow));
        }
        else
        {
            nextCardsView.DeregisterNextCards();
        }
    }

    private void UpdateWeaponView()
    {
        weaponView.DeregisterWeapon();
        if (gameManager.Player.Weapon != null)
        {
            weaponView.RegisterWeapon(gameManager.Player.Weapon);
        }
    }

    private void OnGameOver()
    {
        roomView.DeregisterRoom();
        weaponView.DeregisterWeapon();
    }
}
