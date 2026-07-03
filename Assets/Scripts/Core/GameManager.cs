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
    public GameProcessQueue<GameplayEffect> GameplayEffectQueue { get; private set; }

    public Action OnStartNewGame;
    public Action OnGameOver;
    public Action OnOpenNewRoom;
    public Action OnExitCurrentFloor;
    public Action OnEnterPowerUpDungeonPhase;
    public Action OnEnterShopPhase;
    public Action OnExitShopPhase;
    public Action OnEnterChooseFloorPhase;
    public Action OnExitChooseFloorPhase;
    public Action OnCardsChanged;

    private StateMachine stateMachine;
    private CombatController combatController;

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
        combatController = new(Player, stateMachine, GameplayEffectQueue, DungeonController, ScoreKeeper);
        StartGame();
    }


    void Update()
    {
        stateMachine.Update();
    }

    #region Game Loop
    // TODO: Eventually I think I should move all of the game loop stuff into its own class
    //          Maybe like a state machine controller class?

    public void RestartGame()
    {
        EndGame();
        StartNewRun();
    }

    private void StartGame()
    {
        Player.OnDeath += GameOver;

        stateMachine.SwitchState(new TitleScreenState(stateMachine));
    }

    public void StartNewRun()
    {
        stateMachine.SwitchState(new StartNewRunState(stateMachine: stateMachine,
                                                      gameplayEffectQueue: GameplayEffectQueue,
                                                      player: Player,
                                                      dungeonController: DungeonController,
                                                      scoreKeeper: ScoreKeeper,
                                                      deckController: DeckController));
        OnStartNewGame?.Invoke(); // TODO: this should be called from within the StartNewRunState, but for now it is here to avoid breaking the UIManager
    }

    public void GoToNextRoom()
    {
        if (!DungeonController.CanGoToNextRoom)
        {
            Debug.LogWarning("Tried to go to the next room but can't!");
            return;
        }

        stateMachine.SwitchState(new OpenNewRoomState(stateMachine,
                                                      GameplayEffectQueue,
                                                      Player,
                                                      DungeonController,
                                                      ScoreKeeper));
    }

    public void HandlePlayerRun(bool force = false)
    {
        if (!Player.TrySpendRun())
        {
            Debug.LogWarning("Tried to run but can't!");
            if (!force)
            {
                return;
            }
        }

        stateMachine.SwitchState(new RunFromRoomState(stateMachine,
                                                      GameplayEffectQueue,
                                                      Player,
                                                      DungeonController,
                                                      ScoreKeeper));
    }


    #endregion

    public void TEMP_AddRandomMonsterBuffs(int min, int max)
    {
        return;

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

    /// <summary>
    /// Runs any time the game needs to end
    /// </summary>
    private void EndGame()
    {
        Player.OnDeath -= GameOver;
    }

    /// <summary>
    /// Only runs if the player loses and goes to the game over (loss) screen
    /// </summary>
    private void GameOver()
    {
        EndGame();
        OnGameOver?.Invoke();
        stateMachine.SwitchState(new GameOverState(stateMachine,
                                                    GameplayEffectQueue,
                                                    Player,
                                                    DungeonController,
                                                    ScoreKeeper,
                                                    DeckController));
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

    #region Card Handling - Move!!

    public void OnCardClicked(RuntimeCardModel card, CardClickContext context)
    {
        if (Player.InteractionState != PlayerInteractionState.Full)
        {
            return;
        }
        if (!DungeonController.CurrentRoom.GetCards().Contains(card))
        {
            return;
        }

        // Try to handle the player action based on the card suit
        bool success = card.Suit switch
        {
            Suit.HEARTS => HandlePotion(card),
            Suit.DIAMONDS => HandleWeapon(card),
            Suit.SPADES or Suit.CLUBS => HandleEnemy(card, context),
            Suit.DOORS => HandleDoor(card),
            Suit.TREASURES => HandleTreasure(card),
            _ => false
        };
    }

    private bool HandleDoor(RuntimeCardModel card)
    {
        if (ScoreKeeper.GetScore() >= GetScoreToGoToNextFloor())
        {
            stateMachine.SwitchState(new ResolveCardState(card, stateMachine, GameplayEffectQueue, Player, DungeonController, ScoreKeeper));
            ExitCurrentFloor();
            return true;
        }
        return false;
    }

    private bool HandleTreasure(RuntimeCardModel card)
    {
        Player.AddGold(3);
        stateMachine.SwitchState(new ResolveCardState(card, stateMachine, GameplayEffectQueue, Player, DungeonController, ScoreKeeper));
        return true;
    }


    private bool HandlePotion(RuntimeCardModel card)
    {
        Player.TryDrinkPotion(card);
        stateMachine.SwitchState(new ResolveCardState(card, stateMachine, GameplayEffectQueue, Player, DungeonController, ScoreKeeper));
        return true;
    }

    private bool HandleEnemy(RuntimeCardModel card, CardClickContext context)
    {
        return context == CardClickContext.TOP ? combatController.TryFightWeapon(card) : combatController.TryFightUnarmed(card);
    }

    private bool HandleWeapon(RuntimeCardModel card)
    {
        bool success = Player.TryEquipWeapon(card);
        if (success)
        {
            stateMachine.SwitchState(new ResolveCardState(card, stateMachine, GameplayEffectQueue, Player, DungeonController, ScoreKeeper));
        }
        return success;
    }


    #endregion

    #region DEBUG_FUNCTIONS

    public string DEBUG_GetCurrentGameStateString() => stateMachine.CurrentState.GetType().Name;

    #endregion
}
