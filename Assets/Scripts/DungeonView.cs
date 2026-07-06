using UnityEngine;

public class DungeonView : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RoomView roomView;
    [SerializeField] private WeaponCardView weaponView;

    private DungeonController dungeonController => gameManager.DungeonController;

    void OnEnable()
    {
        gameManager.OnStartNewGame += OnStartNewGame;
        dungeonController.OnNewRoomOpened += OnOpenNewRoom;
        gameManager.OnGameOver += OnGameOver;
        gameManager.Player.OnWeaponChanged += OnWeaponChanged;
    }

    void OnDisable()
    {
        gameManager.OnStartNewGame -= OnStartNewGame;
        dungeonController.OnNewRoomOpened -= OnOpenNewRoom;
        gameManager.OnGameOver -= OnGameOver;
        gameManager.Player.OnWeaponChanged -= OnWeaponChanged;
    }

    private void OnStartNewGame()
    {
        roomView.OnStartNewGame();
        weaponView.OnStartNewGame();
        OnWeaponChanged();
    }

    private void OnOpenNewRoom()
    {
        roomView.DeregisterRoom();
        roomView.RegisterRoom(gameManager.DungeonController.CurrentRoom);
    }

    private void OnWeaponChanged()
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
