using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    GameManager gameManager;

    void Awake()
    {
        ServiceLocator.Global.Get(out gameManager);
    }

    public abstract string Name { get; }

    public abstract void OnDraw();
    public abstract void OnOpenNewRoom();
    public abstract void OnEnterNewRoom();
    public abstract void OnRun();
    public abstract void OnMonsterDie();
    public abstract void OnEquipWeapon();
    public abstract void OnDrinkPotion();
    public abstract void OnDiscardPotion();
    public abstract void OnAttack();
    public abstract void OnDeath();
}

// public class Ability
// {
//     public Ability(AbilityDefinition definition)
//     {
//         this.definition = definition;
//     }

//     GameManager gameManager;
//     AbilityDefinition definition;

//     void Awake()
//     {
//         ServiceLocator.Global.Get(out gameManager);
//     }

//     public void OnDraw() => definition.OnDraw();
//     public abstract void OnOpenNewRoom();
//     public abstract void OnEnterNewRoom();
//     public abstract void OnRun();
//     public abstract void OnMonsterDie();
//     public abstract void OnEquipWeapon();
//     public abstract void OnDrinkPotion();
//     public abstract void OnDiscardPotion();
//     public abstract void OnAttack();
//     public abstract void OnDeath();
// }

public interface IAbilityRegisterable
{
    public List<Ability> GetAbilities();
    public void RegisterAbility(Ability ability);
    public void DeregisterAbility(Ability ability);
}