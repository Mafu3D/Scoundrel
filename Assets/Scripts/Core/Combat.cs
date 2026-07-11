using Project.Decks;
using System;
using Project.Core.StateMachineSystem;
using Project.Core;
using Project.GameStates;
using UnityEngine;

public class Combat
{
    public CombatReport combatReport;

    private readonly Player attacker;
    private readonly WeaponCardModel weapon;
    private readonly MonsterCardModel defender;
    private readonly StateMachine stateMachine;
    private readonly GameProcessQueue<GameplayEffect> gameplayEffectQueue;
    private readonly DungeonController dungeonController;

    public Combat(Player attacker,
                  WeaponCardModel weapon,
                  MonsterCardModel defender,
                  StateMachine stateMachine,
                  GameProcessQueue<GameplayEffect> gameplayEffectQueue,
                  DungeonController dungeonController)
    {
        this.stateMachine = stateMachine;
        this.gameplayEffectQueue = gameplayEffectQueue;
        this.attacker = attacker;
        this.weapon = weapon;
        this.defender = defender;
        this.dungeonController = dungeonController;

        combatReport = new CombatReport(
            attacker: attacker,
            defender: defender,
            weapon: weapon,
            damageReceived: CalculateDamage()
        );
    }


    public void TriggerPreAttackEvents()
    {
        // Handle all pre-attack events. Player is processed before the cards in the room
        attacker.BuffManager.HandleOnPlayerAttackPreDamage(combatReport);
        weapon?.BuffManager.HandleOnWeaponAttackPreDamage(combatReport);
        foreach(RuntimeCardModel card in dungeonController.CurrentRoom.RemainingCards())
        {
            card.BuffManager.HandleOnPlayerAttackPreDamage(combatReport);
        }
        defender.BuffManager.HandleOnSelfAttackedPreDamage(combatReport);
    }

    public void ProcessDamage()
    {
        attacker.TakeDamage(combatReport.DamageReceived);
        weapon?.AddMonsterToSlain(defender);
    }

    public void TriggerPostAttackEvents()
    {
        // Handle all pre-attack events. Player is processed before the cards in the room
        attacker.BuffManager.HandleOnPlayerAttackPostDamage(combatReport);
        weapon?.BuffManager.HandleOnWeaponAttackPostDamage(combatReport);
        defender.BuffManager.HandleOnSelfAttackedPostDamage(combatReport);
        foreach(RuntimeCardModel card in dungeonController.CurrentRoom.RemainingCards())
        {
            card.BuffManager.HandleOnPlayerAttackPostDamage(combatReport);
        }
    }

    public void TriggerOnDeathPreRemovalEvents()
    {
        attacker.BuffManager.HandleOnOtherDiePreRemoval(defender);
        weapon?.BuffManager.HandleOnOtherDiePreRemoval(defender);
        defender.BuffManager.HandleOnSelfDiePreRemoval();

        foreach (RuntimeCardModel other in dungeonController.CurrentRoom.GetAllOtherCards(defender))
        {
            other?.BuffManager.HandleOnOtherDiePreRemoval(defender);
        }
    }

    public void TriggerOnDeathPostRemovalEvents()
    {
        attacker.BuffManager.HandleOnOtherDiePostRemoval(defender);
        weapon?.BuffManager.HandleOnOtherDiePostRemoval(defender);
        defender.BuffManager.HandleOnSelfDiePostRemoval();

        foreach (RuntimeCardModel other in dungeonController.CurrentRoom.GetAllOtherCards(defender))
        {
            other?.BuffManager.HandleOnOtherDiePostRemoval(defender);
        }
    }

    private int CalculateDamage()
    {
        return weapon == null ? defender.Value : Math.Clamp(defender.Value - weapon.Value, 0, 999);
    }
}