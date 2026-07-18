using UnityEngine;

public class UpgradePackageTest_Scratch : MonoBehaviour
{
    [SerializeField] UpgradePackage upgradePackage;
    UpgradePackageView upgradePackageView;
    private void Awake()
    {
        upgradePackageView = GetComponent<UpgradePackageView>();
        upgradePackageView.RegisterUpgradePackage(upgradePackage);
    }
}