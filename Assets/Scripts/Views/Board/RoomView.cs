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
        room.OnCardsChanged += OnCardsChanged;
        room = newRoom;
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
            if (room.GetCards()[i] != null)
            {
                CardViews[i].RegisterCard(room.GetCards()[i]);
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