using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

public class NextCardsView : MonoBehaviour
{
    [SerializeField] List<CardView> nextCardViews;

    void Awake()
    {
        foreach(CardView cardView in nextCardViews)
        {
            cardView.gameObject.SetActive(false);
        }
    }

    public void OnStartNewGame()
    {
        DeregisterNextCards();
    }

    public void RegisterNextCards(List<RuntimeCardModel> nextCards)
    {
        Debug.Log("registering");
        for (int i = 0; i < nextCardViews.Count; i++)
        {
            CardView cardView = nextCardViews[i];
            cardView.DeregisterCard();
            if (i < nextCards.Count)
            {
                cardView.RegisterCard(nextCards[i]);
                cardView.gameObject.SetActive(true);
                continue;
            }
            cardView.gameObject.SetActive(false);
        }
    }

    public void DeregisterNextCards()
    {
        Debug.Log("deregistering");
        foreach(CardView cardView in nextCardViews)
        {
            cardView.DeregisterCard();
            cardView.gameObject.SetActive(false);
        }
    }
}