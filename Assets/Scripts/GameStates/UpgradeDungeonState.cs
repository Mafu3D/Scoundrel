using Mafu.UnityServiceLocator;
using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class UpgradeDungeonState : State
    {
        private readonly Player player;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
        private readonly DungeonController dungeonController;

        public UpgradeDungeonState(StateMachine stateMachine,
                                    GameProcessQueue<GameplayEffect> gameplayEffectQueue,
                                    Player player,
                                    DungeonController dungeonController) : base(stateMachine)
        {
            this.player = player;
            this.gameplayEffectQueue = gameplayEffectQueue;
            this.dungeonController = dungeonController;
        }

        public override void OnEnter()
        {
            player.SetInteractionState(PlayerInteractionState.UIOnly);

            ServiceLocator.Global.Get(out GameManager gamemanager);
            gamemanager.DeckUpgrader.UpgradeMonsterDeckRandomly(4, 6, gamemanager.BuffRegistry);
        }

        public override void Update(float deltaTime)
        {
            if (gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                gameplayEffectQueue.ResolveQueue(deltaTime);
            }
        }

        public override void OnExit() { }
    }
}