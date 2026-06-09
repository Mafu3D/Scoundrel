using TMPro;
using UnityEngine;

public class ShopActionView : MonoBehaviour
{
    [SerializeField] private TMP_Text title_text;
    [SerializeField] private TMP_Text description_text;
    [SerializeField] private TMP_Text gold_text;

    private Shop shop;
    private ShopAction shopAction;

    public void RegisterShop(Shop shop)
    {
        this.shop = shop;
    }

    public void RegisterShopAction(ShopAction shopAction)
    {
        this.shopAction = shopAction;
        title_text.text = this.shopAction.Title;
        description_text.text = this.shopAction.Description;
        gold_text.text = $"Costs {this.shopAction.Cost} gold";
    }

    public void DeregisterShopAction()
    {
        this.shopAction = null;
        title_text.text = "None";
        description_text.text = "None";
        gold_text.text = $"";
    }

    public void OnActionClicked()
    {
        if (shopAction == null)
        {
            return;
        }

        shop.TryPurchaseAction(shopAction);
    }
}
