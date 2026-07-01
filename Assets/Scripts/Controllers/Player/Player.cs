using System;
using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

public class Player : MonoBehaviour, IPlayerBuffRegisterable
{
    [SerializeField] public int MaxHealth = 20;
    [SerializeField] public int RunCooldownTime = 1;

    public int CurrentHealth { get; private set; }
    public WeaponCardModel Weapon { get; private set; }
    public bool HasRunToken { get; private set; } = true;
    public bool HasEnteredTheRoom { get; private set; } = false;
    public int ExtraRunTokens { get; private set; } = 0;
    public bool HasDrankPotionThisRoom { get; private set; } = false;
    public bool IsAtMaxHealth => CurrentHealth == MaxHealth;
    public int CurrentGold { get; private set; }

    public PlayerBuffManager BuffManager => buffManager;

    private PlayerBuffManager buffManager;

    public Action<int> OnHealthChanged;
    public Action<int> OnGoldChanged;
    public Action<int> OnRunTokensChanged;
    public Action OnDeath;
    public Action<MonsterCardModel> OnOtherDie;
    public Action OnWeaponChanged;
    public Action OnRunSuccess;
    public Action<AttackReport> OnAttackPreDamage;
    public Action<AttackReport> OnAttackPostDamage;

    private bool runTokenOnCooldown = false;

    private int runCooldownCounter = 0;

    public void Update()
    {
        Weapon?.Update();

        if (buffManager != null)
        {
            buffManager.Update();
        }
    }

    public void StartNewGame()
    {
        ResetPlayer();
        OnHealthChanged?.Invoke(CurrentHealth);
        OnWeaponChanged?.Invoke();
        OnGoldChanged?.Invoke(CurrentGold);

        buffManager = new PlayerBuffManager(this);
    }

    public void RoundReset()
    {
        // Reset the run token
        if (runTokenOnCooldown)
        {
            runCooldownCounter += 1;
            if (runCooldownCounter > RunCooldownTime)
            {
                runCooldownCounter = 0;
                runTokenOnCooldown = false;
                if (!HasRunToken)
                {
                    HasRunToken = true;
                }
            }
        }

        // Reset other things
        HasEnteredTheRoom = false;
        HasDrankPotionThisRoom = false;
    }

    public void FloorReset()
    {
        UnequipWeapon();
        // CurrentHealth = MaxHealth;
        runCooldownCounter = 0;
        runTokenOnCooldown = false;
        HasEnteredTheRoom = false;
        HasRunToken = true;
        OnHealthChanged?.Invoke(CurrentHealth);

    }

    public void EnterNewRoom()
    {
        HasEnteredTheRoom = true;
    }

    public bool CanRun() => (HasRunToken || ExtraRunTokens > 0) && !HasEnteredTheRoom;

    public bool TryRun()
    {
        if (!CanRun())
        {
            Debug.Log("tried to run but can't");
            return false;
        }

        if (HasRunToken)
        {
            HasRunToken = false;
            runTokenOnCooldown = true;
        }
        else if (ExtraRunTokens > 0)
        {
            TryRemoveRunTokens(1);
        }
        else
        {
            Debug.LogError("Tried to run but no run tokens available");
            return false;
        }

        OnRunSuccess?.Invoke();
        return true;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Math.Clamp(CurrentHealth - Math.Abs(amount), 0, MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public void Heal(int amount)
    {
        CurrentHealth = Math.Clamp(CurrentHealth + Math.Abs(amount), 0, MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        MaxHealth += amount;
        Heal(amount);
    }

    public void AddGold(int amount)
    {
        CurrentGold = Math.Clamp(CurrentGold + Math.Abs(amount), 0, 9999999);
        OnGoldChanged?.Invoke(CurrentGold);
    }

    public void AddRunTokens(int amount)
    {
        ExtraRunTokens += amount;
        OnRunTokensChanged?.Invoke(ExtraRunTokens);
    }

    public bool TryRemoveRunTokens(int amount)
    {
        if (ExtraRunTokens <= 0)
        {
            return false;
        }
        ExtraRunTokens = Math.Clamp(ExtraRunTokens - Math.Abs(amount), 0, 9999999);
        OnRunTokensChanged?.Invoke(ExtraRunTokens);
        return true;
    }

    public bool TryRemoveGold(int amount)
    {
        int newAmount = CurrentGold - Math.Abs(amount);
        if (newAmount < 0)
        {
            return false;
        }
        CurrentGold = newAmount;
        OnGoldChanged?.Invoke(CurrentGold);
        return true;
    }

    public void UnequipWeapon()
    {
        if (this.Weapon != null)
        {
            this.Weapon.Dispose();
        }
        this.Weapon = null;
        OnWeaponChanged?.Invoke();
    }

    #region Card Actions
    public bool TryFightUnarmed(RuntimeCardModel defender)
    {
        ProcessAttack(defender as MonsterCardModel, null);
        return true;
    }

    public bool TryFightWeapon(RuntimeCardModel defender)
    {
        if (Weapon == null || Weapon.GetCurrentStrength() <= defender.Value)
        {
            // Attack was not successful, nothing happens
            return false;
        }

        ProcessAttack(defender as MonsterCardModel, Weapon);
        return true;
    }

    private void ProcessAttack(MonsterCardModel defender, WeaponCardModel weapon)
    {
        // Calculate damage and create an attack report
        AttackReport attackReport = new AttackReport(
            attacker: this,
            target: defender,
            weapon: weapon,
            damageReceived: weapon == null ? defender.Value : Math.Clamp(defender.Value - weapon.Value, 0, 999)
        );

        // Run any pre-damage events first
        OnAttackPreDamage?.Invoke(attackReport);
        defender.OnSelfAttackedPreDamage?.Invoke(attackReport);

        // Apply damage and add the monster to the weapon's slain list
        TakeDamage(attackReport.DamageReceived);
        if (weapon != null)
        {
            weapon.AddMonsterToSlain(defender);
        }

        // Run any post-damage events
        OnAttackPostDamage?.Invoke(attackReport);
        defender.OnSelfAttackedPostDamage?.Invoke(attackReport);
    }

    public bool TryEquipWeapon(RuntimeCardModel card)
    {
        // Setting this up to return a bool so that conditions can be added later
        this.Weapon = new WeaponCardModel(card);
        Debug.Log($"equipping weapon {this.Weapon.GetCardInfoString()}");
        OnWeaponChanged?.Invoke();
        return true;
    }

    public bool TryDrinkPotion(RuntimeCardModel card)
    {
        if (!HasDrankPotionThisRoom)
        {
            Heal(card.Value);
            HasDrankPotionThisRoom = true;
            return true;
        }
        return false;
    }
    #endregion

    private void HandleDeath()
    {
        OnDeath?.Invoke();
        Debug.Log("YOU DIED");
    }

    private void ResetPlayer()
    {
        CurrentHealth = MaxHealth;
        runCooldownCounter = 0;
        runTokenOnCooldown = false;
        HasRunToken = true;
        Weapon = null;
        CurrentGold = 0;
    }

    public List<PlayerBuff> GetBuffs() => buffManager.GetBuffs();

    public PlayerBuff AddNewBuff(PlayerBuff buff) => buffManager.AddNewBuff(buff);

    public void RemoveBuff(PlayerBuff buff) => buffManager.RemoveBuff(buff);

    public void RemoveBuff(PlayerBuffID buffID) => buffManager.RemoveBuff(buffID);

    public bool HasBuff(PlayerBuff buff) => buffManager.HasBuff(buff);

    public bool HasBuff(PlayerBuffID buffID) => buffManager.HasBuff(buffID);
}