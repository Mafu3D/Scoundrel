using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using TMPro;
using System;
using Unity.VisualScripting;

public class TableManager : MonoBehaviour
{
    [SerializeField] List<CardSlot> CardSlots;
    [SerializeField] GameObject CardPrefab;
    [SerializeField] TMP_Text remaining_TMPText;
    [SerializeField] TMP_Text round_TMPText;
    [SerializeField] TMP_Text health_TMPText;

    [SerializeField] GameManager GameManager;
    [SerializeField] DeckManager DeckManager;
    [SerializeField] Player Player;

    void OnEnable()
    {
        GameManager.OnStartNewGame += OnStartNewGame;
        GameManager.OnEnterNewRoom += OnEnterNewRoom;
        GameManager.OnCardsChanged += OnCardsChanged;
        Player.OnHealthChanged += OnPlayerHealthChanged;
        DeckManager.OnCardDraw += OnCardDraw;
    }

    void OnDisable()
    {
        GameManager.OnStartNewGame -= OnStartNewGame;
        GameManager.OnEnterNewRoom -= OnEnterNewRoom;
        GameManager.OnCardsChanged -= OnCardsChanged;
        Player.OnHealthChanged -= OnPlayerHealthChanged;
        DeckManager.OnCardDraw -= OnCardDraw;
    }

    private void OnPlayerHealthChanged(int amount)
    {
        health_TMPText.text = $"{amount} / {Player.MaxHealth}";
    }

    private void OnCardsChanged()
    {
        for (int i = 0; i < CardSlots.Count; i++)
        {
            bool setActive = GameManager.CurrentRoom.Cards[i] != null;
            CardSlots[i].SetActive(setActive);
        }
    }

    private void Start()
    {
        foreach (var cardSlot in CardSlots)
        {
            cardSlot.SetActive(false);
        }
    }

    private void OnStartNewGame()
    {
    }

    private void OnEnterNewRoom()
    {
        for (int i = 0; i < CardSlots.Count; i++)
        {
            if (GameManager.CurrentRoom.Cards[i] != null)
            {
                CardSlots[i].SetActive(true);
            }
            CardSlots[i].AssignCard(GameManager.CurrentRoom.Cards[i]);
        }

        round_TMPText.text = "Round: " + GameManager.RoomNumber.ToString();
    }

    private void OnCardDraw()
    {
        remaining_TMPText.text = $"{DeckManager.Deck.CurrentCount} / {DeckManager.Deck.TotalCount}";
    }

}