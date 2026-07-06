using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopMenuContainer;
    [SerializeField] private ShopView merchantView;
    [SerializeField] private BlacksmithView blacksmithView;
    [SerializeField] private List<GameObject> otherBlacksmithObjects;

    private GameManager gameManager;
    private Merchant merchant;
    private Blacksmith blacksmith;

    private bool hasRestedThisPhase = false;

    // private void Awake()
    // {
    //     shopMenuContainer.SetActive(false);
    //     merchantView.gameObject.SetActive(false);
    //     blacksmithView.gameObject.SetActive(false);
    //     otherBlacksmithObjects.ForEach(go => go.SetActive(false));
    // }

    void Update()
    {
        blacksmith?.Update();
    }

    public void StartNewShopPhase()
    {
        ServiceLocator.Global.Get(out gameManager);

        // Create the traveling merchant
        merchant = new (gameManager);
        merchantView.RegisterShop(merchant);

        // Create the blacksmith
        blacksmith = new (gameManager.Player, gameManager.DeckController, gameManager.BuffRegistry, 4);
        blacksmithView.RegisterBlacksmithInstance(blacksmith);

        shopMenuContainer.SetActive(true);

        hasRestedThisPhase = false;
    }

    public void ExitShopPhase()
    {
        merchantView.DeregisterShop();
        blacksmithView.DeregisterBlacksmithInstance();
        merchant = null;
        blacksmith = null;

        shopMenuContainer.SetActive(false);
        merchantView.gameObject.SetActive(false);
        blacksmithView.Hide();
        otherBlacksmithObjects.ForEach(go => go.SetActive(false));
    }

    public void BackToMenu()
    {
        shopMenuContainer.SetActive(true);
        merchantView.gameObject.SetActive(false);
        blacksmithView.Hide();
        otherBlacksmithObjects.ForEach(go => go.SetActive(false));
    }

    public void OpenMerchant()
    {
        shopMenuContainer.SetActive(false);
        merchantView.gameObject.SetActive(true);
        blacksmithView.Hide();
        otherBlacksmithObjects.ForEach(go => go.SetActive(false));
    }

    public void OpenBlacksmith()
    {
        shopMenuContainer.SetActive(false);
        merchantView.gameObject.SetActive(false);
        blacksmithView.Show();
        otherBlacksmithObjects.ForEach(go => go.SetActive(true));
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