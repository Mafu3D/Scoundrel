using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradePackageView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int index;
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float hoverSpeed = 0.5f;

    [SerializeField] private Color notSelectedColor = Color.black;
    [SerializeField] private Color selectedColor = Color.red;
    [SerializeField] private SpriteRenderer backgroundSprite;

    [SerializeField] private TMP_Text upgradePackageNameTMP_Text;
    [SerializeField] private GameObject buffUpgradeViewPrefab;
    [SerializeField] private Transform buffUpgradeObjectContainer;

    public Action<UpgradePackage, int> OnClicked;

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
            newGameObject.transform.localPosition += new Vector3(0f, 0f, -1f);
            BuffUpgradeView buffUpgradeView = newGameObject.GetComponent<BuffUpgradeView>();
            buffUpgradeView.RegisterBuff(buff);
            instantiatedBuffUpgradeViewPrefabs.Add(buffUpgradeView);
            buffUpgradeView.OnClicked += OnMouseDown;
        }
    }

    public void DeregisterUpgradePackage()
    {
        foreach (BuffUpgradeView buffUpgradeView in instantiatedBuffUpgradeViewPrefabs)
        {
            buffUpgradeView.OnClicked -= OnMouseDown;
            Destroy(buffUpgradeView.gameObject);
        }
        upgradePackage = null;
        instantiatedBuffUpgradeViewPrefabs = new();
        upgradePackageNameTMP_Text.text = "";
    }

    public void OnMouseDown()
    {
        OnClicked?.Invoke(upgradePackage, index);
    }

    public void SetAsSelected(bool value)
    {
        backgroundSprite.color = value ? selectedColor : notSelectedColor;
    }
}
