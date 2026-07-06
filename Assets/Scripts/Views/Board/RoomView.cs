using System.Collections.Generic;
using UnityEngine;

public class RoomView : MonoBehaviour
{
    [SerializeField] public List<CardView> CardViews;

    private RoomController room;

    void Start()
    {
        RefreshView();
    }

    public void OnStartNewGame()
    {
        RefreshView();
    }

    public void RegisterRoom(RoomController newRoom)
    {
        if (room != null)
        {
            Debug.LogWarning("RoomView is already registered to a room. Deregistering the current room.");
            DeregisterRoom();
        }
        room = newRoom;
        room.OnCardsChanged += OnCardsChanged;
        RefreshView();
    }

    public void DeregisterRoom()
    {
        if (room != null)
        {
            room.OnCardsChanged -= OnCardsChanged;
        }
        room = null;

        RefreshView();
    }

    private void OnCardsChanged()
    {
        RefreshView();
    }

    private void RefreshView()
    {
        // Disable all cards if no room
        if (room == null)
        {
            for (int i = 0; i < CardViews.Count; i++)
            {
                CardView cardView = CardViews[i];
                cardView.gameObject.SetActive(false);
                cardView.DeregisterCard();
            }

            return;
        }

        // Otherwise, register the card model to each card view
        for (int i = 0; i < CardViews.Count; i++)
        {
            CardView cardView = CardViews[i];
            if (cardView.Card != null)
            {
                cardView.DeregisterCard();
            }

            if (room.GetCards()[i] != null)
            {
                cardView.RegisterCard(room.GetCards()[i]);
                cardView.gameObject.SetActive(true);
            }
            else
            {
                cardView.gameObject.SetActive(false);
            }
        }
    }
}