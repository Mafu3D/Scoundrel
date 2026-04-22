using System;
using System.Collections;
using System.Collections.Generic;
using Project.Decks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum CardClickContext
{
    NONE,
    TOP,
    BOT
}

public class CardView : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> valueTMPTexts = new();
    [SerializeField] private TMP_Text suitTMPText;
    [SerializeField] private Color red;
    [SerializeField] private Color black;
    [SerializeField] private GameObject attackWeaponContainer;
    [SerializeField] private GameObject attackUnarmedContainer;
    [SerializeField] private GameObject drinkContainer;
    [SerializeField] private GameObject discardContainer;
    [SerializeField] private GameObject equipContainer;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool clickable = true;

    public bool IsActive { get; private set; } = false;
    public CardModel Card {get; private set; }
    public UnityEvent<CardModel, CardClickContext> OnCardClicked;

    private BoxCollider2D myCollider;
    private CardClickContext cardClickContext;

    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        attackWeaponContainer.SetActive(false);
        attackUnarmedContainer.SetActive(false);
        drinkContainer.SetActive(false);
        discardContainer.SetActive(false);
        equipContainer.SetActive(false);
    }

    public void RegisterCard(CardModel card)
    {
        Card = card;
        if (card == null) return;
        Color color = black;
        if (card.Suit == Suit.HEARTS || card.Suit == Suit.DIAMONDS)
        {
            color = red;
        }
        foreach (var TMPText in valueTMPTexts)
        {
            string valueString = card.Value.ToString();
            if (card.Value > 10)
            {
                if (card.Value == 11)
                {
                    valueString = "J";
                }
                else if (card.Value == 12)
                {
                    valueString = "Q";
                }
                else if (card.Value == 13)
                {
                    valueString = "K";
                }
                else if (card.Value == 14)
                {
                    valueString = "A";
                }
            }
            TMPText.text = valueString;
            TMPText.color = color;
        }

        suitTMPText.text = card.Suit.ToString();
        suitTMPText.color = color;
    }

    public void DeregisterCard()
    {
        Card = null;
    }

    void OnMouseDown()
    {
        if (!clickable)
        {
            return;
        }
        OnCardClicked?.Invoke(Card, cardClickContext);
        gameManager.OnCardClicked(Card, cardClickContext);
    }

    void Update()
    {
        if (Card == null || !clickable)
        {
            return;
        }

        Vector3 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (myCollider.OverlapPoint(worldPos))
        {
            if (Card.Suit == Suit.SPADES || Card.Suit == Suit.CLUBS)
            {
                if (worldPos.y >= this.transform.position.y)
                {
                    cardClickContext = CardClickContext.TOP;
                    attackWeaponContainer.SetActive(true);
                    attackUnarmedContainer.SetActive(false);
                    drinkContainer.SetActive(false);
                    discardContainer.SetActive(false);
                    equipContainer.SetActive(false);
                }
                else
                {
                    cardClickContext = CardClickContext.BOT;
                    attackWeaponContainer.SetActive(false);
                    attackUnarmedContainer.SetActive(true);
                    drinkContainer.SetActive(false);
                    discardContainer.SetActive(false);
                    equipContainer.SetActive(false);
                }
            }
            else if (Card.Suit == Suit.HEARTS)
            {
                if (gameManager.HasDrankPotionThisRoom)
                {
                    cardClickContext = CardClickContext.TOP;
                    attackWeaponContainer.SetActive(false);
                    attackUnarmedContainer.SetActive(false);
                    drinkContainer.SetActive(false);
                    discardContainer.SetActive(true);
                    equipContainer.SetActive(false);
                }
                else
                {
                    cardClickContext = CardClickContext.TOP;
                    attackWeaponContainer.SetActive(false);
                    attackUnarmedContainer.SetActive(false);
                    drinkContainer.SetActive(true);
                    discardContainer.SetActive(false);
                    equipContainer.SetActive(false);
                }
            }
            else if (Card.Suit == Suit.DIAMONDS)
            {
                cardClickContext = CardClickContext.NONE;
                attackWeaponContainer.SetActive(false);
                attackUnarmedContainer.SetActive(false);
                drinkContainer.SetActive(false);
                discardContainer.SetActive(false);
                equipContainer.SetActive(true);
            }
        }
        else
        {
            attackWeaponContainer.SetActive(false);
            attackUnarmedContainer.SetActive(false);
            drinkContainer.SetActive(false);
            discardContainer.SetActive(false);
            equipContainer.SetActive(false);
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
