using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using System;
using System.Linq;
using TMPro;
using Mafu.UnityServiceLocator;
using Mafu.StateMachineSystem;

public class GameManager : MonoBehaviour
{
    [Header("GameSettings")]
    [SerializeField] public GameSettings GameSettings;

    [Header("Services")]
    [SerializeField] public DeckManager DeckManager;
    [SerializeField] public ShopManager ShopManager;
    [SerializeField] public GlobalBuffRegistry BuffRegistry;
    [SerializeField] public Player Player;

    public bool GameHasStarted {get; private set;}
    public int RoomNumber { get; private set; } = 0;
    public int FloorNumber { get; private set; } = 0;
    public RoomModel CurrentRoom { get; private set; } = null;
    public AdvancedScoreKeeper ScoreKeeper { get; private set; } = null;

    public Action OnStartNewGame;
    public Action OnGameOver;
    public Action OnOpenNewRoom;
    public Action OnExitCurrentFloor;
    public Action OnGoToNextFloor;
    public Action OnPlayerEnterRoom;
    public Action OnPlayerRun;
    public Action OnEnterPowerUpDungeonPhase;
    public Action OnEnterShopPhase;
    public Action OnExitShopPhase;
    public Action OnEnterChooseFloorPhase;
    public Action OnExitChooseFloorPhase;

    public int CardsPerRoom => GameSettings != null ? GameSettings.CardsPerRoom : 4;
    private StateMachine stateMachine;

    public bool CanGoToNextRoom => CurrentRoom != null &&
                                   CurrentRoom.CanGoToNextRoom();

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
        ShopManager.gameObject.SetActive(false);

        ScoreKeeper = new(this);
        stateMachine = new();
        stateMachine.SwitchState(new TitleScreenState(stateMachine));
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
        stateMachine.SwitchState(new TitleScreenState(stateMachine));

        RoomNumber = 0;
        ScoreKeeper.Reset();
        DeckManager.TEMP_CreateNewDeck();
        TEMP_AddRandomMonsterBuffs(6, 9);
        FloorNumber = 1;
        OnGoToNextFloor?.Invoke();

        OnStartNewGame?.Invoke();
        OpenFirstRoom();
        GameHasStarted = true;

