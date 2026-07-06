using System;
using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using UnityEngine;

public class Blacksmith
{
    public BlacksmithBuffItem CurrentlySelectedBuffItem;
    public BlacksmithWeaponItem CurrentlySelectedWeaponItem;
    public BlacksmithBuffItem[] AvailableBuffItems { get; private set; }
    public BlacksmithWeaponItem[] AvailableWeaponItems { get; private set; }

    public Action OnBuffInventoryChanged;
    public Action OnWeaponInventoryChanged;
    public Action OnBuffSelected;
    public Action OnWeaponSelected;

    private readonly Player player;
    private readonly DeckController deckController;
    private readonly GlobalBuffRegistry buffRegistry;
    private readonly int size;

    public Blacksmith(Player player, DeckController deckController, GlobalBuffRegistry buffRegistry, int size)
    {
        this.player = player;
        this.deckController = deckController;
        this.buffRegistry = buffRegistry;
        this.size = size;

        RollRandomWeapons(size);
        RollRandomBuffs(size);
    }

    public void Update()
    {
        foreach(BlacksmithWeaponItem weaponItem in AvailableWeaponItems)
        {
            weaponItem.Card.Update();
        }
    }

    public void SelectBuffItem(BlacksmithBuffItem blacksmithBuffItem)
    {
        if (!AvailableBuffItems.Contains(blacksmithBuffItem))
        {
            Debug.LogWarning("Tried to select a buff item that is not registered to this instance of the blacksmith!");
        }

        CurrentlySelectedBuffItem = blacksmithBuffItem;
        OnBuffSelected?.Invoke();
    }

    public void DeselectBuffItem()
    {
        CurrentlySelectedBuffItem = null;
        OnBuffSelected?.Invoke();
    }

    public void SelectWeapon(BlacksmithWeaponItem blacksmithWeaponItem)
    {
        if (CurrentlySelectedBuffItem == null)
        {
            Debug.LogWarning("Must select a buff first before selecting a weapon!");
            return;
        }

        if (!AvailableWeaponItems.Contains(blacksmithWeaponItem))
        {
            Debug.LogWarning($"Tried to select a weapon that is not registered to this blacksmith instance!");
            return;
        }

        CurrentlySelectedWeaponItem = blacksmithWeaponItem;
        OnWeaponSelected?.Invoke();
    }

    public void DeselectWeapon()
    {
        CurrentlySelectedWeaponItem = null;
        OnWeaponSelected?.Invoke();
    }

    public void DeselectAll()
    {
        DeselectBuffItem();
        DeselectWeapon();
    }

    public bool TryApplyUpgrade()
    {
        if (CurrentlySelectedBuffItem == null || CurrentlySelectedWeaponItem == null)
        {
            Debug.Log("A valid buff item and weapon item must be selected to apply the ugprade!");
            return false;
        }

        CurrentlySelectedWeaponItem.Card.AddNewBuff(CurrentlySelectedBuffItem.Buff);
        CurrentlySelectedWeaponItem.NumberOfTimesUpgraded++;
        return true;
    }

    public void RollRandomWeapons(int amount)
    {
        AvailableWeaponItems = GetRandomlyChosenWeapons(amount);
        OnWeaponInventoryChanged?.Invoke();
    }

    public void RollRandomBuffs(int amount)
    {
        AvailableBuffItems = GetRandomlyChosenWeaponBuffs(amount);
        OnBuffInventoryChanged?.Invoke();
    }

    private BlacksmithWeaponItem[] GetRandomlyChosenWeapons(int targetAmount)
    {
        // Get all weapon cards in the deck
        List<RuntimeCardModel> randomWeapons = deckController.GetRemainingOfSuit(new() {Suit.DIAMONDS});

        // Get the amount of weaponst to draw, either the maximum amount or the length of the weaponCards
        int amount = Math.Min(randomWeapons.Count, targetAmount);

        // Get random indices of the weapon cards
        int[] randomIndices = GetUniqueRandomIndices(amount, 0, randomWeapons.Count);

        // Assign the random weapons to the random cards
        BlacksmithWeaponItem[] blacksmithWeaponItems = new BlacksmithWeaponItem[amount];
        for (int i = 0; i < amount; i++)
        {
            blacksmithWeaponItems[i] = new (randomWeapons[randomIndices[i]], 1); // TODO: Get amount of upgrades later... somehow??
        }

        return blacksmithWeaponItems;
    }

    private BlacksmithBuffItem[] GetRandomlyChosenWeaponBuffs(int targetAmount)
    {
        Buff[] randomBuffs = new Buff[size];

        for (int i = 0; i < size; i++)
        {
            randomBuffs[i] = buffRegistry.GetRandomBuff(CardType.WEAPON);
        }

        BlacksmithBuffItem[] blacksmithBuffItems = new BlacksmithBuffItem[size];
        for (int i = 0; i < randomBuffs.Length; i++)
        {
            blacksmithBuffItems[i] = new (randomBuffs[i], 5); // TODO: Get cost later... somehow??
        }

        return blacksmithBuffItems;
    }

    private int[] GetUniqueRandomIndices(int amount, int min, int max)
    {
        HashSet<int> randomIndices = new();
        while (randomIndices.Count < amount)
        {
            randomIndices.Add(UnityEngine.Random.Range(min, max));
        }
        return randomIndices.ToArray();
    }
}

public class BlacksmithBuffItem
{
    public readonly Buff Buff;
    public int Cost;

    public BlacksmithBuffItem(Buff buff, int cost)
    {
        Buff = buff;
        Cost = cost;
    }
}

public class BlacksmithWeaponItem
{
    public readonly RuntimeCardModel Card;
    public readonly int TotalUpgradesAmount;
    public int NumberOfTimesUpgraded;

    public bool CanUpgrade => NumberOfTimesUpgraded < TotalUpgradesAmount;

    public BlacksmithWeaponItem(RuntimeCardModel card, int totalUpgradesAmount)
    {
        Card = card;
        TotalUpgradesAmount = totalUpgradesAmount;
    }
}