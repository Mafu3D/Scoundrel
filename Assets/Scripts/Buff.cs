using System;
using Mafu.UnityServiceLocator;
using System.Collections.Generic;
using Project.Decks;
using UnityEngine;
using System.Linq;

public interface IBuffRegisterable<T> where T : CardModel
{
    public BuffManager BuffManager { get; }
    public List<Buff<T>> GetBuffs();
    public Buff<T> AddNewBuff(Buff<T> buff);
    public void RemoveBuff(Buff<T> buff);
    public void RemoveBuff(BuffID buffID);
    public bool HasBuff(Buff<T> buff);
    public bool HasBuff(BuffID buffID);
}

public class BuffID
{
    public Guid Guid;
    public string Name;
    public BuffID(string name)
    {
        Guid = Guid.NewGuid();
        Name = name;
    }

    public override string ToString()
    {
        return Guid.ToString();
    }
}

public abstract class Buff<T> : ScriptableObject where T : CardModel
{
    [Header("Buff Meta")]
    [SerializeField] public string Name;
    [SerializeField] public string Description;

    [Header("Base Buff Parameters")]
    [SerializeField] public bool ApplyOnDraw = true;
    [SerializeField] public bool IsTemporary = false;
    [SerializeField] public bool RemoveOnParentCleanup = false;
    [SerializeField] public bool RemoveOnDeath = true;
    [SerializeField] public List<Buff<T>> registeredChildBuffs = new();

    public List<Buff<T>> ChildBuffInstances { get; private set; } = new();

    public BuffID ID;
    public virtual T Owner { get; protected set; }

    protected GameManager gameManager;

    public override string ToString() => $"{Name}({ID})";

    public TBuff GetRegisteredChildBuffByName<TBuff>(string name) where TBuff : Buff<T>
    {
        foreach (TBuff buff in registeredChildBuffs.Cast<TBuff>())
        {
            if (buff.Name == name)
            {
                return buff;
            }
        }
        throw new ArgumentException($"{name} is not a registered child buff of {this.name}", "name");
    }

    public TBuff GetChildBuffInstanceByName<TBuff>(string name) where TBuff : Buff<T>
    {
        foreach (TBuff buff in ChildBuffInstances.Cast<TBuff>())
        {
            if (buff.Name == name)
            {
                return buff;
            }
        }
        throw new ArgumentException($"There is no instance of {name} registered to {this.name}", "name");
    }

    public void Initialize(T owner)
    {
        this.Owner = owner;
        ServiceLocator.Global.Get(out gameManager);
        ID = new(Name);
        OnBuffInitialized();
    }

    // public void Remove()
    // {
    //     Owner.RemoveBuff(this);
    // }

    private void OnDestroy()
    {
    }

    public void Cleanup()
    {
        OnCleanup();
    }

    public void TriggerEffect(BuffTrigger trigger)
    {
        GetTriggerCallable(trigger)();
    }

    private Action GetTriggerCallable(BuffTrigger trigger)
    {
        return trigger switch
        {
            BuffTrigger.OnBuffApplied => OnBuffApplied,
            BuffTrigger.OnDraw => OnDraw,
            BuffTrigger.OnEnterRoom => OnEnterRoom,
            BuffTrigger.OnRun => OnRun,
            BuffTrigger.OnOtherDie => OnOtherDie,
            BuffTrigger.OnSelfDie => OnSelfDie,
            BuffTrigger.OnEquipWeapon => OnEquipWeapon,
            BuffTrigger.OnDrinkPotion => OnDrinkPotion,
            BuffTrigger.OnDiscardPotion => OnDiscardPotion,
            BuffTrigger.OnAttack => OnAttack,
            BuffTrigger.OnUpdate => OnAttack,
            _ => throw new ArgumentOutOfRangeException(nameof(trigger), $"{nameof(trigger)} has not been registered as a callable trigger.")
        };
    }

    // protected TBuff AddBuff<TBuff>(IBuffRegisterable target, TBuff buff) where TBuff : Buff<T>
    // {
    //     TBuff newInstance = target.AddNewBuff(buff);
    //     ChildBuffInstances.Add(newInstance);
    //     return newInstance;
    // }

    /// <summary>
    /// Called at initialization of the buff by the Buff Manager when adding a new buff.
    /// This is for any extra initialization that needs to be
    /// done for the construction of this buff.
    /// </summary>
    protected abstract void OnBuffInitialized();

    /// <summary>
    /// Called whenever the buff is removed, no matter the source.
    /// This is for logic that needs to run if the buff ever becomes
    /// no longe active. Like state changes.
    /// </summary>
    protected abstract void OnCleanup();

    /// <summary>
    /// Called when the buff is considered active, after initialization.
    /// </summary>
    protected abstract void OnBuffApplied();

    /// <summary>
    /// Called when the card is drawn, before the room has been fully created.
    /// Use this for logic that affects how cards are drawn?
    /// </summary>
    protected abstract void OnDraw();

    /// <summary>
    /// Called when the card leaves the table for any reason.
    /// </summary>
    protected abstract void OnLeave();

    /// <summary>
    /// Called every update cycle of the Game Manager update loop.
    /// </summary>
    protected abstract void OnUpdate();

    protected abstract void OnEnterRoom();
    protected abstract void OnRun();
    protected abstract void OnSelfDie();
    protected abstract void OnOtherDie();
    protected abstract void OnEquipWeapon();
    protected abstract void OnDrinkPotion();
    protected abstract void OnDiscardPotion();
    protected abstract void OnAttack();
}

public abstract class CardBuff : Buff<CardModel>
{
    public override CardModel Owner { get; protected set; }

    public override string ToString() => $"{Name}({ID})";

    protected CardBuff AddBuff(CardModel target, CardBuff buff)
    {
        CardBuff newInstance = target.AddNewBuff(buff);
        ChildBuffInstances.Add(newInstance);
        return newInstance;
    }
}

public abstract class WeaponBuff : Buff<WeaponModel>
{
    public override WeaponModel Owner { get; protected set; }

    public override string ToString() => $"{Name}({ID})";

    protected WeaponBuff AddBuff(WeaponModel target, WeaponBuff buff)
    {
        WeaponBuff newInstance = target.AddNewBuff(buff);
        ChildBuffInstances.Add(newInstance);
        return newInstance;
    }
}