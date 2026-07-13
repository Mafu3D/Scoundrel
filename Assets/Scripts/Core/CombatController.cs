using Project.Decks;
using Project.Core.StateMachineSystem;
using Project.Core;
using Project.GameStates;

public class CombatController
{
    private readonly Player player;
    private readonly StateMachine stateMachine;
    private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
    private readonly DungeonController dungeonController;
    private readonly AdvancedScoreKeeper scoreKeeper;

    public CombatController(Player player, StateMachine stateMachine, GameProcessQueue<GameplayEffect> gameplayEffectQueue, DungeonController dungeonController, AdvancedScoreKeeper scoreKeeper)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.gameplayEffectQueue = gameplayEffectQueue;
        this.dungeonController = dungeonController;
        this.scoreKeeper = scoreKeeper;
    }

    public bool CheckForTaunt(MonsterCardModel defender)
    {
        if (defender.HasTaunt)
        {
            return true;
        }
        else
        {
            foreach(RuntimeCardModel card in dungeonController.CurrentRoom.GetOtherActiveCards(defender))
            {
                if (card is MonsterCardModel)
                {
                    if ((card as MonsterCardModel).HasTaunt)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public bool TryFightUnarmed(MonsterCardModel defender)
    {
        if (!CheckForTaunt(defender)) return false;
        StartNewCombat(player, null, defender);
        return true;
    }

    public bool TryFightWeapon(MonsterCardModel defender)
    {
        if (!CheckForTaunt(defender)) return false;
        if (player.Weapon == null || player.Weapon.GetCurrentStrength() <= defender.Value)
        {
            // Attack was not successful, nothing happens
            return false;
        }

        StartNewCombat(player, player.Weapon, defender);
        return true;
    }

    private void StartNewCombat(Player attacker, WeaponCardModel weapon, MonsterCardModel defender)
    {
        Combat newCombat = new Combat(attacker, weapon, defender, stateMachine, gameplayEffectQueue, dungeonController);
        stateMachine.SwitchState(new PreAttackState(newCombat,
                                                    stateMachine,
                                                    gameplayEffectQueue,
                                                    attacker,
                                                    dungeonController,
                                                    scoreKeeper));
    }
}