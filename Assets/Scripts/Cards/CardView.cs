using System;
using System.Collections;
using System.Collections.Generic;
using Project.Decks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public enum CardClickContext
{
    NONE,
    TOP,
    BOT
}

public partial class CardView : MonoBehaviour
{
    [SerializeField] public bool Clickable = true;

    [Header("Card Visuals")]
    [SerializeField] private List<TMP_Text> valueTMPTexts = new();
    [SerializeField] private SpriteRenderer suitSprite;

    [Header("Card Colors")]
    [SerializeField] private Color red;
    [SerializeField] private Color black;
    [SerializeField] private Color valueIncreasedColor;
    [SerializeField] private Color valueDecreasedColor;

    [Header("Suit Sprites")]
    [SerializeField] private Sprite diamondsSprite;
    [SerializeField] private Sprite heartsSprite;
    [SerializeField] private Sprite clubsSprite;
    [SerializeField] private Sprite spadesSprite;

    [Header("Hover")]
    [SerializeField] CardHoverContextView topHoverContext;
    [SerializeField] CardHoverContextView botHoverContext;
    [SerializeField] CardHoverContextView fullHoverContext;

    [Header("Hover Contexts Colors")]
    [SerializeField] private Color attackWeaponColor;
    [SerializeField] private Color attackUnarmedColor;
    [SerializeField] private Color equipColor;
    [SerializeField] private Color drinkColor;
    [SerializeField] private Color discardColor;


    [SerializeField] private GameManager gameManager;

    public bool IsActive { get; private set; } = false;
    public CardModel Card {get; private set; }
    public Action<CardModel, CardClickContext> OnCardClicked; // Not used?

    private BoxCollider2D myCollider;
    private SortingGroup mySortingGroup;
    private CardClickContext cardClickContext;
    private List<CardHoverContextView> allHoverContexts;

    void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        mySortingGroup = GetComponent<SortingGroup>();

        allHoverContexts = new() {topHoverContext, botHoverContext, fullHoverContext};
    }

    void Start()
    {
        HideAllHoverBoxes();
    }

    public void RegisterCard(CardModel card)
    {
        Card = card;
        if (card == null) return;
    }

    public void DeregisterCard()
    {
        Card = null;
    }

    public void SetSortingLayer(int layer)
    {
        mySortingGroup.sortingOrder = layer;
    }

    void OnMouseDown()
    {
        if (!Clickable)
        {
            return;
        }
        OnCardClicked?.Invoke(Card, cardClickContext);
        gameManager.OnCardClicked(Card, cardClickContext);
    }

    void Update()
    {
        UpdateView();

        if (Card == null || !Clickable)
        {
            return;
        }

        HandleMousePosition();
    }

    private void UpdateView()
    {
        if (Card == null)
        {
            return;
        }

        Color color = black;
        if (Card.Suit == Suit.HEARTS || Card.Suit == Suit.DIAMONDS)
        {
            color = red;
        }
        foreach (var TMPText in valueTMPTexts)
        {
            string valueString = Card.Value.ToString();
            if (Card.Value > 10)
            {
                if (Card.Value == 11)
                {
                    valueString = "J";
                }
                else if (Card.Value == 12)
                {
                    valueString = "Q";
                }
                else if (Card.Value == 13)
                {
                    valueString = "K";
                }
                else if (Card.Value == 14)
                {
                    valueString = "A";
                }
            }
            TMPText.text = valueString;

            Color textColor = color;
            if (Card.Value > Card.BaseValue)
            {
                textColor = valueIncreasedColor;
            }
            else if (Card.Value < Card.BaseValue)
            {
                textColor = valueDecreasedColor;
            }
            TMPText.color = textColor;
        }

        switch (Card.Suit)
        {
            case Suit.SPADES:
                suitSprite.sprite = spadesSprite;
                break;
            case Suit.CLUBS:
                suitSprite.sprite = clubsSprite;
                break;
            case Suit.HEARTS:
                suitSprite.sprite = heartsSprite;
                break;
            case Suit.DIAMONDS:
                suitSprite.sprite = diamondsSprite;
                break;
        }
    }

    private void HandleMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (myCollider.OverlapPoint(worldPos))
        {
            if (Card.Suit == Suit.SPADES || Card.Suit == Suit.CLUBS)
            {
                // Attack Weapon (Top)
                if (worldPos.y >= this.transform.position.y)
                {
                    cardClickContext = CardClickContext.TOP;
                    ShowHoverBox(topHoverContext, attackWeaponColor, "Weapon");
                }
                // Attack Unarmed (Bot)
                else
                {
                    cardClickContext = CardClickContext.BOT;
                    ShowHoverBox(botHoverContext, attackUnarmedColor, "Fist");
                }
            }
            else if (Card.Suit == Suit.HEARTS)
            {
                // Drink
                if (gameManager.Player.HasDrankPotionThisRoom || gameManager.Player.IsAtMaxHealth)
                {
                    cardClickContext = CardClickContext.TOP;
                    ShowHoverBox(fullHoverContext, discardColor, "Discard");
                }
                // Discard
                else
                {
                    cardClickContext = CardClickContext.TOP;
                    ShowHoverBox(fullHoverContext, drinkColor, "Drink");
                }
            }
            else if (Card.Suit == Suit.DIAMONDS)
            {
                // Equip
                cardClickContext = CardClickContext.NONE;
                ShowHoverBox(fullHoverContext, equipColor, "Equip");
            }
        }
        else
        {
            // None
            HideAllHoverBoxes();
        }
    }

    private void ShowHoverBox(CardHoverContextView boxToShow, Color bgColor, string text)
    {
        foreach (CardHoverContextView box in allHoverContexts)
        {
            if (box != boxToShow)
            {
                box.gameObject.SetActive(false);
                box.Clear();
                continue;
            }
            box.gameObject.SetActive(true);
            box.SetBGColor(bgColor);
            box.SetText(text);
        }
    }

    private void HideAllHoverBoxes()
    {
        foreach (CardHoverContextView box in allHoverContexts)
        {
            box.gameObject.SetActive(false);
            box.Clear();
        }
    }

    private void OnMouseEnterTop()
    {

    }

    private void OnMouseExitTop()
    {

    }

    private void OnMouseStayTop()
    {

    }

    private void OnMouseEnterBot()
    {

    }

    private void OnMouseExitBot()
    {

    }

    private void OnMouseStayBot()
    {

    }
}
