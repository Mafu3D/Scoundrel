using System;
using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameManager GameManager;
    [SerializeField] public int MaxHealth { get; private set; } = 20;

    public Weapon Weapon { get; private set; }

    public Action<int> OnHealthChanged;
    public Action OnDeath;
    public Action OnWeaponChanged;

    public int CurrentHealth { get; private set; }

    void OnEnable()
    {
        GameManager.OnStartNewGame += OnStartNewGame;
    }

    void OnDisable()
    {
        GameManager.OnStartNewGame -= OnStartNewGame;
    }

    private void OnStartNewGame()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth);
        Weapon = null;
        OnWeaponChanged?.Invoke();
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

    public void EquipWeapon(Card card)
    {
        this.Weapon = new Weapon(card);
        OnWeaponChanged?.Invoke();
    }

    private void HandleDeath()
    {
        OnDeath?.Invoke();
        Debug.Log("YOU DIED");
    }
}

public class Weapon
{
    public int StartingPower { get; private set; }
    public int Power { get; private set; }
    public Card Card;
    public List<Card> SlainCards { get; private set; } = new();
    public Action OnWeaponUpdated;

    public Weapon(Card card)
    {
        Card = card;
        StartingPower = card.Value;
        Power = card.Value;
    }

    public string GetWeaponInfoString()
    {
        return $"Pow: {Power} Str: {GetCurrentStrength()} Slain: {SlainCards.Count}";
    }

    public int GetCurrentStrength()
    {
        if (SlainCards.Count == 0)
        {
            return 15;
        }

        return SlainCards[^1].Value;
    }

    public bool CanSlayMonster(int monsterStrength) => monsterStrength < GetCurrentStrength();

    public void AddMonsterToSlain(Card card)
    {
        SlainCards.Add(card);
        OnWeaponUpdated?.Invoke();
    }
}