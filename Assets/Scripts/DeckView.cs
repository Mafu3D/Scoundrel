using TMPro;
using UnityEngine;

public class DeckView : MonoBehaviour
{
    [SerializeField] DeckManager DeckManager;
    [SerializeField] TMP_Text remainingCards_TMPText;

    void OnEnable()
    {
        DeckManager.OnCardDraw += OnCardDraw;
    }

    void OnDisable()
    {
        DeckManager.OnCardDraw -= OnCardDraw;
    }

    private void OnCardDraw()
    {
        remainingCards_TMPText.text = $"{DeckManager.Deck.CurrentCount}/{DeckManager.Deck.TotalCount}";
    }
}