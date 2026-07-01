using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using System;
using System.Linq;

public class RoomModel
{
    public int Size = 4;
    public RuntimeCardModel[] Cards;

    public RoomModel(int roomSize, List<RuntimeCardModel> cards)
    {
        Cards = new RuntimeCardModel[roomSize];

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = cards[i];
        }
    }
}
