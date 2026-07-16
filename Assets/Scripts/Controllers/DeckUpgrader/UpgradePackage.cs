using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradePackage", menuName = "UpgradePackage", order = 0)]
public class UpgradePackage : ScriptableObject, IRarity
{
    [SerializeField] public string Name;
    [SerializeField] private Rarity rarity;
    [SerializeField] public List<Buff> Buffs;

    public Rarity Rarity => rarity;
}