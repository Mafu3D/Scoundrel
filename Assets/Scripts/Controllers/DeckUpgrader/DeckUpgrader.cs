using System.Collections.Generic;
using Mafu.Extensions;
using Project.Decks;
using UnityEngine;

public class DeckUpgrader
{
    private readonly UpgradePackageCollection upgradePackages;
    private readonly DeckController deckController;

    public DeckUpgrader(DeckController deckController, UpgradePackageCollection upgradePackages)
    {
        this.deckController = deckController;
        this.upgradePackages = upgradePackages;
    }

    public void UpgradeMonsterDeckRandomly(int min, int max, GlobalBuffRegistry buffRegistry)
    {
        string outputString = "Random monster buffs:\n";

        int amount = UnityEngine.Random.Range(min, max);
        List<RuntimeCardModel> monsterCards = deckController.GetRemainingOfType(CardType.MONSTER);
        List<RuntimeCardModel> cardsToBuff = monsterCards.GetRandomUniqueElements(amount);

        foreach (RuntimeCardModel card in cardsToBuff)
        {
            Buff randomBuff = buffRegistry.GetRandomBuff(CardType.MONSTER);
            card.AddNewBuff(randomBuff);
            outputString += $"{card} >>> {randomBuff}\n";
        }
        Debug.Log(outputString);
    }

    public void UpgradeMonsterDeckFromPackage(UpgradePackage upgradePackage)
    {
        string outputString = $"Upgrading deck with package '{upgradePackage}':\n";

        int amount = upgradePackage.Buffs.Count;
        List<RuntimeCardModel> monsterCards = deckController.GetRemainingOfType(CardType.MONSTER);
        List<RuntimeCardModel> cardsToBuff = monsterCards.GetRandomUniqueElements(amount);

        for (int i = 0; i < cardsToBuff.Count; i++)
        {
            RuntimeCardModel card = cardsToBuff[i];
            Buff buff = upgradePackage.Buffs[i];
            card.AddNewBuff(buff);
            outputString += $"{card} >>> {buff}\n";
        }
        Debug.Log(outputString);
    }

    public List<UpgradePackage> GetRandomUniqueUpgradePackages(int amount, Dictionary<Rarity, int> weights)
    {
        Rarity rarity = default;
        System.Random random = new();
        return rarity.GetUniqueRandom(upgradePackages.UpgradePackages, amount, weights, random);
    }
}
