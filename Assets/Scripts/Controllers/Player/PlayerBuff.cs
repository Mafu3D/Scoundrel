using System;
using Mafu.UnityServiceLocator;
using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

public class PlayerBuffID
{
    public Guid Guid;
    public string Name;
    public PlayerBuffID(PlayerBuff buff)
    {
        Guid = Guid.NewGuid();
        Name = buff.Name;
    }

    public override string ToString()
    {
        return Guid.ToString();
    }
}

internal interface IPlayerBuffRegisterable
{
    public PlayerBuffManager BuffManager { get; }
    public List<PlayerBuff> GetBuffs();
    public PlayerBuff AddNewBuff(PlayerBuff buff);
    public void RemoveBuff(PlayerBuff buff);
    public void RemoveBuff(PlayerBuffID buffID);
    public bool HasBuff(PlayerBuff buff);
    public bool HasBuff(PlayerBuffID buffID);
}

/// <summary>
/// Main class for buffs.
///
/// </summary>
public abstract class PlayerBuff : ScriptableObject
{
    [Header("Buff Meta")]
    [SerializeField] public string Name;
    [SerializeField] public Sprite Sprite;
    [SerializeField] public string Description;

    [Header("Base Buff Parameters")]
    [SerializeField] public bool IsHidden = false;
    [SerializeField] public bool IsTemporary = false;
    [SerializeField] public bool RemoveOnParentCleanup = false;
    [SerializeField] public List<PlayerBuff> registeredChildBuffs = new();

    public List<PlayerBuff> ChildBuffInstances { get; private set; } = new();

    public PlayerBuffID ID  { get; private set; }
    public Player Owner { get; private set; }

    public GameManager gameManager { get; private set; }

    public override string ToString() => $"{Name}({ID})";

    public PlayerBuff GetRegisteredChildBuffByName(string name)
    {
        foreach (PlayerBuff buff in registeredChildBuffs)
        {
            if (buff.Name == name)
            {
                return buff;
            }
        }
        throw new ArgumentException($"{name} is not a registered child buff of {this.name}", "name");
    }

    public PlayerBuff GetChildBuffInstanceByName(string name)
    {
        foreach (PlayerBuff buff in ChildBuffInstances)
        {
            if (buff.Name == name)
            {
                return buff;
            }
        }
        throw new ArgumentException($"There is no instance of {name} registered to {this.name}", "name");
    }

    public void Initialize(Player owner)
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

    public PlayerBuff AddBuff(Player target, PlayerBuff buff)
    {
        PlayerBuff newInstance = target.AddNewBuff(buff);
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
    /// Called every update cycle of the Game Manager update loop.
    /// </summary>
    public virtual void OnUpdate() { }

    public virtual void OnGoToNewRoom() { }
    public virtual void OnEnterRoom() { }
    public virtual void OnEnterNewFloor() { }
    public virtual void OnRun() { }
    public virtual void OnSelfDie() { }
    public virtual void OnOtherDiePreRemoval(MonsterCardModel other) { }
    public virtual void OnOtherDiePostRemoval(MonsterCardModel other) { }
    public virtual void OnEquipWeapon() { }
    public virtual void OnDrinkPotion() { }
    public virtual void OnDiscardPotion() { }
    public virtual void OnPlayerAttackPreDamage(CombatReport attackReport) { }
    public virtual void OnPlayerAttackPostDamage(CombatReport attackReport) { }
}