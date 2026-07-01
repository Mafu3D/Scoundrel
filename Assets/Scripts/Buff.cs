using System;
using Mafu.UnityServiceLocator;
using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

public interface IBuffRegisterable
{
    public BuffManager BuffManager { get; }
    public List<Buff> GetBuffs();
    public Buff AddNewBuff(Buff buff);
    public void RemoveBuff(Buff buff);
    public void RemoveBuff(BuffID buffID);
    public bool HasBuff(Buff buff);
    public bool HasBuff(BuffID buffID);
}

public class BuffID
{
    public Guid Guid;
    public string Name;
    public BuffID(Buff buff)
    {
        Guid = Guid.NewGuid();
        Name = buff.Name;
    }

    public override string ToString()
    {
        return Guid.ToString();
    }
}

/// <summary>
/// Main class for buffs.
///
/// </summary>
public abstract class Buff : ScriptableObject
{
    [Header("Buff Meta")]
    [SerializeField] public string Name;
    [SerializeField] public Sprite Sprite;
    [SerializeField] public string Description;
    [SerializeField] public List<CardType> ValidCardTypes;

    [Header("Base Buff Parameters")]
    [SerializeField] public bool IsHidden = false;
    [SerializeField] public bool ApplyOnDraw = true;
    [SerializeField] public bool IsTemporary = false;
    [SerializeField] public bool RemoveOnParentCleanup = false;
    [SerializeField] public bool RemoveOnDeath = true;
    [SerializeField] public List<Buff> registeredChildBuffs = new();

    public List<Buff> ChildBuffInstances { get; private set; } = new();

    public BuffID ID  { get; private set; }
    public RuntimeCardModel Owner { get; private set; }

    public GameManager gameManager { get; private set; }

    public override string ToString() => $"{Name}({ID})";

    public Buff GetRegisteredChildBuffByName(string name)
    {
        foreach (Buff buff in registeredChildBuffs)
        {
            if (buff.Name == name)
            {
                return buff;
            }
        }
        throw new ArgumentException($"{name} is not a registered child buff of {this.name}", "name");
    }

    public Buff GetChildBuffInstanceByName(string name)
    {
        foreach (Buff buff in ChildBuffInstances)
        {
            if (buff.Name == name)
            {
                return buff;
            }
        }
        throw new ArgumentException($"There is no instance of {name} registered to {this.name}", "name");
    }

    public void Initialize(RuntimeCardModel owner)
    {
        Owner = owner;
        ServiceLocator.Global.Get(out GameManager _gameManager);
        gameManager = _gameManager;
        ID = new(this);
        OnBuffInitialized();
    }

    public void Remove()
    {
        Owner.RemoveBuff(this);
    }

    private void OnDestroy()
    {
        // Cleanup();
    }

    public void Cleanup()
    {
        // foreach (Buff buff in childBuffInstances)
        // {
        //     if (!buff.CleanupWithParent)
        //     {
        //         continue;
        //     }
        //     buff.Cleanup();
        // }
        OnCleanup();
    }

    public Buff AddBuff(RuntimeCardModel target, Buff buff)
    {
        Buff newInstance = target.AddNewBuff(buff);
        ChildBuffInstances.Add(newInstance);
        return newInstance;
    }

    /// <summary>
    /// Called at initialization of the buff by the Buff Manager when adding a new buff.
    /// This is for any extra initialization that needs to be
    /// done for the construction of this buff.
    /// </summary>
    public virtual void OnBuffInitialized() { }

    /// <summary>
    /// Called whenever the buff is removed, no matter the source.
    /// This is for logic that needs to run if the buff ever becomes
    /// no longe active. Like state changes.
    /// </summary>
    public virtual void OnCleanup() { }

    /// <summary>
    /// Called when the buff is considered active, after initialization.
    /// </summary>
    public virtual void OnBuffApplied() { }

    /// <summary>
    /// Called when the card is drawn, before the room has been fully created.
    /// Use this for logic that affects how cards are drawn?
    /// </summary>
    public virtual void OnDraw() { }

    /// <summary>
    /// Called when the card leaves the table for any reason.
    /// </summary>
    public virtual void OnLeave() { }

    /// <summary>
    /// Called every update cycle of the Game Manager update loop.
    /// </summary>
    public virtual void OnUpdate() { }

    public virtual void OnEnterRoom() { }
    public virtual void OnRun() { }
    public virtual void OnSelfDiePreRemoval() { }
    public virtual void OnSelfDiePostRemoval() { }
    public virtual void OnOtherDie(MonsterCardModel other) { }
    public virtual void OnCardsChanged() { }
    public virtual void OnEquipWeapon() { }
    public virtual void OnDrinkPotion() { }
    public virtual void OnDiscardPotion() { }
    public virtual void OnPlayerAttackPreDamage(AttackReport attackReport) { }
    public virtual void OnPlayerAttackPostDamage(AttackReport attackReport) { }
    public virtual void OnSelfAttackedPreDamage(AttackReport attackReport) { }
    public virtual void OnSelfAttackedPostDamage(AttackReport attackReport) { }
}