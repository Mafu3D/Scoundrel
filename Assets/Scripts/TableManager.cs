using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using TMPro;
using System;
using Unity.VisualScripting;

public class TableManager : MonoBehaviour
{
    [SerializeField] List<CardView> Cards;
    [SerializeField] GameObject CardPrefab;
    [SerializeField] TMP_Text remaining_TMPText;
    [SerializeField] TMP_Text round_TMPText;
    [SerializeField] TMP_Text health_TMPText;

    [SerializeField] private List<GameObject> showDuringGame = new();
    [SerializeField] private List<GameObject> showDuringGameOver = new();

    [SerializeField] private TMP_Text endMessage_TMPText;
    [SerializeField] private TMP_Text score_TMPText;

    [SerializeField] GameManager GameManager;
    [SerializeField] DeckManager DeckManager;
    [SerializeField] Player Player;

    void OnEnable()
    {
        GameManager.OnStartNewGame += OnStartNewGame;
        GameManager.OnEnterNewRoom += OnEnterNewRoom;
        GameManager.OnCardsChanged += OnCardsChanged;
        GameManager.OnGameOver += OnGameOver;
        Player.OnHealthChanged += OnPlayerHealthChanged;
        DeckManager.OnCardDraw += OnCardDraw;
    }

    void OnDisable()
    {
        GameManager.OnStartNewGame -= OnStartNewGame;
        GameManager.OnEnterNewRoom -= OnEnterNewRoom;
        GameManager.OnCardsChanged -= OnCardsChanged;
        GameManager.OnGameOver -= OnGameOver;
        Player.OnHealthChanged -= OnPlayerHealthChanged;
        DeckManager.OnCardDraw -= OnCardDraw;
    }


    private void OnPlayerHealthChanged(int amount)
    {
        health_TMPText.text = $"{amount} / {Player.MaxHealth}";
    }

    private void OnCardsChanged()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            bool setActive = GameManager.CurrentRoom.Cards[i] != null;
            Cards[i].gameObject.SetActive(setActive);
        }
    }

    private void Start()
    {
        foreach (var cardSlot in Cards)
        {
            cardSlot.gameObject.SetActive(false);
        }
        foreach (var item in showDuringGameOver)
        {
            item.SetActive(false);
        }
        foreach (var item in showDuringGame)
        {
            item.SetActive(false);
        }
    }

    private void OnStartNewGame()
    {
        foreach (var item in showDuringGameOver)
        {
            item.SetActive(false);
        }
        foreach (var item in showDuringGame)
        {
            item.SetActive(true);
        }
    }

    private void OnEnterNewRoom()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            if (GameManager.CurrentRoom.Cards[i] != null)
            {
                Cards[i].gameObject.SetActive(true);
            }
            Cards[i].RegisterCard(GameManager.CurrentRoom.Cards[i]);
        }

        round_TMPText.text = "Round: " + GameManager.RoomNumber.ToString();
    }

    private void OnCardDraw()
    {
        remaining_TMPText.text = $"{DeckManager.Deck.CurrentCount} / {DeckManager.Deck.TotalCount}";
    }

    private void OnGameOver()
    {
        foreach (var item in showDuringGameOver)
        {
            item.SetActive(true);
        }
        foreach (var item in showDuringGame)
        {
            item.SetActive(false);
        }

        int monsterScore = 0;
        foreach (CardModel card in DeckManager.Deck.RemainingItems)
        {
            if (card.Suit == Suit.SPADES || card.Suit == Suit.CLUBS)
            {
                monsterScore += card.Value;
            }
        }
        foreach (CardModel card in GameManager.CurrentRoom.Cards)
        {
            if (card != null && (card.Suit == Suit.SPADES || card.Suit == Suit.CLUBS))
            {
                monsterScore += card.Value;
            }
        }

        if (DeckManager.Deck.CurrentCount == 0 && monsterScore == 0)
        {
            endMessage_TMPText.text = "You Win!";
        }
        else
        {
            endMessage_TMPText.text = "You Lose!";
        }

        score_TMPText.text = GameManager.GetScore().ToString();
    }
}