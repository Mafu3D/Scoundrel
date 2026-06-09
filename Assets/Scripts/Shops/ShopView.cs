using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    [SerializeField] private List<ShopActionView> shopActionViews;

    private Shop shop;

    public void RegisterShop(Shop shop)
    {
        this.shop = shop;

        this.shop.OnShopActionsChanged += PopulateShopActionViews;
        PopulateShopActionViews();
    }

    private void PopulateShopActionViews()
    {
        for (int i = 0; i < shopActionViews.Count; i++)
        {
            ShopActionView view = shopActionViews[i];
            view.RegisterShop(shop);
            ShopAction shopAction = shop.GetShopAction(i);
            view.DeregisterShopAction();
            if (shopAction == null)
            {
                continue;
            }
            view.RegisterShopAction(shopAction);
        }
    }

    public void DeregisterShop()
    {
        shop.OnShopActionsChanged -= PopulateShopActionViews;
        shop = null;
        for (int i = 0; i < shopActionViews.Count; i++)
        {
            ShopActionView view = shopActionViews[i];
            view.DeregisterShopAction();
        }

    }
}
