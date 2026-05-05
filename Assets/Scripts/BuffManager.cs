using System;
using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using UnityEngine;

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
    List<Buff> orderedBuffs = new();
    Dictionary<BuffID, Buff> registeredBuffs = new();
    public List<Buff> GetBuffs() => orderedBuffs;

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
        foreach (Buff buff in orderedBuffs)
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
        List<Buff> buffs = new();
        foreach (Buff buff in orderedBuffs)
        {
            if (buff.RemoveOnRun)
            {
                buffs.Add(buff);
            }
        }
        return buffs;
    }

    public BuffID RegisterBuff(Buff buffDefinition)
    {
        Buff buff = UnityEngine.Object.Instantiate(buffDefinition);
        buff.Initialize(owner);
        registeredBuffs.Add(buff.ID, buff);
        Debug.Log(registeredBuffs.Count);
        orderedBuffs.Add(buff);
        buff.OnBuffApplied();

        return buff.ID;
    }

    public void DeregisterBuff(BuffID buffID)
    {
        Debug.Log(registeredBuffs.Count);
        if (registeredBuffs.Keys.Contains(buffID))
        {
            Buff buff = registeredBuffs[buffID];
            orderedBuffs.Remove(buff);
            registeredBuffs.Remove(buffID);
            buff.OnBuffRemoved();
            return;
        }
        Debug.LogWarning($"Tried to remove buff {buffID.Name} on {owner} and failed!");
    }

    public void DeregisterBuff(Buff buff)
    {
        if (registeredBuffs.Values.Contains(buff))
        {
            BuffID buffID = registeredBuffs.FirstOrDefault(x => x.Value == buff).Key;
            orderedBuffs.Remove(buff);
            registeredBuffs.Remove(buffID);
            buff.OnBuffRemoved();
            return;
        }
        Debug.LogWarning($"Tried to remove buff {buff.Name} on {owner} and failed!");
    }


    public void ActivateBuffTrigger(BuffTrigger trigger)
    {
        foreach(Buff buff in orderedBuffs)
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