using System;
using Project.Decks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int MaxHealth = 20;

    public int CurrentHealth { get; private set; }
    public Weapon Weapon { get; private set; }
    public bool HasRunToken { get; private set; } = true;
    public bool HasEnteredTheRoom { get; private set; } = false;
    public int ExtraRunTokens { get; private set; } = 0;
    public bool HasDrankPotionThisRoom { get; private set; } = false;

    public Action<int> OnHealthChanged;
    public Action OnDeath;
    public Action OnWeaponChanged;
    public Action OnRunSuccess;

    private bool runTokenOnCooldown = false;

    public void StartNewGame()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth);
        Weapon = null;
        OnWeaponChanged?.Invoke();
    }

    public void RoundReset()
    {
        // Reset the run token
        if (runTokenOnCooldown)
        {
            // TODO: Add counter here so that run token can have variable cooldown
            runTokenOnCooldown = false;
            if (!HasRunToken)
            {
                HasRunToken = true;
            }
        }

        // Reset other things
        HasEnteredTheRoom = false;
        HasDrankPotionThisRoom = false;
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

    #region Card Actions
    public bool TryFightUnarmed(CardModel card)
    {
        // Setting this up to return a bool so that conditions can be added later
        TakeDamage(card.Value);
        return true;
    }

    public bool TryFightWeapon(CardModel card)
    {
        if (Weapon == null || Weapon.GetCurrentStrength() <= card.Value)
        {
            return false;
        }
        int damage = Math.Clamp(card.Value - Weapon.Power, 0, 999);
        TakeDamage(damage);
        Weapon.AddMonsterToSlain(card);
        return true;
    }

    public bool TryEquipWeapon(CardModel card)
    {
        // Setting this up to return a bool so that conditions can be added later
        this.Weapon = new Weapon(card);
        OnWeaponChanged?.Invoke();
        return true;
    }

    public bool TryDrinkPotion(CardModel card)
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
}
