using System;
using System.Collections.Generic;
using System.Linq;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

public class PlayerBuffManager : IDisposable
    {
    List<PlayerBuff> orderedBuffs = new();
    Dictionary<PlayerBuffID, PlayerBuff> registeredBuffs = new();

    Player owner;
    GameManager gameManager;

    public PlayerBuffManager(Player owner)
    {
        this.owner = owner;

        ServiceLocator.Global.Get(out gameManager);

        // owner.OnAttackedPreDamage += HandleOnAttackedPreDamage;
        // owner.OnAttackedPostDamage += HandleOnAttackedPostDamage;
        owner.OnAttackPreDamage += HandleOnPlayerAttackPreDamage;
        owner.OnAttackPostDamage += HandleOnPlayerAttackPostDamage;

        owner.OnDeath += HandleOnSelfDie;
        owner.OnOtherDie += HandleOnOtherDie;

        gameManager.OnPlayerEnterRoom += HandleOnPlayerEnterRoom;
        gameManager.OnOpenNewRoom += HandleOnPlayerGoToNewRoom;
        gameManager.OnPlayerRun += HandleOnPlayerRun;
        gameManager.DungeonController.OnGoToNextFloor += HandleOnPlayerGoToNewFloor;
    }

    public List<PlayerBuff> GetBuffs() => orderedBuffs;

    public List<PlayerBuff> GetVisibleBuffs()
    {
        List<PlayerBuff> visibleBuffs = new();
        foreach(PlayerBuff buff in orderedBuffs)
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
        // owner.OnAttackedPreDamage -= HandleOnAttackedPreDamage;
        // owner.OnAttackedPostDamage -= HandleOnAttackedPostDamage;
        owner.OnAttackPreDamage -= HandleOnPlayerAttackPreDamage;
        owner.OnAttackPostDamage -= HandleOnPlayerAttackPostDamage;

        owner.OnDeath -= HandleOnSelfDie;
        owner.OnOtherDie -= HandleOnOtherDie;

        gameManager.OnPlayerEnterRoom -= HandleOnPlayerEnterRoom;
        gameManager.OnOpenNewRoom -= HandleOnPlayerGoToNewRoom;
        gameManager.OnPlayerRun -= HandleOnPlayerRun;
        gameManager.DungeonController.OnGoToNextFloor -= HandleOnPlayerGoToNewFloor;
    }

    public bool HasBuff(PlayerBuffID buffID) => registeredBuffs.Keys.Contains(buffID);
    public bool HasBuff(PlayerBuff buff) => registeredBuffs.Values.Contains(buff);

    public void Update()
    {
        orderedBuffs.ForEach(n => n.OnUpdate());
    }

    public void CleanupTemporaryBuffs()
    {
        foreach(PlayerBuff buff in GetTemporaryBuffs())
        {
            DeregisterBuff(buff);
        }
    }

    private List<PlayerBuff> GetTemporaryBuffs()
    {
        List<PlayerBuff> buffs = new();
        foreach (PlayerBuff buff in orderedBuffs)
        {
            if (buff.IsTemporary)
            {
                buffs.Add(buff);
            }
        }
        return buffs;
    }

    // public void CleanupRemoveOnDeathBuffs()
    // {
    //     foreach(PlayerBuff buff in GetRemoveOnDeathBuffs())
    //     {
    //         DeregisterBuff(buff);
    //     }
    // }

    // private List<PlayerBuff> GetRemoveOnDeathBuffs()
    // {
    //     List<PlayerBuff> buffs = new();
    //     foreach (PlayerBuff buff in orderedBuffs)
    //     {
    //         if (buff.RemoveOnDeath)
    //         {
    //             buffs.Add(buff);
    //         }
    //     }
    //     return buffs;
    // }

    public PlayerBuff AddNewBuff(PlayerBuff buff)
    {
        if (buff.ID != null)
        {
            Debug.LogWarning($"{buff.name} is being added but has already been initialized with id {buff.ID.ToString()}");
        }
        PlayerBuff newInstance = UnityEngine.Object.Instantiate(buff);
        RegisterBuff(newInstance);
        return newInstance;
    }

    private void RegisterBuff(PlayerBuff buff)
    {
        buff.Initialize(owner);
        registeredBuffs.Add(buff.ID, buff);
        orderedBuffs.Add(buff);
        buff.OnBuffApplied();
    }

    public void RemoveBuff(PlayerBuffID buffID)
    {
        if (!HasBuff(buffID))
        {
            Debug.LogWarning($"Tried to remove buff {buffID.Name} on {owner} and failed!");
            return;
        }
        DeregisterBuff(registeredBuffs[buffID]);
    }

    public void RemoveBuff(PlayerBuff buff)
    {
        if (!HasBuff(buff))
        {
            Debug.LogWarning($"Tried to remove buff {buff.Name} on {owner} and failed!");
            return;
        }
        DeregisterBuff(buff);
    }

    private void DeregisterBuff(PlayerBuff buff)
    {
        foreach(PlayerBuff childBuff in buff.ChildBuffInstances)
        {
            // TODO: If a child buff is destroyed or deregistered before
            // its parent the object may become null?
            if (childBuff != null && childBuff.RemoveOnParentCleanup)
            {
                childBuff.Remove();
            }
        }
        orderedBuffs.Remove(buff);
        PlayerBuffID buffID = registeredBuffs.FirstOrDefault(x => x.Value == buff).Key;
        registeredBuffs.Remove(buffID);
        buff.Cleanup();
    }


    private void HandleOnPlayerAttackPreDamage(AttackReport attackReport) => orderedBuffs.ForEach(n => n.OnPlayerAttackPreDamage(attackReport));

    private void HandleOnPlayerAttackPostDamage(AttackReport attackReport) => orderedBuffs.ForEach(n => n.OnPlayerAttackPostDamage(attackReport));

    // private void HandleOnAttackedPostDamage(WeaponCardModel weapon) => orderedBuffs.ForEach(n => n.OnAttackedPostDamage(weapon));

    // private void HandleOnAttackedPreDamage(WeaponCardModel weapon) => orderedBuffs.ForEach(n => n.OnAttackedPreDamage(weapon));

    private void HandleOnPlayerEnterRoom() => orderedBuffs.ForEach(n => n.OnEnterRoom());

    private void HandleOnPlayerGoToNewRoom() => orderedBuffs.ForEach(n => n.OnGoToNewRoom());

    private void HandleOnPlayerRun() => orderedBuffs.ForEach(n => n.OnRun());

    private void HandleOnOtherDie(MonsterCardModel other) => orderedBuffs.ForEach(n => n.OnOtherDie(other));

    private void HandleOnSelfDie() => orderedBuffs.ForEach(n => n.OnSelfDie());

    private void HandleOnPlayerGoToNewFloor() => orderedBuffs.ForEach(n => n.OnEnterNewFloor());
}