using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

public abstract class Buff : ScriptableObject
{
    [Header("Buff Meta")]
    [SerializeField] public string Name;
    [SerializeField] public string Description;

    [Header("Base Buff Parameters")]
    [SerializeField] public bool RemoveOnDeath;

    protected GameManager gameManager;
    protected CardModel owner;

    public void Initialize(CardModel owner)
    {
        ServiceLocator.Global.Get(out gameManager);
        this.owner = owner;
    }

    public abstract void OnBuffApplied();
    public abstract void OnBuffRemoved();
    public abstract void OnDraw();
    public abstract void OnOpenNewRoom();
    public abstract void OnEnterNewRoom();
    public abstract void OnRun();
    public abstract void OnMonsterDie();
    public abstract void OnEquipWeapon();
    public abstract void OnDrinkPotion();
    public abstract void OnDiscardPotion();
    public abstract void OnAttack();
    public abstract void OnCardRemoval();
    public abstract void OnUpdate();
}

public interface IBuffRegisterable
{
    public List<Buff> GetAbilities();
    public void RegisterBuff(Buff buff);
    public void DeregisterBuff(Buff buff);
}
