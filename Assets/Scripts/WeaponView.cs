using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] GameObject CardViewPrefab;
    [SerializeField] CardView weaponCard;
    [SerializeField] List<CardView> defeatedCardsPool;
    [SerializeField] float defeatedCardsOffset = -0.755f;

    WeaponModel weapon;

    void Start()
    {
        RefreshView();
    }

    public void OnStartNewGame()
    {
        RefreshView();
    }

    public void RegisterWeapon(WeaponModel weapon)
    {
        this.weapon = weapon;
        weaponCard.RegisterCard(weapon.Card);
        weapon.OnWeaponUpdate += OnWeaponUpdate;
        weaponCard.gameObject.SetActive(true);

        RefreshView();
    }

    public void DeregisterWeapon()
    {
        if (weapon != null)
        {
            weapon.OnWeaponUpdate -= OnWeaponUpdate;
        }
        this.weapon = null;

        RefreshView();
    }

    private void RefreshView()
    {
        if (weapon == null)
        {
            weaponCard.gameObject.SetActive(false);
            foreach (CardView card in defeatedCardsPool)
            {
                if (card.Card != null)
                {
                    card.DeregisterCard();
                }
                card.gameObject.SetActive(false);
            }
            return;
        }

        // If a CardView has not already been created in the object pool, create a new one
        int difference = weapon.SlainCards.Count - defeatedCardsPool.Count;
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
            if (i < weapon.SlainCards.Count)
            {
                card.RegisterCard(weapon.SlainCards[i]);
                card.gameObject.SetActive(true);
                card.transform.localPosition = new Vector3(defeatedCardsOffset*(i+1), 0, 0);
            }
            else
            {
                if (card.Card != null)
                {
                    card.DeregisterCard();
                }
                card.gameObject.SetActive(false);
            }
        }
    }

    private void OnWeaponUpdate()
    {
        RefreshView();
    }
}
