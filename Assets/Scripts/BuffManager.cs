using System;
using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using UnityEngine;

public enum BuffTrigger
{
    OnBuffApplied,
    OnDraw,
    OnEnterRoom,
    OnRun,
    OnOtherDie,
    OnSelfDie,
    OnEquipWeapon,
    OnDrinkPotion,
    OnDiscardPotion,
    OnAttack,
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

    public bool HasBuff(BuffID buffID) => registeredBuffs.Keys.Contains(buffID);
    public bool HasBuff(Buff buff) => registeredBuffs.Values.Contains(buff);

    public void Update()
    {
        TriggerEffect(BuffTrigger.OnUpdate);
    }

    public void CleanupTemporaryBuffs()
    {
        foreach(Buff buff in GetTemporaryBuffs())
        {
            DeregisterBuff(buff);
        }
    }

    private List<Buff> GetTemporaryBuffs()
    {
        List<Buff> buffs = new();
        foreach (Buff buff in orderedBuffs)
        {
            if (buff.IsTemporary)
            {
                buffs.Add(buff);
            }
        }
        return buffs;
    }

    public void CleanupRemoveOnDeathBuffs()
    {
        foreach(Buff buff in GetRemoveOnDeathBuffs())
        {
            DeregisterBuff(buff);
        }
    }

    private List<Buff> GetRemoveOnDeathBuffs()
    {
        List<Buff> buffs = new();
        foreach (Buff buff in orderedBuffs)
        {
            if (buff.RemoveOnDeath)
            {
                buffs.Add(buff);
            }
        }
        return buffs;
    }

    public Buff AddNewBuff(Buff buff)
    {
        if (buff.ID != null)
        {
            Debug.LogWarning($"{buff.name} is being added but has already been initialized with id {buff.ID.ToString()}");
        }
        Buff newInstance = UnityEngine.Object.Instantiate(buff);
        RegisterBuff(newInstance);
        return newInstance;
    }

    private void RegisterBuff(Buff buff)
    {
        buff.Initialize(owner);
        registeredBuffs.Add(buff.ID, buff);
        orderedBuffs.Add(buff);
        buff.TriggerEffect(BuffTrigger.OnBuffApplied);
    }

    public void RemoveBuff(BuffID buffID)
    {
        if (!HasBuff(buffID))
        {
            Debug.LogWarning($"Tried to remove buff {buffID.Name} on {owner} and failed!");
            return;
        }
        DeregisterBuff(registeredBuffs[buffID]);
    }

    public void RemoveBuff(Buff buff)
    {
        if (!HasBuff(buff))
        {
            Debug.LogWarning($"Tried to remove buff {buff.Name} on {owner} and failed!");
            return;
        }
        DeregisterBuff(buff);
    }

    private void DeregisterBuff(Buff buff)
    {
        foreach(Buff childBuff in buff.ChildBuffInstances)
        {
            // TODO: If a child buff is destroyed or deregistered before
            // its parent the object may become null?
            if (childBuff != null && childBuff.RemoveOnParentCleanup)
            {
                childBuff.Remove();
            }
        }
        orderedBuffs.Remove(buff);
        BuffID buffID = registeredBuffs.FirstOrDefault(x => x.Value == buff).Key;
        registeredBuffs.Remove(buffID);
        buff.Cleanup();
    }


    public void TriggerEffect(BuffTrigger trigger)
    {
        foreach(Buff buff in orderedBuffs)
        {
            buff.TriggerEffect(trigger);
        }
    }
}