using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using System;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("GameSettings")]
    [SerializeField] public GameSettings GameSettings;

    [Header("Services")]
    [SerializeField] public DeckManager DeckManager;
    [SerializeField] public Player Player;

    public bool GameHasStarted {get; private set;}
    public int RoomNumber { get; private set; }= 0;
    public RoomModel CurrentRoom { get; private set; } = null;
    public ScoreKeeper ScoreKeeper { get; private set; } = null;

    public Action OnStartNewGame;
    public Action OnGameOver;
    public Action OnEnterNewRoom;

    private int cardsPerRoom => GameSettings != null ? GameSettings.CardsPerRoom : 4;
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
        // ServiceLocator.Global.Register(this);
    }

    void Start()
    {
        ScoreKeeper = new ScoreKeeper(this);
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
        List<CardModel> drawnCards = DeckManager.Draw(cardsPerRoom);
        RoomModel room = new(cardsPerRoom, drawnCards);
        CurrentRoom = room;

        RoomNumber++;

        OnEnterNewRoom?.Invoke();
    }

    private void EnterNewRoom()
    {
        if (!CanGoToNextRoom) return;

        List<CardModel> newCards = new();

        newCards.AddRange(CurrentRoom.RemainingCards());
        List<CardModel> drawnCards = DeckManager.Draw(cardsPerRoom - CurrentRoom.RemainingCount);
        newCards.AddRange(drawnCards);

        RoomModel NextRoom = new(cardsPerRoom, newCards);
        CurrentRoom = NextRoom;
        RoomNumber++;

        Player.RoundReset();

        OnEnterNewRoom?.Invoke();
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
        EnterNewRoom();
    }
}
