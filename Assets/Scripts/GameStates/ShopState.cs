using Project.Core;
using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class ShopState : State
    {
        private readonly ShopManager shopManager;
        private readonly Player player;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
        private readonly DungeonController dungeonController;

        public ShopState(StateMachine stateMachine,
                         ShopManager shopManager,
                         GameProcessQueue<GameplayEffect> gameplayEffectQueue,
                         Player player,
                         DungeonController dungeonController) : base(stateMachine)
        {
            this.shopManager = shopManager;
            this.player = player;
            this.gameplayEffectQueue = gameplayEffectQueue;
            this.dungeonController = dungeonController;
        }

        public override void OnEnter()
        {
            player.SetInteractionState(PlayerInteractionState.Full);

            shopManager.StartNewShopPhase();
            shopManager.gameObject.SetActive(true);
        }

        public override void Update(float deltaTime)
        {
            if (gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                gameplayEffectQueue.ResolveQueue(deltaTime);
            }
        }

        public override void OnExit()
        {
            shopManager.gameObject.SetActive(false);
            shopManager.ExitShopPhase();
        }
    }
}