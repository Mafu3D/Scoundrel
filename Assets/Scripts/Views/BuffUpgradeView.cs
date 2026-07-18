using System;
using DG.Tweening;
using Project.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuffUpgradeView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ITooltipGettable
{
    [SerializeField] private SpriteRenderer upgradeIconSpriteRenderer;
    [SerializeField] private TMP_Text upgradeNameTMP_Text;
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float hoverSpeed = 0.5f;

    private Vector3 startingScale;
    private Buff buff;

    private void Awake() {
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

    public void RegisterBuff(Buff buff)
    {
        DeregisterBuff();
        this.buff = buff;
        upgradeIconSpriteRenderer.sprite = buff.Sprite;
        upgradeNameTMP_Text.text = buff.Name;
    }

    public void DeregisterBuff()
    {
        this.buff = null;
        upgradeIconSpriteRenderer.sprite = null;
        upgradeNameTMP_Text.text = "";
    }

    public bool TryGetTooltipInformation(out TooltipCollection tooltipCollection)
    {
        TooltipData tooltipData = new(header: buff.Name,
                                      subtitle: "",
                                      content: buff.Description,
                                      image: buff.Sprite);
        tooltipCollection = new(new() {tooltipData});
        return true;
    }
}
