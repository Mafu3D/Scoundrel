using System;
using System.Collections.Generic;

using Mafu.UnityServiceLocator;
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
        shopActions = new();
        for (int i = 0; i < 4; i++) // Magic number!!!
        {
            ShopAction newAction = new("Test Merchant Action", "This is a test description for a new merchant action", 3,
                (gameManager) =>
                {
                    Debug.Log("Successfully purchased the merchant action!");
                });
            shopActions.Add(newAction);
        }
    }
}

public class Blacksmith : Shop
{
    public Blacksmith(GameManager gameManager) : base(gameManager) { }

    protected override void PopulateShopActions()
    {
        shopActions = new();
        for (int i = 0; i < 8; i++) // Magic number!!!
        {
            ShopAction newAction = new("Test Blacksmith Action", "This is a test description for a new blacksmith action", 5,
                (gameManager) =>
                {
                    Debug.Log("Successfully purchased the blacksmith action!");
                });
            shopActions.Add(newAction);
        }
    }
}