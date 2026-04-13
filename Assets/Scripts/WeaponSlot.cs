using System;
using System.Collections;
using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] GameObject CardPrefab;
    [SerializeField] CardObject mainCardObject;
    [SerializeField] CardObject secondaryCardObject;

    Weapon weapon;

    void OnEnable()
    {
        mainCardObject.gameObject.SetActive(false);
        secondaryCardObject.gameObject.SetActive(false);
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
        mainCardObject.RegisterCard(weapon.Card);
        weapon.OnWeaponUpdated += OnWeaponUpdated;
        mainCardObject.gameObject.SetActive(true);
    }

    public void DeregisterWeapon()
    {
        if (weapon != null)
        {
            weapon.OnWeaponUpdated -= OnWeaponUpdated;
        }
        mainCardObject.gameObject.SetActive(false);
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
            secondaryCardObject.gameObject.SetActive(false);
        }
        else
        {
            secondaryCardObject.gameObject.SetActive(true);
            if (secondaryCardObject.Card != null)
            {
                secondaryCardObject.DeregisterCard();
            }
            secondaryCardObject.RegisterCard(weapon.SlainCards[^1]);
        }
    }
}
