using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Project.Decks;
using Sirenix.Utilities;
using UnityEngine;

public class DungeonController
{
    public RoomController CurrentRoom { get; private set; }

    public bool CanGoToNextRoom => CurrentRoom != null &&
                                   CurrentRoom.CanGoToNextRoom();

    public Action OnGoToNextFloor;
    public Action OnNewRoomOpened;
    public Action OnExitCurrentFloor;

    private readonly DungeonModel dungeonModel;
    private DeckController deckController;

    public DungeonController()
    {
        this.dungeonModel = new DungeonModel();;

    }

    #region Getters & Setters
    public int GetRoomNumber() => dungeonModel.RoomNumber;

    public int GetFloorNumber() => dungeonModel.FloorNumber;

    public void RegisterDeckController(DeckController deckController)
    {
        this.deckController = deckController;
    }
    #endregion

    #region Game Loop
    public void Update()
    {
        CurrentRoom?.RunCardsOnUpdate();
    }

    public void StartNewDungeon()
    {
        CurrentRoom?.ClearCards();
        ResetRoomNumber();
        ResetFloorNumber();
        deckController.TEMP_CreateNewDeck();
    }

    public void EnterNewFloor()
    {
        IncrementFloorNumber();
        ResetRoomNumber();
        OnGoToNextFloor?.Invoke();
    }

    public void ExitCurrentFloor()
    {
        CurrentRoom.ClearCards();
        deckController.ResetDeck();
        foreach(RuntimeCardModel cardModel in deckController.Deck.AllItems)
        {
            cardModel.BuffManager.HandleOnExitFloor();
        }
        OnExitCurrentFloor?.Invoke();
    }

    public void OpenNewRoom()
    {
        int roomSize = 4; // TODO: Magic number, get from the floor model

        if (CurrentRoom != null)
        {
            CurrentRoom.OnCardsChanged -= OnCurrentRoomCardsChanged;
        }

        // Create the new room controller with the new room contents
        CurrentRoom = RoomFactory.CreateNew(roomSize, deckController, CurrentRoom);
        CurrentRoom.OnCardsChanged += OnCurrentRoomCardsChanged;

        // Run all of the on drawn events for the cards in the room
        CurrentRoom.GetAllCards().ForEach(card => card?.HandleOnDraw());

        IncrementRoomNumber();
        OnNewRoomOpened?.Invoke();
    }

    public void RunFromRoom()
    {
        foreach(RuntimeCardModel card in CurrentRoom.GetAllCards())
        {
            card?.BuffManager.CleanupTemporaryBuffs();
        }

        List<RuntimeCardModel> nonpersistantCards = CurrentRoom.PopNonPersistantCards();
        deckController.Deck.AddToRemaining(nonpersistantCards, addToTop: false, shuffle: false);
    }
    #endregion

    private void OnCurrentRoomCardsChanged()
    {
        foreach(RuntimeCardModel card in CurrentRoom.RemainingCards())
        {
            card.BuffManager.HandleOnCardsChanged();
        }
    }

    private void IncrementRoomNumber() => dungeonModel.RoomNumber++;
    private void ResetRoomNumber() => dungeonModel.RoomNumber = 0;

    private void IncrementFloorNumber() => dungeonModel.FloorNumber++;
    private void ResetFloorNumber() => dungeonModel.FloorNumber = 0;
}

public static class RoomFactory
{
    private static readonly ReadOnlyCollection<CardType> VALID_ACTIVE_CARD_TYPES = new( new List<CardType> { CardType.MONSTER, CardType.WEAPON, CardType.POTION} );

    public static RoomController CreateNew(int roomSize, DeckController deckController, RoomController existingRoom=null)
    {
        // Get any slots that need to carry over to the next room
        List<RoomSlot> remainingPopulatedSlots = new();
        if (existingRoom != null)
        {
            // Shuffle in any doors that still remain across all slots
            // List<RuntimeCardModel> doorCards = existingRoom.PopDoorCards();
            List<RuntimeCardModel> doorCards = new ();
            foreach(RoomSlot slot in existingRoom.Slots)
            {
                RuntimeCardModel activeCard = slot.ActiveCard;
                if (activeCard != null && activeCard is DoorCardModel)
                {
                    existingRoom.TryRemoveCard(activeCard);
                    doorCards.Add(activeCard);
                }
            }
            deckController.Deck.ShuffleIn(doorCards);

            // Add the remaining slots from the current room to the new room, shifting them
            remainingPopulatedSlots.AddRange(GetRemainingPopulatedSlotsFromRoom(existingRoom));
        }

        // Get the amount of new slots to create
        int amountOfSlotsToCreate = roomSize - remainingPopulatedSlots.Count;

        // Draw a pool of new cards
        List<RuntimeCardModel> drawnCards = deckController.DrawUntil(amountOfSlotsToCreate,
                                                                     (card) => VALID_ACTIVE_CARD_TYPES.Contains(card.CardType),
                                                                     out List<RuntimeCardModel> validCards,
                                                                     out List<RuntimeCardModel> invalidCards);

        // Create new slots with each valid as that slot's active card
        List<RoomSlot> newSlots = validCards.Select(card => new RoomSlot(new () { card })).ToList();

        // Divide the remaining invalid cards amongst the newly created slots
        for (int i = 0; i < invalidCards.Count; i++)
        {
            int slotIndex = i % newSlots.Count;
            newSlots[slotIndex].Add(invalidCards[i]);
        }

        // Create the new room controller with the new room contents
        return new (new RoomModel(roomSize, remainingPopulatedSlots.Concat(newSlots).ToList()));
    }

    private static List<RoomSlot> GetRemainingPopulatedSlotsFromRoom(RoomController roomController)
    {
        return roomController.Slots.Where(slot => !slot.IsEmpty).ToList();
    }
}
