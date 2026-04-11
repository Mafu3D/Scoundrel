using System;
using Project.Decks;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    [SerializeReference] CardObject cardObject;
    public bool IsActive { get; private set; } = false;

    public void AssignCard(Card card)
    {
        cardObject.AssignCard(card);
    }

    public void SetActive(bool value)
    {
        IsActive = value;
        cardObject.gameObject.SetActive(value);
    }
}