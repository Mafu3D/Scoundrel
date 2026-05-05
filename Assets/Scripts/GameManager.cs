using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using System;
using System.Linq;
using TMPro;
using Mafu.UnityServiceLocator;

public class GameManager : MonoBehaviour
{
    [Header("GameSettings")]
    [SerializeField] public GameSettings GameSettings;

    [Header("Services")]
    [SerializeField] public DeckManager DeckManager;
    [SerializeField] public GlobalBuffRegistry BuffRegistry;
    [SerializeField] public Player Player;

    public bool GameHasStarted {get; private set;}
    public int RoomNumber { get; private set; }= 0;
    public RoomModel CurrentRoom { get; private set; } = null;
    public ScoreKeeper ScoreKeeper { get; private set; } = null;

    public Action OnStartNewGame;
    public Action OnGameOver;
    public Action OnOpenNewRoom;

    public int CardsPerRoom => GameSettings != null ? GameSettings.CardsPerRoom : 4;
    private readonly int remainingToMove = 1;

    public bool CanGoToNextRoom => CurrentRoom != null && CurrentRoom.RemainingCount <= remainingToMove;

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    void Awake()
    {
        ServiceLocator.Global.Register(this);
    }

    void Start()
    {
        ScoreKeeper = new ScoreKeeper(this);
    }

    void Update()
    {
        if (CurrentRoom != null)
        {
            CurrentRoom.Update();
        }
        Player.Update();
    }

    public void RestartGame()
    {
        EndGame();
        StartNewGame();
    }

    public void StartNewGame()
    {
        RoomNumber = 0;
        DeckManager.ResetDeck();
        OnStartNewGame?.Invoke();
        EnterFirstRoom();
        GameHasStarted = true;

        Player.OnRunSuccess += OnPlayerRun;
        Player.OnDeath += GameOver;
        Player.StartNewGame();
    }

    private void EndGame()
    {
        Player.OnRunSuccess -= OnPlayerRun;
        Player.OnDeath -= GameOver;
    }

    private void GameOver()
    {
        EndGame();
        OnGameOver?.Invoke();
    }

    private void EnterFirstRoom()
    {
        List<CardModel> drawnCards = DeckManager.Draw(CardsPerRoom);
        RoomModel room = new(CardsPerRoom, drawnCards);
        CurrentRoom = room;
        CurrentRoom.OnCardsChanged += CheckForGameResolution;
        CurrentRoom.InitializeRoom();

        RoomNumber++;

        OnOpenNewRoom?.Invoke();
    }

    private void OpenNewRoom()
    {
        if (CurrentRoom != null)
        {
            CurrentRoom.OnCardsChanged -= CheckForGameResolution;
        }

        List<CardModel> newCards = new();

        newCards.AddRange(CurrentRoom.RemainingCards());
        List<CardModel> drawnCards = DeckManager.Draw(CardsPerRoom - CurrentRoom.RemainingCount);
        newCards.AddRange(drawnCards);

        RoomModel NextRoom = new(CardsPerRoom, newCards);
        CurrentRoom = NextRoom;
        CurrentRoom.OnCardsChanged += CheckForGameResolution;
        CurrentRoom.InitializeRoom();
        RoomNumber++;

        Player.RoundReset();

        OnOpenNewRoom?.Invoke();
    }

    public void DEBUG_RUN()
    {
        OnPlayerRun();
    }

    private void CheckForGameResolution()
    {
        if (ScoreKeeper.HasPlayerWon())
        {
            EndGame();
        }
    }

    public void OnCardClicked(CardModel card, CardClickContext context)
    {
        if (!CurrentRoom.Cards.Contains(card))
        {
            return;
        }

        // Try to handle the player action based on the card suit
        bool success = card.Suit switch
        {
            Suit.HEARTS => HandlePotion(card),
            Suit.DIAMONDS => Player.TryEquipWeapon(card),
            Suit.SPADES or Suit.CLUBS => HandleEnemy(card, context),
            _ => false
        };

        if (success)
        {
            if (!Player.HasEnteredTheRoom)
            {
                Player.EnterNewRoom();
            }
            CurrentRoom.TryRemoveCard(card);
        }
    }

    public void GoToNextRoom()
    {
        if (!CanGoToNextRoom) return;
        OpenNewRoom();
    }

    private bool HandlePotion(CardModel card)
    {
        Player.TryDrinkPotion(card);
        return true;
    }

    private bool HandleEnemy(CardModel card, CardClickContext context) => context == CardClickContext.TOP ? Player.TryFightWeapon(card) : Player.TryFightUnarmed(card);

    private void OnPlayerRun()
    {
        DeckManager.Deck.AddToRemaining(CurrentRoom.Cards.ToList(), addToTop: false, shuffle: false);
        CurrentRoom.ClearCards();
        OpenNewRoom();
    }
}
