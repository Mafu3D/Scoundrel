using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradePackageCollection", menuName = "UpgradePackageCollection")]
public class UpgradePackageCollection : ScriptableObject
{
    [SerializeField] public List<UpgradePackage> StartingPackages;
    [SerializeField] public List<UpgradePackage> BasePackages;
}