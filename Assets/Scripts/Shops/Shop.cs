using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Sirenix.Utilities;
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
                cost: 0,
                OnPurchaseAction: (gameManager) =>
                {
                    gameManager.Player.BuffManager.AddNewBuff(buff);
                });

            shopActions.Add(newAction);
        }
    }
}