        Player.OnRunSuccess += HandlePlayerRun;
        Player.OnDeath += GameOver;
        Player.StartNewGame();
    }

    private void TEMP_AddRandomMonsterBuffs(int min, int max)
    {
        string outputString = "Random monster buffs:\n";
        // TEMP: add random monster buffs
        List<string> buffs = new() { "Inspiring", "Elite", "Bloodthirsty", "Exploding" };
        int amount = UnityEngine.Random.Range(min, max);
        List<RuntimeCardModel> cardsToBuff = new();
        List<RuntimeCardModel> monsterCards = new();
        foreach (Suit suit in new List<Suit>() { Suit.CLUBS, Suit.SPADES})
        {
            monsterCards.AddRange(DeckManager.GetRemainingOfSuit(new() {suit}));
        }
        for (int i = 0; i < amount; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, monsterCards.Count);
            cardsToBuff.Add(monsterCards[randIndex]);
        }
        foreach (RuntimeCardModel card in cardsToBuff)
        {
            int randBuff = UnityEngine.Random.Range(0, buffs.Count);
            Buff buff = BuffRegistry.GetBuffFromName(buffs[randBuff]);
            card.AddNewBuff(buff);
            outputString += $"{card} >>> {buff}\n";
        }
        Debug.Log(outputString);
    }

    private void TEMP_AddRandomWeaponBuffs(int min, int max)
    {
        string outputString = "Random weapon buffs:\n";
        // TEMP: add random weapon buffs
        List<string> buffs = new() { "Bomb", "Cleaving", "Honed", "Reinforced" };
        int amount = UnityEngine.Random.Range(min, max);
        List<RuntimeCardModel> cardsToBuff = new();
        List<RuntimeCardModel> weaponCards = DeckManager.GetRemainingOfSuit(new() {Suit.DIAMONDS});
        for (int i = 0; i < amount; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, weaponCards.Count);
            cardsToBuff.Add(weaponCards[randIndex]);
        }
        foreach (RuntimeCardModel card in cardsToBuff)
        {
            int randBuff = UnityEngine.Random.Range(0, buffs.Count);
            Buff buff = BuffRegistry.GetBuffFromName(buffs[randBuff]);
            card.AddNewBuff(buff);outputString += $"{card} >>> {buff}\n";
        }
        Debug.Log(outputString);
    }

    private void EndGame()
    {
        Player.OnRunSuccess -= HandlePlayerRun;
        Player.OnDeath -= GameOver;
    }

    private void GameOver()
    {
        EndGame();
        OnGameOver?.Invoke();
    }

    public void DEBUG_GOTONEXTFLOOR()
    {
        ExitCurrentFloor();
    }

    private void ExitCurrentFloor()
    {
        CurrentRoom.ClearCards();
        Player.FloorReset();
        FloorNumber += 1;
        RoomNumber = 0;
        DeckManager.ResetDeck();

        OnExitCurrentFloor?.Invoke();

        GoToPowerUpDungeonPhase();
    }

    public int GetScoreToGoToNextFloor()
    {
        return FloorNumber * 10000; // Magic number!!
    }

    private void GoToPowerUpDungeonPhase()
    {
        OnEnterPowerUpDungeonPhase?.Invoke();
        TEMP_AddRandomMonsterBuffs(4, 6);
        // StartCoroutine(PowerUpDungeonPhaseRoutine());
    }

    // private IEnumerator PowerUpDungeonPhaseRoutine()
    // {
    //     yield return new WaitForSecondsRealtime(4);
    //     GoToShopPhase();
    // }

    public void GoToShopPhase()
    {
        ShopManager.StartNewShopPhase();
        ShopManager.gameObject.SetActive(true);
        OnEnterShopPhase?.Invoke();
    }

    public void ExitShopPhase()
    {
        ShopManager.gameObject.SetActive(false);
        ShopManager.ExitShopPhase();
        OnExitShopPhase?.Invoke();
        GoToChooseFloorPhase();
    }

    private void GoToChooseFloorPhase()
    {
        OnEnterChooseFloorPhase?.Invoke();
    }

    public void ExitGoToFloorPhase()
    {
        OnExitChooseFloorPhase?.Invoke();
        GoToNextFloor();
    }

    private void GoToNextFloor()
    {
        OnGoToNextFloor?.Invoke();
        OpenFirstRoom();
    }

    private void OpenFirstRoom()
    {
        List<RuntimeCardModel> drawnCards = DeckManager.Draw(CardsPerRoom);
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

        // Shuffle in any doors
        foreach(RuntimeCardModel card in CurrentRoom.RemainingCards())
        {
            if (card is DoorCardModel)
            {
                CurrentRoom.TryRemoveCard(card);
                DeckManager.Deck.ShuffleIn(card);
            }
        }

        List<RuntimeCardModel> newCards = new();

        newCards.AddRange(CurrentRoom.RemainingCards());
        List<RuntimeCardModel> drawnCards = DeckManager.Draw(CardsPerRoom - CurrentRoom.RemainingCount);
        newCards.AddRange(drawnCards);

        RoomModel NextRoom = new(CardsPerRoom, newCards);
        CurrentRoom = NextRoom;
        CurrentRoom.OnCardsChanged += CheckForGameResolution;
        CurrentRoom.InitializeRoom();
        RoomNumber++;

        Player.RoundReset();
        ScoreKeeper.ResetRoomMultiplier();

        OnOpenNewRoom?.Invoke();
    }

    public void DEBUG_RUN()
    {
        HandlePlayerRun();
    }

    private void CheckForGameResolution()
    {
        if (ScoreKeeper.HasPlayerWon())
        {
            EndGame();
        }
    }

    public void OnCardClicked(RuntimeCardModel card, CardClickContext context)
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
            Suit.DOORS => HandleDoor(),
            Suit.TREASURES => HandleTreasure(),
            _ => false
        };

        if (success)
        {
            if (!Player.HasEnteredTheRoom)
            {
                // Player enter room
                Player.EnterNewRoom();
                OnPlayerEnterRoom?.Invoke();
            }

            // Add gold for defeating a monster
            if (card is MonsterCardModel)
            {
                Player.AddGold(1);

                // also broadcast to other cards that this died
                foreach (RuntimeCardModel other in CurrentRoom.GetOthers(card))
                {
                    other?.HandleOnOtherDie(card as MonsterCardModel);
                }


            }

            CurrentRoom.TryRemoveCard(card);

            // Add gold if its the last card in the room
            if (CurrentRoom.IsEmpty)
            {
                Player.AddGold(2);
            }

            if (card.Suit == Suit.CLUBS || card.Suit == Suit.SPADES)
            {
                ScoreKeeper.AddToScore(card);
                // TEMP: Inc score keeper multiplier here
                ScoreKeeper.IncRoomMultiplier();
            }
        }
    }

    private bool HandleDoor()
    {
        if (ScoreKeeper.GetScore() >= GetScoreToGoToNextFloor())
        {
            ExitCurrentFloor();
            return true;
        }
        return false;
    }

    private bool HandleTreasure()
    {
        Player.AddGold(3);
        return true;
    }

    public void GoToNextRoom()
    {
        if (!CanGoToNextRoom) return;
        OpenNewRoom();
    }

    private bool HandlePotion(RuntimeCardModel card)
    {
        Player.TryDrinkPotion(card);
        return true;
    }

    private bool HandleEnemy(RuntimeCardModel card, CardClickContext context) => context == CardClickContext.TOP ? Player.TryFightWeapon(card) : Player.TryFightUnarmed(card);

    private void HandlePlayerRun()
    {
        OnPlayerRun?.Invoke();
        DeckManager.Deck.AddToRemaining(CurrentRoom.Cards.ToList(), addToTop: false, shuffle: false);
        CurrentRoom.ClearCards(keepPersistentThroughRun: true);
        OpenNewRoom();
    }
}
