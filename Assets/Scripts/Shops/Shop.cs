using System;
using System.Collections.Generic;

using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

public abstract class Shop
{
    public Action OnShopActionsChanged;
    protected List<ShopAction> shopActions;
    protected GameManager gameManager;

    public Shop(GameManager gameManager)
    {
        this.gameManager = gameManager;
        PopulateShopActions();
    }

    public ShopAction GetShopAction(int index)
    {
        if (index >= shopActions.Count)
        {
            return null;
        }
        return shopActions[index];
    }

    public bool TryPurchaseAction(ShopAction targetAction)
    {
        if (targetAction == null || !shopActions.Contains(targetAction))
        {
            return false;
        }

        int cost = targetAction.Cost;
        if (!gameManager.Player.TryRemoveGold(cost))
        {
            return false;
        }

        targetAction.OnPurchase(gameManager);

        int index = shopActions.IndexOf(targetAction);
        shopActions[index] = null;

        OnShopActionsChanged?.Invoke();
        return true;
    }

    protected abstract void PopulateShopActions();
}

public class ShopAction
{
    public readonly int Cost;
    public readonly string Title;
    public readonly string Description;
    public readonly Action<GameManager> OnPurchaseAction;

    public ShopAction(string title, string description, int cost, Action<GameManager> OnPurchaseAction)
    {
        Title = title;
        Description = description;
        Cost = cost;
        this.OnPurchaseAction = OnPurchaseAction;
    }

    public void OnPurchase(GameManager gameManager)
    {
        OnPurchaseAction?.Invoke(gameManager);
    }
}

public class Merchant : Shop
{
    public Merchant(GameManager gameManager) : base(gameManager) { }

    protected override void PopulateShopActions()
    {
        int AMOUNT = 4; // Magic number!!
        shopActions = new();
        for (int i = 0; i < AMOUNT; i++)
        {
            PlayerBuff buff = gameManager.BuffRegistry.GetRandomPlayerBuff();
            if (buff == null)
            {
                Debug.LogWarning("No player buffs available to add to the shop.");
                continue;
            }

            ShopAction newAction = new(
                title: buff.Name,
                description: buff.Description,
                cost: 10,
                OnPurchaseAction: (gameManager) =>
                {
                    gameManager.Player.BuffManager.AddNewBuff(buff);
                });

            shopActions.Add(newAction);
        }
    }
}

public class Blacksmith : Shop
{
    readonly List<string> buffs = new() { "Bomb", "Cleaving", "Honed", "Reinforced" };

    public Blacksmith(GameManager gameManager) : base(gameManager) { }

    protected override void PopulateShopActions()
    {
        int AMOUNT = 4; // Magic number!!
        shopActions = new();

        // Get cards
        RuntimeCardModel[] cardsToBuff = new RuntimeCardModel[AMOUNT];
        List<RuntimeCardModel> weaponCards = gameManager.DeckController.GetRemainingOfSuit(new() {Suit.DIAMONDS});
        for (int i = 0; i < AMOUNT; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, weaponCards.Count);
            cardsToBuff[i] = weaponCards[randIndex];
        }

        // Create the shop actions
        foreach (RuntimeCardModel card in cardsToBuff)
        {
            // Get the random buff
            int randBuff = UnityEngine.Random.Range(0, buffs.Count);
            Buff buff = gameManager.BuffRegistry.GetBuffFromName(buffs[randBuff]);

            // Create the action
            string title = $"{card.GetName()}\n + {buff.Name}";
            string description = "";
            if (card.BuffManager.GetBuffs().Count > 0)
            {
                description += "EXISTING: ";
                foreach (Buff existingBuff in card.BuffManager.GetBuffs())
                {
                    description += $"{existingBuff.Name}, ";
                }
                description += "\n\n";
            }
            description += $"NEW:\n{buff.Description}";
            // string description = $"{buff.Description}\n\nAdd to {card.GetName()}";
            // foreach(Buff existingBuff in card.GetBuffs())
            // {
            //     description += $"\n   -{existingBuff.Name}";
            // }

            ShopAction newAction = new(
                title,
                description,
                5,
                (gameManager) =>
                {
                    card.AddNewBuff(buff);
                });
            shopActions.Add(newAction);
        }
    }


}