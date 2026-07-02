using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class StartNewRunState : State
    {
        Player player;
        DungeonController dungeonController;
        AdvancedScoreKeeper scoreKeeper;
        DeckController deckController;
        GameProcessQueue<GameplayEffect> gameplayEffectQueue;

        public StartNewRunState(StateMachine stateMachine,
                                GameProcessQueue<GameplayEffect> gameplayEffectQueue,
                                Player player,
                                DungeonController dungeonController,
                                AdvancedScoreKeeper scoreKeeper,
                                DeckController deckController
                                ) : base(stateMachine)
        {
            this.player = player;
            this.dungeonController = dungeonController;
            this.scoreKeeper = scoreKeeper;
            this.deckController = deckController;
            this.gameplayEffectQueue = gameplayEffectQueue;
        }

        public override void OnEnter()
        {
            player.SetInteractionState(PlayerInteractionState.None);

            scoreKeeper.Reset();
            dungeonController.RegisterDeckController(deckController);
            dungeonController.StartNewDungeon();

            GameManager.TEMP_AddRandomMonsterBuffs(6, 9);

            // OpenFirstRoom();

            // player.OnRunSuccess += HandlePlayerRun;
            // player.OnDeath += GameOver;
            player.StartNewRun();

            // Go to the next floor
            StateMachine.SwitchState(new EnterNewFloorState(StateMachine, gameplayEffectQueue, player, dungeonController, scoreKeeper));
        }

        public override void Update(float deltaTime) { }
        public override void OnExit() { }
    }
}
