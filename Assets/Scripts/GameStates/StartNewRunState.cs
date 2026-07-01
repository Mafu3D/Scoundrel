using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class StartNewRunState : State
    {
        Player player;
        DungeonController dungeonController;
        AdvancedScoreKeeper scoreKeeper;
        DeckController deckController;

        public StartNewRunState(StateMachine stateMachine,
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
        }

        public override void OnEnter()
        {
            scoreKeeper.Reset();
            dungeonController.RegisterDeckController(deckController);
            dungeonController.StartNewDungeon();

            GameManager.TEMP_AddRandomMonsterBuffs(6, 9);

            // OpenFirstRoom();

            // player.OnRunSuccess += HandlePlayerRun;
            // player.OnDeath += GameOver;
            player.StartNewRun();

            // Go to the next floor
            StateMachine.SwitchState(new EnterNewFloorState(StateMachine, player, dungeonController, scoreKeeper));
        }

        public override void Update(float time) { }
        public override void OnExit() { }
    }
}
