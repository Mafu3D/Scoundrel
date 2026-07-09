using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomSlotView : MonoBehaviour
{
    [SerializeField] public GameObject CardViewPrefab;
    [SerializeField] public List<CardView> ExistingCardViews;
    [SerializeField] float cardOffset = 1f;

    private List<CardView> cardViewInstances = new();

    private RoomSlot slot;

    void Start()
    {
        RefreshView();
        cardViewInstances.AddRange(ExistingCardViews.Where(inst => inst != null));
    }

    public void RegisterRoomSlot(RoomSlot newSlot)
    {
        if (slot != null)
        {
            Debug.LogWarning("RoomSlotView is already registered to a slot. Deregistering the current slot.");
            DeregisterRoomSlot();
        }
        slot = newSlot;
        slot.OnCardsChanged += OnCardsChanged;
        RefreshView();
    }

    public void DeregisterRoomSlot()
    {
        if (slot != null)
        {
            slot.OnCardsChanged -= OnCardsChanged;
        }
        slot = null;

        RefreshView();
    }

    private void OnCardsChanged()
    {
        RefreshView();
    }

    private void RefreshView()
    {
        ClearAllCardViews();
        if (slot != null)
        {
            PopulateCardViews();
        }
    }

    private void ClearAllCardViews()
    {
        foreach (CardView cardView in cardViewInstances.ToList())
        {
            cardViewInstances.Remove(cardView);
            Destroy(cardView.gameObject);
        }
    }

    private void PopulateCardViews()
    {
        for (int i = 0; i < slot.Cards.Count; i++)
        {
            GameObject gameObject = Instantiate(CardViewPrefab, this.transform.position, Quaternion.identity, this.transform);

            CardView instance = gameObject.GetComponent<CardView>();
            instance.RegisterCard(slot.Cards[i]);

            gameObject.transform.localPosition += new Vector3(0f, cardOffset * i, 1f * i);
            instance.SetSortingLayer(-5 * i);

            instance.Clickable = i == 0;

            cardViewInstances.Add(instance);
        }
    }
}