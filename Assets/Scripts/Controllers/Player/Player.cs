using System;
using System.Collections.Generic;
using Project.Decks;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerInteractionState
{
    None,
    UIOnly,
    Full
}

public class Player : MonoBehaviour, IPlayerBuffRegisterable
{
    [SerializeField] public InputActionAsset inputAsset;

    [SerializeField] public int StartingMaxHealth = 20;
    [SerializeField] public int RunCooldownTime = 1;

    public bool IsAtMaxHealth => CurrentHealth == MaxHealth;
    public bool RunTokenOnCooldown => RunCooldownCounter > 0;

    public int CurrentHealth { get; private set; }
    public int MaxHealth;
    public int Armor { get; private set; }
    public WeaponCardModel Weapon { get; private set; }
    public bool HasRunToken { get; private set; } = true;
    public bool HasEnteredTheRoom { get; private set; } = false;
    public int ExtraRunTokens { get; private set; } = 0;
    public bool HasDrankPotionThisRoom { get; private set; } = false;
    public int CurrentGold { get; private set; }
    public PlayerInteractionState InteractionState { get; set; } = PlayerInteractionState.Full;
    public int RunCooldownCounter { get; private set; } = 0;

    public PlayerBuffManager BuffManager => buffManager;

    private PlayerBuffManager buffManager;

    public Action<int> OnHealthChanged;
    public Action<int> OnArmorChanged;
    public Action<int> OnGoldChanged;
    public Action<int> OnRunTokensChanged;
    public Action OnDeath;
    public Action OnWeaponChanged;
    public Action OnRunSuccess;

    private InputActionMap uiActions;


    private void Awake() {
        uiActions = inputAsset.FindActionMap("UI");

        MaxHealth = StartingMaxHealth;
    }

    public void Update()
    {
        Weapon?.Update();
        buffManager?.Update();
    }

    // public void EnableActionMap()
    // {
    //     uiActions?.Enable();
    // }

    // public void DisableActionMap()
    // {
    //     uiActions?.Disable();
    // }

    public void SetInteractionState(PlayerInteractionState state)
    {
        InteractionState = state;
    }

    public void StartNewRun()
    {
        ResetPlayer();
    }

    public void RoundReset()
    {
        // Reset the run token
        if (RunTokenOnCooldown)
        {
            RunCooldownCounter -= 1;
        }
        else
        {
            HasRunToken = true;
        }

        // Reset other things
        HasEnteredTheRoom = false;
        HasDrankPotionThisRoom = false;
    }

    public void FloorReset()
    {
        UnequipWeapon();
        // CurrentHealth = MaxHealth;
        ClearArmor();
        RunCooldownCounter = 0;
        HasEnteredTheRoom = false;
        HasRunToken = true;
        OnHealthChanged?.Invoke(CurrentHealth);

    }

    public void EnterNewRoom()
    {
        HasEnteredTheRoom = true;
        BuffManager.HandleOnPlayerEnterRoom();
    }

    public bool CanRun => (HasRunToken || ExtraRunTokens > 0) && !HasEnteredTheRoom;

    public bool TrySpendRun()
    {
        if (!CanRun)
        {
            return false;
        }

        if (HasRunToken)
        {
            HasRunToken = false;
            RunCooldownCounter = RunCooldownTime;
        }
        else if (ExtraRunTokens > 0)
        {
            TryRemoveRunTokens(1);
        }
        else
        {
            return false;
        }

        OnRunSuccess?.Invoke();
        return true;
    }

    public void TakeDamage(int amount)
    {
        int amountRemaining = Armor - Math.Abs(amount);
        if (amountRemaining < 0)
        {
            CurrentHealth = Math.Clamp(CurrentHealth - Math.Abs(amountRemaining), 0, MaxHealth);
        }
        ClearArmor();
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

    public void AddArmor(int amount)
    {
        Armor += amount;
        OnArmorChanged?.Invoke(Armor);
    }

    public void RemoveArmor(int amount)
    {
        Armor = Math.Clamp(Armor - amount, 0, 9999);
        OnArmorChanged?.Invoke(Armor);
    }

    public void ClearArmor()
    {
        Armor = 0;
        OnArmorChanged?.Invoke(Armor);
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
        BuffManager.HandleOnSelfDie();
        OnDeath?.Invoke();
        Debug.Log("YOU DIED");
    }

    private void ResetPlayer()
    {
        MaxHealth = StartingMaxHealth;
        CurrentHealth = MaxHealth;
        ClearArmor();
        RunCooldownCounter = 0;
        ExtraRunTokens = 0;
        HasRunToken = true;
        Weapon = null;
        CurrentGold = 0;
        buffManager = new PlayerBuffManager(this);

        Debug.Log("resetting player");

        OnHealthChanged?.Invoke(CurrentHealth);
        OnWeaponChanged?.Invoke();
        OnGoldChanged?.Invoke(CurrentGold);
        OnRunTokensChanged?.Invoke(ExtraRunTokens);
    }

    public List<PlayerBuff> GetBuffs() => buffManager.GetBuffs();

    public PlayerBuff AddNewBuff(PlayerBuff buff) => buffManager.AddNewBuff(buff);

    public void RemoveBuff(PlayerBuff buff) => buffManager.RemoveBuff(buff);

    public void RemoveBuff(PlayerBuffID buffID) => buffManager.RemoveBuff(buffID);

    public bool HasBuff(PlayerBuff buff) => buffManager.HasBuff(buff);

    public bool HasBuff(PlayerBuffID buffID) => buffManager.HasBuff(buffID);
}