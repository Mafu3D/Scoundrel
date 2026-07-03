using System;
using System.Collections.Generic;
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
        ResetRoomNumber();
        ResetFloorNumber();
        deckController.TEMP_CreateNewDeck();
    }

    public void GoToNextFloor()
    {
        IncrementFloorNumber();
        ResetRoomNumber();
        OnGoToNextFloor?.Invoke();
    }

    public void OpenNewRoom()
    {
        int roomSize = 4; // TODO: Magic number, get from the floor model
        List<RuntimeCardModel> newCards = new();

        if (CurrentRoom != null)
        {
            // Shuffle in any doors that still remain
            List<RuntimeCardModel> doorCards = CurrentRoom.PopDoorCards();
            deckController.Deck.ShuffleIn(doorCards);

            // Add the remaining cards from the current room to the new room
            newCards.AddRange(CurrentRoom.RemainingCards());

            CurrentRoom.OnCardsChanged -= OnCardsChanged;
        }

        // Create the new room contents by taking the remaining cards from the current room and drawing new cards from the deck
        int amountToDraw = roomSize - (CurrentRoom?.RemainingCount ?? 0);
        List<RuntimeCardModel> drawnCards = deckController.Draw(amountToDraw);
        newCards.AddRange(drawnCards);

        // Create the new room controller with the new room contents
        CurrentRoom = new RoomController(new RoomModel(roomSize, newCards));
        CurrentRoom.OnCardsChanged += OnCardsChanged;

        // Run all of the on drawn events for the cards in the room
        CurrentRoom.GetCards().ForEach(card => card?.HandleOnDraw());

        IncrementRoomNumber();
        OnNewRoomOpened?.Invoke();
    }


    public void RunFromRoom()
    {
        List<RuntimeCardModel> nonpersistantCards = CurrentRoom.PopNonPersistantCards();
        deckController.Deck.AddToRemaining(nonpersistantCards, addToTop: false, shuffle: false);
    }
    #endregion

    private void OnCardsChanged()
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
