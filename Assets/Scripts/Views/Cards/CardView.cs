using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mafu.Extensions;
using Project.Decks;
using Project.UI.Tooltips;
using Sirenix.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public enum MousePositionContext
{
    NONE,
    TOP,
    BOT,
    FULL
}

[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(BoxCollider2D))]
public class CardView : MonoBehaviour, ITooltipGettable, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Globals")]
    [SerializeField] public GlobalCardData GlobalCardData;

    [Header("Extender (Optional)")]
    [SerializeField] private CardViewExtender cardViewExtender;

    [Header("Components")]
    [SerializeField] private List<TMP_Text> valueTMPTexts = new();
    [SerializeField] private SpriteRenderer suitSpriteRenderer;
    [SerializeField] private Transform buffIconsTransform;

    [Header("Animation")]
    [SerializeField] private float onHoverGrowthSize = 1.1f;
    [SerializeField] private float onHoverGrowthSpeed = 0.1f;

    [HideInInspector] public bool Clickable = true;
    public bool IsActive { get; private set; } = false;
    public RuntimeCardModel Card {get; private set; }
    public Action<MousePositionContext> OnClicked;
    public Action OnMouseOver;
    public Action OnMouseExit;
    public Action<MousePositionContext> OnMouseStay;

    private Transform myTransform;
    private BoxCollider2D myCollider;
    private SortingGroup mySortingGroup;

    public void SetSortingLayer(int layer)
    {
        mySortingGroup.sortingOrder = layer;
    }

    #region Card Registration
    public void RegisterCard(RuntimeCardModel newCard)
    {
        if (Card != null)
        {
            Debug.LogError($"Card is already registered to card view : {this.name}. You must deregister the card first!");
            return;
        }

        Card = newCard;

        if (newCard == null)
        {
            Debug.LogWarning($"Card registered was null");
            return;
        }

        Card.OnUpdate += UpdateView;
    }

    public void DeregisterCard()
    {
        if (Card != null)
        {
            Card.OnUpdate -= UpdateView;
        }
        Card = null;
    }
    #endregion

    #region Mouse Handling
    public MousePositionContext GetMousePositionContext()
    {
        // This function only returns TOP or BOT (or maybe NONE but it really shouldn't). It should NEVER return
        //      FULL, as the implementer should handle that

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (myCollider.OverlapPoint(worldPos))
        {
            // Return TOP if the mouse pos above the mid point, otherwise return BOT
            return worldPos.y >= this.transform.position.y ? MousePositionContext.TOP : MousePositionContext.BOT;
        }
        // If the mouse pos isn't overlapping the collider, return NONE
        //  Theoretically, this should NEVER return NONE, since OnMouseDown won't be called if the player clicks outside of the collider
        return MousePositionContext.NONE;
    }

    private void OnMouseDown()
    {
        if (!Clickable || Card == null)
        {
            return;
        }

        MousePositionContext mousePositionContext = GetMousePositionContext();
        cardViewExtender?.OnClicked(mousePositionContext);
        OnClicked?.Invoke(mousePositionContext);

        myTransform.localScale = new Vector3(1f, 1f, 1f);
    }
    #endregion

    #region Unity Loop
    private void Awake()
    {
        myTransform = GetComponent<Transform>();
        myCollider = GetComponent<BoxCollider2D>();
        mySortingGroup = GetComponent<SortingGroup>();
    }

    private void Update()
    {
        if (Card == null || !Clickable)
        {
            return;
        }

        MousePositionContext mousePositionContext = GetMousePositionContext();
        if (mousePositionContext != MousePositionContext.NONE)
        {
            cardViewExtender?.OnMouseStay(mousePositionContext);
            OnMouseStay?.Invoke(mousePositionContext);
        }
    }
    #endregion

    #region Update Visuals
    private void UpdateView()
    {
        if (Card == null)
        {
            return;
        }

        UpdateText();
        UpdateSprite();
        UpdateBuffIcons();
    }

    private void UpdateText()
    {
        // Update color
        Color color = Card.Suit == Suit.HEARTS || Card.Suit == Suit.DIAMONDS ?  GlobalCardData.Red : GlobalCardData.Black;

        // Update text and text color for both texts
        foreach (var TMPText in valueTMPTexts)
        {
            TMPText.text = Card.GetCardValueStringSymbol();
            Color textColor = color;
            if (Card.Value > Card.BaseValue)
            {
                textColor = GlobalCardData.ValueIncreasedColor;
            }
            else if (Card.Value < Card.BaseValue)
            {
                textColor = GlobalCardData.ValueDecreasedColor;
            }
            TMPText.color = textColor;
        }
    }

    private void UpdateSprite() => suitSpriteRenderer.sprite = GlobalCardData.GetSuitSprite(Card.Suit);

    private void UpdateBuffIcons()
    {
        // Display buff icons
        List<Buff> visibleBuffs = Card.BuffManager.GetVisibleBuffs();
        int buffCount = visibleBuffs.Count;
        if (buffCount > 0 )
        {
            buffIconsTransform.gameObject.SetActive(true);
            for (int i = 0; i < buffIconsTransform.childCount; i++)
            {
                Transform child = buffIconsTransform.GetChild(i);
                if (i < buffCount)
                {
                    child.gameObject.SetActive(true);
                    child.GetComponent<SpriteRenderer>().sprite = visibleBuffs[i].Sprite;
                }
                else
                {
                    child.gameObject.SetActive(false);
                    child.GetComponent<SpriteRenderer>().sprite = null;
                }
            }
        }
        else
        {
            buffIconsTransform.gameObject.SetActive(false);
            foreach (Transform child in buffIconsTransform)
            {
                child.gameObject.SetActive(false);
                child.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }
    #endregion

    #region Tooltip
    public bool TryGetTooltipInformation(out TooltipCollection tooltipCollection)
    {
        if (Card == null)
        {
            tooltipCollection = new();
            Debug.LogWarning("Cannot get tooltip for null object");
            return false;
        }

        // Handle some special cases that need default content for now
        string content = "";
        if (Card.Suit == Suit.DOORS)
        {
            content = "Proceed to the next floor of the dungeon. Only unlocks once you have received the appropriate number of points";
        }
        else if (Card.Suit == Suit.TREASURES)
        {
            content = "Gain 3 gold";
        }
        else if (Card.Suit == Suit.HEARTS)
        {
            content = $"Restore {Card.Value} health (up to max). Can only heal once per room.";
        }
        else if (Card.Suit == Suit.DIAMONDS)
        {
            content = "Equip this as a weapon, replacing your currently equipped weapon (if any)";
        }
        else if (Card.Suit == Suit.SPADES || Card.Suit == Suit.CLUBS)
        {
            content = "Fight the monster with either you fist or weapon";
        }

        List<TooltipData> tooltipDatas = new();
        TooltipData baseTooltip = new(Card.GetCardInfoString(), Card.CardType.ToString().ToFirstUppercase(), content, null);
        tooltipDatas.Add(baseTooltip);

        foreach (Buff buff in Card.BuffManager.GetBuffs())
        {
            if (buff.IsHidden)
            {
                continue;
            }
            TooltipData buffTooltip = new(buff.Name, "" , buff.Description, buff.Sprite);
            tooltipDatas.Add(buffTooltip);
        }

        tooltipCollection = new(tooltipDatas);

        // foreach (int value in Card.ValueModifiers)
        // {
        //     content += $"\n{value}";
        // }
        return true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myTransform.DOScale(onHoverGrowthSize, onHoverGrowthSpeed);
        cardViewExtender?.OnMouseEnter();
        OnMouseOver?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myTransform.DOScale(1f, onHoverGrowthSpeed);
        cardViewExtender?.OnMouseExit();
        OnMouseExit?.Invoke();
    }
    #endregion
}
