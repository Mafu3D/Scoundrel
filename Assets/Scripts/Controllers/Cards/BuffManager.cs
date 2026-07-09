using System;
using System.Collections.Generic;
using System.Linq;
using Mafu.UnityServiceLocator;
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

public class BuffManager : IDisposable
{
    List<Buff> orderedBuffs = new();
    Dictionary<BuffID, Buff> registeredBuffs = new();

    RuntimeCardModel owner;
    GameManager gameManager;
    Player player;

    public BuffManager(RuntimeCardModel owner)
    {
        this.owner = owner;

        ServiceLocator.Global.Get(out gameManager);
        player = gameManager.Player;
    }

    public List<Buff> GetBuffs() => orderedBuffs;

    public List<Buff> GetVisibleBuffs()
    {
        List<Buff> visibleBuffs = new();
        foreach(Buff buff in orderedBuffs)
        {
            if (buff.IsHidden)
            {
                continue;
            }
            visibleBuffs.Add(buff);
        }
        return visibleBuffs;
    }

    public void Dispose()
    {
    }

    public bool HasBuff(BuffID buffID) => registeredBuffs.Keys.Contains(buffID);
    public bool HasBuff(Buff buff) => registeredBuffs.Values.Contains(buff);

    public void Update()
    {
        List<Buff> orderedBuffsCopy = new(orderedBuffs);
        orderedBuffsCopy.ForEach(n => n.OnUpdate());
    }

    public void CleanupTemporaryBuffs()
    {
        foreach(Buff buff in GetTemporaryBuffs())
        {
            Debug.Log($"Cleaning up {buff.Name}");
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
        if (!buff.ValidCardTypes.Contains(owner.CardType))
        {
            Debug.LogError($"Tried to add {buff.name} to {owner.ToString()}. Card was not of expected type. Instead was {owner.CardType}");
            return null;
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
        buff.OnBuffApplied();
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

    public void HandleOnPlayerAttackPreDamage(CombatReport attackReport) => orderedBuffs.ForEach(n => n.OnPlayerAttackPreDamage(attackReport));

    public void HandleOnPlayerAttackPostDamage(CombatReport attackReport) => orderedBuffs.ForEach(n => n.OnPlayerAttackPostDamage(attackReport));

    public void HandleOnSelfAttackedPreDamage(CombatReport combatReport) => orderedBuffs.ForEach(n => n.OnSelfAttackedPreDamage(combatReport));

    public void HandleOnSelfAttackedPostDamage(CombatReport combatReport) => orderedBuffs.ForEach(n => n.OnSelfAttackedPostDamage(combatReport));

    public void HandleOnPlayerEnterRoom() => orderedBuffs.ForEach(n => n.OnEnterRoom());

    public void HandleOnPlayerRun() => orderedBuffs.ForEach(n => n.OnRun());

    public void HandleOnOtherDiePreRemoval(MonsterCardModel other) => orderedBuffs.ForEach(n => n.OnOtherDiePreRemoval(other));

    public void HandleOnOtherDiePostRemoval(MonsterCardModel other) => orderedBuffs.ForEach(n => n.OnOtherDiePostRemoval(other));

    public void HandleOnDraw() => orderedBuffs.ForEach(n => n.OnDraw());

    public void HandleOnSelfDiePreRemoval() => orderedBuffs.ForEach(n => n.OnSelfDiePreRemoval());

    public void HandleOnSelfDiePostRemoval() => orderedBuffs.ForEach(n => n.OnSelfDiePostRemoval());

    public void HandleOnCardsChanged() => orderedBuffs.ForEach(n => n.OnCardsChanged());

    internal void HandleOnWeaponAttackPreDamage(CombatReport combatReport) => orderedBuffs.ForEach(n => n.OnWeaponAttackPreDamage(combatReport));

    internal void HandleOnWeaponAttackPostDamage(CombatReport combatReport) => orderedBuffs.ForEach(n => n.OnWeaponAttackPostDamage(combatReport));
}