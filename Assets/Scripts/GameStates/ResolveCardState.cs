using Project.Core;
using Project.Core.StateMachineSystem;
using Project.Decks;

namespace Project.GameStates
{
    public class ResolveCardState : State
    {
        private readonly RuntimeCardModel card;
        private readonly Player player;
        private readonly DungeonController dungeonController;
        private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
        private readonly AdvancedScoreKeeper scoreKeeper;

        public ResolveCardState(RuntimeCardModel card,
                                StateMachine stateMachine,
                                GameProcessQueue<GameplayEffect> gameplayEffectQueue,
                                Player player,
                                DungeonController dungeonController,
                                AdvancedScoreKeeper scoreKeeper) : base(stateMachine)
        {
            this.card = card;
            this.player = player;
            this.dungeonController = dungeonController;
            this.gameplayEffectQueue = gameplayEffectQueue;
            this.scoreKeeper = scoreKeeper;
        }

        public override void OnEnter()
        {
            player.SetInteractionState(PlayerInteractionState.UIOnly);

            if (!player.HasEnteredTheRoom)
            {
                // Player enter room
                player.EnterNewRoom();
                foreach(RuntimeCardModel card in dungeonController.CurrentRoom.RemainingCards())
                {
                    card.BuffManager.HandleOnPlayerEnterRoom();
                }
            }

            dungeonController.CurrentRoom.TryRemoveCard(card);

            // Add gold if its the last card in the room
            if (dungeonController.CurrentRoom.IsEmpty)
            {
                player.AddGold(2);
            }

            scoreKeeper.AddToScore(card);
            scoreKeeper.IncRoomMultiplier();

            card.Kill();
        }

        public override void Update(float deltaTime)
        {
            if (!gameplayEffectQueue.QueueNeedsToBeResolved)
            {
                StateMachine.SwitchState(new RoomActiveState(StateMachine,
                                                             gameplayEffectQueue,
                                                             player,
                                                             dungeonController));
                return;
            }

            gameplayEffectQueue.ResolveQueue(deltaTime);
        }

        public override void OnExit() { }
    }
}