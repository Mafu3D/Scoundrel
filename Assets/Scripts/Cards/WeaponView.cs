using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using UnityEngine;

public class WeaponCardView : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] CardView cardView;
    [SerializeField] GameObject CardViewPrefab;
    [SerializeField] List<CardView> defeatedCardsPool;
    [SerializeField] float cardOffset = -0.755f;

    private WeaponCardModel registeredWeapon;

    void Start()
    {
        RefreshView();
    }

    public void OnStartNewGame()
    {
        RefreshView();
    }

    public void RegisterWeapon(WeaponCardModel newWeapon)
    {
        this.registeredWeapon = newWeapon;
        cardView.RegisterCard(newWeapon);
        newWeapon.OnWeaponUpdate += OnWeaponUpdate;
        RefreshView();
        this.gameObject.SetActive(true);
    }

    public void DeregisterWeapon()
    {
        this.gameObject.SetActive(false);
        if (registeredWeapon != null)
        {
            registeredWeapon.OnWeaponUpdate -= OnWeaponUpdate;
        }
        this.registeredWeapon = null;
        cardView.DeregisterCard();
        RefreshView();
    }

    private void OnWeaponUpdate()
    {
        RefreshView();
    }

    private void RefreshView()
    {
        if (registeredWeapon == null)
        {
            foreach (CardView card in defeatedCardsPool)
            {
                if (card.Card != null)
                {
                    card.DeregisterCard();
                }
                card.gameObject.SetActive(false);
            }
            this.gameObject.SetActive(false);
            return;
        }

        // If a CardView has not already been created in the object pool, create a new one
        int difference = registeredWeapon.SlainCards.Count - defeatedCardsPool.Count;
        if (difference > 0)
        {
            for (int i = 0; i < difference; i++)
            {
                GameObject newCardGO = Instantiate(CardViewPrefab);
                CardView newCardView = newCardGO.GetComponent<CardView>();
                newCardGO.transform.SetParent(this.transform);
                newCardGO.transform.localPosition = this.transform.position;
                newCardView.Clickable = false;
                defeatedCardsPool.Add(newCardView);
                newCardView.SetSortingLayer((defeatedCardsPool.Count + 1) * -1);
            }
        }

        // Iterate through the pool - turn on & register views that have a slain card
        for (int i = 0; i < defeatedCardsPool.Count; i++)
        {
            CardView card = defeatedCardsPool[i];
            if (card.Card != null)
            {
                card.DeregisterCard();
            }

            if (i < registeredWeapon.SlainCards.Count)
            {
                card.RegisterCard(registeredWeapon.SlainCards[i]);
                card.gameObject.SetActive(true);
                card.transform.localPosition = new Vector3(cardOffset*(i+1), 0, 0);
            }
            else
            {
                card.gameObject.SetActive(false);
            }
        }

        // Offset the cards
        for (int i = 0; i < registeredWeapon.SlainCards.Count + 1; i++)
        {
            CardView card = i == 0 ? cardView : defeatedCardsPool[i-1];
            float midPoint = (registeredWeapon.SlainCards.Count + 1) / 2;
            if (i < midPoint)
            {
                // Positive
                int step = (int)Math.Floor(midPoint - i);
                card.transform.localPosition = Vector3.zero + new Vector3(cardOffset * step, 0f, 0f);
            }
            else
            {
                // Negative
                int step = (int)Math.Ceiling(i - midPoint);
                card.transform.localPosition = Vector3.zero + new Vector3(cardOffset * -step, 0f, 0f);
            }
        }
    }

}
