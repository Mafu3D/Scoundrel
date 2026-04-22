using System;
using System.Collections;
using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] GameObject CardPrefab;
    [SerializeField] CardView weaponCard;
    [SerializeField] CardView lastDefeatedCard;

    Weapon weapon;

    void OnEnable()
    {
        weaponCard.gameObject.SetActive(false);
        lastDefeatedCard.gameObject.SetActive(false);
        Player.OnWeaponChanged += OnWeaponChanged;
    }

    void OnDisable()
    {
        Player.OnWeaponChanged -= OnWeaponChanged;
    }

    private void OnWeaponChanged()
    {
        if (Player.Weapon != null)
        {
            RegisterWeapon(Player.Weapon);
        }
        else
        {
            DeregisterWeapon();
        }
        OnWeaponUpdated();
    }

    public void RegisterWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weaponCard.RegisterCard(weapon.Card);
        weapon.OnWeaponUpdated += OnWeaponUpdated;
        weaponCard.gameObject.SetActive(true);
    }

    public void DeregisterWeapon()
    {
        if (weapon != null)
        {
            weapon.OnWeaponUpdated -= OnWeaponUpdated;
        }
        weaponCard.gameObject.SetActive(false);
        this.weapon = null;
    }

    private void OnWeaponUpdated()
    {
        if (weapon == null)
        {
            return;
        }
        if (weapon.SlainCards.Count == 0)
        {
            lastDefeatedCard.gameObject.SetActive(false);
        }
        else
        {
            lastDefeatedCard.gameObject.SetActive(true);
            if (lastDefeatedCard.Card != null)
            {
                lastDefeatedCard.DeregisterCard();
            }
            lastDefeatedCard.RegisterCard(weapon.SlainCards[^1]);
        }
    }
}
