using UnityEngine;

public class GameViewManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RoomView roomView;

    void OnEnable()
    {
        gameManager.OnStartNewGame += OnStartNewGame;
        gameManager.OnEnterNewRoom += OnEnterNewRoom;
        gameManager.OnGameOver += OnGameOver;
    }

    void OnDisable()
    {
        gameManager.OnStartNewGame -= OnStartNewGame;
        gameManager.OnEnterNewRoom -= OnEnterNewRoom;
        gameManager.OnGameOver -= OnGameOver;
    }

    private void OnStartNewGame()
    {
        roomView.OnStartNewGame();
    }

    private void OnEnterNewRoom()
    {
        roomView.DeregisterRoom();
        roomView.RegisterRoom(gameManager.CurrentRoom);
    }

    private void OnGameOver()
    {
        roomView.DeregisterRoom();
    }
}