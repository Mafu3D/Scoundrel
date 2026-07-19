using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckUpgradeChoice
{
    readonly public List<UpgradePackage> UpgradePackages;
    readonly public DeckUpgrader DeckUpgrader;
    public UpgradePackage SelectedPackage;
    public bool ChoiceSelected => SelectedPackage != null;
    public bool ChoiceFinished = false;
    public Action OnChoiceFinished;

    public DeckUpgradeChoice(DeckUpgrader deckUpgrader, int amount)
    {
        DeckUpgrader = deckUpgrader;
        UpgradePackages = deckUpgrader.GetRandomUniqueUpgradePackages(amount);
    }

    public void SelectPackage(int index)
    {
        SelectedPackage = UpgradePackages[index];
    }

    public void SelectPackage(UpgradePackage upgradePackage)
    {
        if (!UpgradePackages.Contains(upgradePackage))
        {
            Debug.LogError($"This choice does not contain {upgradePackage}");
            return;
        }
        SelectedPackage = upgradePackage;
    }

    public void FinishAndApply()
    {
        DeckUpgrader.UpgradeMonsterDeckFromPackage(SelectedPackage);
        ChoiceFinished = true;
        OnChoiceFinished?.Invoke();
    }
}
