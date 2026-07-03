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

    public bool TryFightUnarmed(RuntimeCardModel defender)
    {
        StartNewCombat(player, null, defender as MonsterCardModel);
        return true;
    }

    public bool TryFightWeapon(RuntimeCardModel defender)
    {
        if (player.Weapon == null || player.Weapon.GetCurrentStrength() <= defender.Value)
        {
            // Attack was not successful, nothing happens
            return false;
        }

        StartNewCombat(player, player.Weapon, defender as MonsterCardModel);
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