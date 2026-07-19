using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using UnityEngine;

public class DeckUpgradeChoiceView : MonoBehaviour
{
    [SerializeField] List<UpgradePackageView> upgradePackageViews;
    private DeckUpgradeChoice deckUpgradeChoice;

    public bool CanProceed
     {
        get
        {
            if (deckUpgradeChoice == null || !deckUpgradeChoice.ChoiceSelected)
            {
                return false;
            }
            return true;
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Proceed()
    {
        if (!CanProceed)
        {
            Debug.LogWarning("Can't proceed!");
            return;
        }

        deckUpgradeChoice.FinishAndApply();

        ServiceLocator.Global.Get(out GameManager gameManager);
        gameManager.GoToShopState();
    }

    public void RegisterDeckUpgradeChoice(DeckUpgradeChoice deckUpgradeChoice)
    {
        DeregisterDeckUpgradeChoice();
        this.deckUpgradeChoice = deckUpgradeChoice;
        for (int i = 0; i < upgradePackageViews.Count; i++)
        {
            UpgradePackageView upgradePackageView = upgradePackageViews[i];
            upgradePackageView.RegisterUpgradePackage(deckUpgradeChoice.UpgradePackages[i]);
            upgradePackageView.gameObject.SetActive(true);
            upgradePackageView.OnClicked += SelectPackage;
        }
    }

    public void DeregisterDeckUpgradeChoice()
    {
        foreach(UpgradePackageView upgradePackageView in upgradePackageViews)
        {
            upgradePackageView.gameObject.SetActive(false);
            upgradePackageView.DeregisterUpgradePackage();
            upgradePackageView.OnClicked -= SelectPackage;
        }
    }

    private void SelectPackage(UpgradePackage upgradePackage)
    {
        deckUpgradeChoice.SelectPackage(upgradePackage);
    }

}
