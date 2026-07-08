using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using System;
using System.Linq;
using Sirenix.Utilities;

public class RoomModel
{
    public int Size = 4;
    public RoomSlot[] Slots;

    public RoomModel(int roomSize, List<RoomSlot> contents)
    {
        Slots = new RoomSlot[roomSize];

        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i] = contents[i];
        }
    }
}
