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
    [SerializeField] public bool RemoveOnRun;
    [SerializeField] public List<Buff> childBuffs = new();

    public BuffID ID;
    protected GameManager gameManager;
    protected CardModel owner;

    public Buff GetChildBuffByName(string name)
    {
        foreach (Buff buff in childBuffs)
        {
            if (buff.Name == name)
            {
                return buff;
            }
        }
        return default;
    }

    public void Initialize(CardModel owner)
    {
        this.owner = owner;
        ServiceLocator.Global.Get(out gameManager);
        ID = new(this);
        OnBuffInitialized();
    }

    public abstract void OnBuffInitialized();
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
