using System.Collections.Generic;
using UnityEngine;

public class RoomView : MonoBehaviour
{
    [SerializeField] public List<CardView> CardViews;

    private RoomModel room;

    void Start()
    {
        RefreshView();
    }

    public void OnStartNewGame()
    {
        RefreshView();
    }

    public void RegisterRoom(RoomModel newRoom)
    {
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
                CardViews[i].gameObject.SetActive(false);
                CardViews[i].DeregisterCard();
            }

            return;
        }

        // Otherwise, register the card model to each card view
        for (int i = 0; i < CardViews.Count; i++)
        {
            if (room.Cards[i] != null)
            {
                CardViews[i].RegisterCard(room.Cards[i]);
                CardViews[i].gameObject.SetActive(true);
            }
            else
            {
                CardViews[i].gameObject.SetActive(false);
                CardViews[i].DeregisterCard();
            }
        }
    }
}