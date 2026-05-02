using UnityEngine;

public class GameViewManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RoomView roomView;
    [SerializeField] private WeaponView weaponView;

    void OnEnable()
    {
        gameManager.OnStartNewGame += OnStartNewGame;
        gameManager.OnOpenNewRoom += OnOpenNewRoom;
        gameManager.OnGameOver += OnGameOver;
        gameManager.Player.OnWeaponChanged += OnWeaponChanged;
    }

    void OnDisable()
    {
        gameManager.OnStartNewGame -= OnStartNewGame;
        gameManager.OnOpenNewRoom -= OnOpenNewRoom;
        gameManager.OnGameOver -= OnGameOver;
        gameManager.Player.OnWeaponChanged -= OnWeaponChanged;
    }

    private void OnStartNewGame()
    {
        roomView.OnStartNewGame();
        weaponView.OnStartNewGame();
    }

    private void OnOpenNewRoom()
    {
        roomView.DeregisterRoom();
        roomView.RegisterRoom(gameManager.CurrentRoom);
    }

    private void OnWeaponChanged()
    {
        if (gameManager.Player.Weapon != null)
        {
            weaponView.RegisterWeapon(gameManager.Player.Weapon);
        }
        else
        {
            weaponView.DeregisterWeapon();
        }
    }

    private void OnGameOver()
    {
        roomView.DeregisterRoom();
        weaponView.DeregisterWeapon();
    }
}