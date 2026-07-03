using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class GameOverState : State
    {
        Player player;
        DungeonController dungeonController;
        AdvancedScoreKeeper scoreKeeper;
        DeckController deckController;
        GameProcessQueue<GameplayEffect> gameplayEffectQueue;

        public GameOverState(StateMachine stateMachine,
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
            player.SetInteractionState(PlayerInteractionState.UIOnly);
        }

        public override void Update(float deltaTime) { }
        public override void OnExit() { }
    }
}