using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEditor.PackageManager;
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

    protected GameManager gameManager;
    protected CardModel owner;

    public void Initialize(CardModel owner)
    {
        ServiceLocator.Global.Get(out gameManager);
        this.owner = owner;
        OnBuffInitialized();
    }

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

public interface IBuffRegisterable
{
    public BuffManager Buffs { get; }
    public BuffManager GetBuffs();
    public void RegisterBuff(Buff buff);
    public void DeregisterBuff(Buff buff);
}

public enum BuffTrigger
{
    OnBuffInitialized,
    OnBuffApplied,
    OnBuffRemoved,
    OnDraw,
    OnOpenNewRoom,
    OnEnterNewRoom,
    OnRun,
    OnMonsterDie,
    OnEquipWeapon,
    OnDrinkPotion,
    OnDiscardPotion,
    OnAttack,
    OnCardRemoval,
    OnUpdate,
}

public class BuffManager
{
    List<Buff> registeredBuffs = new();
    public List<Buff> GetBuffs() => registeredBuffs;

    CardModel owner;

    public BuffManager(CardModel owner)
    {
        this.owner = owner;
    }

    public void Update()
    {
        ActivateBuffTrigger(BuffTrigger.OnUpdate);
    }

    public void OnDeath()
    {
        ActivateBuffTrigger(BuffTrigger.OnCardRemoval);
        foreach(Buff buff in GetRemoveOnDeathBuffs())
        {
            buff.OnCardRemoval();
            DeregisterBuff(buff);
        }
    }

    public void OnRun()
    {
        ActivateBuffTrigger(BuffTrigger.OnRun);
        foreach(Buff buff in GetRemoveOnRunBuffs())
        {
            buff.OnCardRemoval();
            DeregisterBuff(buff);
        }
    }

    private List<Buff> GetRemoveOnDeathBuffs()
    {
        List<Buff> buffsToBeRemoved = new();
        foreach (Buff buff in registeredBuffs)
        {
            if (buff.RemoveOnDeath)
            {
                buffsToBeRemoved.Add(buff);
            }
        }
        return buffsToBeRemoved;
    }

    private List<Buff> GetRemoveOnRunBuffs()
    {
        List<Buff> buffsToBeRemoved = new();
        foreach (Buff buff in registeredBuffs)
        {
            if (buff.RemoveOnRun)
            {
                buffsToBeRemoved.Add(buff);
            }
        }
        return buffsToBeRemoved;
    }

    public void RegisterBuff(Buff buff)
    {
        registeredBuffs.Add(buff);
        buff.Initialize(owner);
        buff.OnBuffApplied();
    }

    public void DeregisterBuff(Buff buff)
    {
        if (registeredBuffs.Contains(buff))
        {
            buff.OnBuffRemoved();
            registeredBuffs.Remove(buff);
        }
    }


    public void ActivateBuffTrigger(BuffTrigger trigger)
    {
        foreach(Buff buff in registeredBuffs)
        {
            switch (trigger)
            {
                case BuffTrigger.OnBuffInitialized:
                    buff.OnBuffInitialized();
                    break;
                case BuffTrigger.OnBuffApplied:
                    buff.OnBuffApplied();
                    break;
                case BuffTrigger.OnBuffRemoved:
                    buff.OnBuffRemoved();
                    break;
                case BuffTrigger.OnDraw:
                    buff.OnDraw();
                    break;
                case BuffTrigger.OnOpenNewRoom:
                    buff.OnOpenNewRoom();
                    break;
                case BuffTrigger.OnEnterNewRoom:
                    buff.OnEnterNewRoom();
                    break;
                case BuffTrigger.OnRun:
                    buff.OnRun();
                    break;
                case BuffTrigger.OnMonsterDie:
                    buff.OnMonsterDie();
                    break;
                case BuffTrigger.OnEquipWeapon:
                    buff.OnEquipWeapon();
                    break;
                case BuffTrigger.OnDrinkPotion:
                    buff.OnDrinkPotion();
                    break;
                case BuffTrigger.OnDiscardPotion:
                    buff.OnDiscardPotion();
                    break;
                case BuffTrigger.OnAttack:
                    buff.OnAttack();
                    break;
                case BuffTrigger.OnCardRemoval:
                    buff.OnCardRemoval();
                    break;
                case BuffTrigger.OnUpdate:
                    buff.OnUpdate();
                    break;
                default:
                    Debug.LogError($"{trigger} is not accepted");
                    break;
            }
        }
    }

    internal void DeregisterBuff()
    {
        throw new NotImplementedException();
    }
}