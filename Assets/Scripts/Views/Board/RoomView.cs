using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

public class RoomView : MonoBehaviour
{
    [SerializeField] List<RoomSlotView> roomSlotViews;

    private RoomController room;

    // void Start()
    // {
    //     RefreshView();
    // }

    public void OnStartNewGame()
    {
        // RefreshView();
    }

    public void RegisterRoom(RoomController newRoom)
    {
        if (room != null)
        {
            Debug.LogWarning("RoomView is already registered to a room. Deregistering the current room.");
            DeregisterRoom();
        }
        room = newRoom;

        for (int i = 0; i < roomSlotViews.Count; i++)
        {
            RoomSlotView roomSlotView = roomSlotViews[i];
            roomSlotView.RegisterRoomSlot(newRoom.Slots[i]);
        }

        // room.OnCardsChanged += OnCardsChanged;
        // RefreshView();
    }

    public void DeregisterRoom()
    {
        if (room != null)
        {
            // room.OnCardsChanged -= OnCardsChanged;
        }
        room = null;

        foreach(RoomSlotView roomSlotView in roomSlotViews)
        {
            roomSlotView.DeregisterRoomSlot();
        }

        // RefreshView();
    }

    // private void OnCardsChanged()
    // {
    //     RefreshView();
    // }

    // private void RefreshView()
    // {

    // }
}
