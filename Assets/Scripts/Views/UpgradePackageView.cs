using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradePackageView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float hoverSpeed = 0.5f;

    [SerializeField] private TMP_Text upgradePackageNameTMP_Text;
    [SerializeField] private GameObject buffUpgradeViewPrefab;
    [SerializeField] private Transform buffUpgradeObjectContainer;

    private UpgradePackage upgradePackage;
    private List<BuffUpgradeView> instantiatedBuffUpgradeViewPrefabs = new();
    private Vector3 startingScale;

    private void Awake()
    {
        startingScale = this.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.DOScale(startingScale * hoverScale, hoverSpeed);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.DOScale(startingScale, hoverSpeed);
    }

    public void RegisterUpgradePackage(UpgradePackage upgradePackage)
    {
        DeregisterUpgradePackage();

        this.upgradePackage = upgradePackage;
        upgradePackageNameTMP_Text.text = upgradePackage.Name;

        foreach(Buff buff in upgradePackage.Buffs)
        {
            GameObject newGameObject = Instantiate(buffUpgradeViewPrefab, buffUpgradeObjectContainer);
            BuffUpgradeView buffUpgradeView = newGameObject.GetComponent<BuffUpgradeView>();
            buffUpgradeView.RegisterBuff(buff);
            instantiatedBuffUpgradeViewPrefabs.Add(buffUpgradeView);
        }
    }

    public void DeregisterUpgradePackage()
    {
        foreach (BuffUpgradeView buffUpgradeView in instantiatedBuffUpgradeViewPrefabs)
        {
            Destroy(buffUpgradeView.gameObject);
        }
        upgradePackage = null;
        instantiatedBuffUpgradeViewPrefabs = new();
        upgradePackageNameTMP_Text.text = "";
    }
}
