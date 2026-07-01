using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using System;
using System.Linq;

using Mafu.UnityServiceLocator;
using Project.Core.StateMachineSystem;
using Project.Core;
using Project.GameStates;

public class GameManager : MonoBehaviour
{
    [Header("GameSettings")]
    [SerializeField] public GameSettings GameSettings;
    [SerializeField] private bool debugLoop;

    [Header("Services")]
    [SerializeField] public DeckController DeckController;
    [SerializeField] public ShopManager ShopManager;
    [SerializeField] public GlobalBuffRegistry BuffRegistry;
    [SerializeField] public Player Player;

    public DungeonController DungeonController { get; private set; } = new();
    public AdvancedScoreKeeper ScoreKeeper { get; private set; } = null;
    public GameProcessQueue<GameplayEffect> GameplayEffectQueue;

    public Action OnStartNewGame;
    public Action OnGameOver;
    public Action OnOpenNewRoom;
    public Action OnExitCurrentFloor;
    public Action OnPlayerEnterRoom;
    public Action OnPlayerRun;
    public Action OnEnterPowerUpDungeonPhase;
    public Action OnEnterShopPhase;
    public Action OnExitShopPhase;
    public Action OnEnterChooseFloorPhase;
    public Action OnExitChooseFloorPhase;
    public Action OnCardsChanged;

    public int CardsPerRoom => GameSettings != null ? GameSettings.CardsPerRoom : 4;
    private StateMachine stateMachine;

    // public bool CanGoToNextRoom => CurrentRoom != null &&
    //                                CurrentRoom.CanGoToNextRoom();
    public bool CanGoToNextRoom => true; // TEMP: remove this when the CanGoToNextRoom logic is implemented in the RoomController

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    void Awake()
    {
        ServiceLocator.Global.Register(this);
        GameplayEffectQueue = new GameProcessQueue<GameplayEffect>();
        stateMachine = new(this);
    }

    void Start()
    {
        // Temp - remove
        ShopManager.gameObject.SetActive(false);

        ScoreKeeper = new(this);
        StartGame();
    }


    void Update()
    {
        stateMachine.Update();

        // TOOD: This should be run in the update of the states, not the game manager
        DungeonController.Update();
        Player.Update();
    }

    private Status ProcessGameplayEffectQueue(float deltaTime)
    {
        // GameplayEffectQueue.SuspendQueue(loopIsSuspended); // Temp
        if (GameplayEffectQueue.QueueNeedsToBeResolved)
        {
            GameplayEffectQueue.ResolveQueue(deltaTime, debugLoop);
            return Status.Running;
        }
        return Status.Complete;
    }

    #region Game Loop

    public void RestartGame()
    {
        EndGame();
        StartNewRun();
    }

    private void StartGame()
    {
        stateMachine.SwitchState(new TitleScreenState(stateMachine));
    }

    public void StartNewRun()
    {
        stateMachine.SwitchState(new StartNewRunState(stateMachine: stateMachine,
                                                      player: Player,
                                                      dungeonController: DungeonController,
                                                      scoreKeeper: ScoreKeeper,
                                                      deckController: DeckController));
        OnStartNewGame?.Invoke(); // TODO: this should be called from within the StartNewRunState, but for now it is here to avoid breaking the UIManager
    }

    #endregion

    public void TEMP_AddRandomMonsterBuffs(int min, int max)
    {
        string outputString = "Random monster buffs:\n";
        // TEMP: add random monster buffs
        List<string> buffs = new() { "Inspiring", "Elite", "Bloodthirsty", "Exploding", "Hungry", "LoneWolf", "PackTactics", "Pursuer", "Reanimate" };
        int amount = UnityEngine.Random.Range(min, max);
        List<RuntimeCardModel> cardsToBuff = new();
        List<RuntimeCardModel> monsterCards = new();
        foreach (Suit suit in new List<Suit>() { Suit.CLUBS, Suit.SPADES})
        {
            monsterCards.AddRange(DeckController.GetRemainingOfSuit(new() {suit}));
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
        List<RuntimeCardModel> weaponCards = DeckController.GetRemainingOfSuit(new() {Suit.DIAMONDS});
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
        // CurrentRoom.ClearCards();
        Player.FloorReset();
        // FloorCounter.IncrementFloorNumber();
        // RoomCounter.ResetRoomNumber();
        DeckController.ResetDeck();

        OnExitCurrentFloor?.Invoke();

        GoToPowerUpDungeonPhase();
    }

    public int GetScoreToGoToNextFloor()
    {
        // return FloorCounter.FloorNumber * 10000; // Magic number!!
        return 10000; // Magic number!!
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
        // OnGoToNextFloor?.Invoke();
        // OpenFirstRoom();
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

    private void BroadcastOnCardsChanged()
    {
        // This is just temp, this should be refactored
        OnCardsChanged?.Invoke();
    }

    public void OnCardClicked(RuntimeCardModel card, CardClickContext context)
    {
        if (!DungeonController.CurrentRoom.GetCards().Contains(card))
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
                foreach (RuntimeCardModel other in DungeonController.CurrentRoom.GetOthers(card))
                {
                    other?.HandleOnOtherDie(card as MonsterCardModel);
                }


            }

            DungeonController.CurrentRoom.TryRemoveCard(card);

            // Add gold if its the last card in the room
            if (DungeonController.CurrentRoom.IsEmpty)
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
        // OpenNewRoom();
    }

    private bool HandlePotion(RuntimeCardModel card)
    {
        Player.TryDrinkPotion(card);
        return true;
    }

    private bool HandleEnemy(RuntimeCardModel card, CardClickContext context) => context == CardClickContext.TOP ? Player.TryFightWeapon(card) : Player.TryFightUnarmed(card);

    private void HandlePlayerRun()
    {
        // OnPlayerRun?.Invoke();
        // DeckController.Deck.AddToRemaining(CurrentRoom.Cards.ToList(), addToTop: false, shuffle: false);
        // CurrentRoom.ClearCards(keepPersistentThroughRun: true);
        // OpenNewRoom();
    }

    #region DEBUG_FUNCTIONS

    public string DEBUG_GetCurrentGameStateString() => stateMachine.CurrentState.GetType().Name;

    #endregion
}
