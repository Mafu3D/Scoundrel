using System;
using Mafu.UnityServiceLocator;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopMenuContainer;
    [SerializeField] private ShopView merchantView;
    [SerializeField] private ShopView blacksmithView;

    private GameManager gameManager;
    private Merchant merchant;
    private Blacksmith blacksmith;

    private bool hasRestedThisPhase = false;

    public void StartNewShopPhase()
    {
        ServiceLocator.Global.Get(out gameManager);
        merchant = new (gameManager);
        merchantView.RegisterShop(merchant);
        blacksmith = new (gameManager);
        blacksmithView.RegisterShop(blacksmith);

        shopMenuContainer.SetActive(true);

        hasRestedThisPhase = false;
    }

    public void ExitShopPhase()
    {
        merchantView.DeregisterShop();
        blacksmithView.DeregisterShop();
        merchant = null;
        blacksmith = null;

        shopMenuContainer.SetActive(false);
        merchantView.gameObject.SetActive(false);
        blacksmithView.gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        shopMenuContainer.SetActive(true);
        merchantView.gameObject.SetActive(false);
        blacksmithView.gameObject.SetActive(false);
    }

    public void OpenMerchant()
    {
        shopMenuContainer.SetActive(false);
        merchantView.gameObject.SetActive(true);
        blacksmithView.gameObject.SetActive(false);
    }

    public void OpenBlacksmith()
    {
        shopMenuContainer.SetActive(false);
        merchantView.gameObject.SetActive(false);
        blacksmithView.gameObject.SetActive(true);
    }

    public void OnRestClicked()
    {
        ServiceLocator.Global.Get(out gameManager);
        if (!hasRestedThisPhase)
        {
            if (gameManager.Player.TryRemoveGold(5))
            {
                gameManager.Player.Heal(10);
                hasRestedThisPhase = true;
            }

        }
    }
}