using UnityEngine;
using UnityEngine.UI;

public class DeckUpgradeChoiceProceedButton : MonoBehaviour
{
    [SerializeField] private DeckUpgradeChoiceView deckUpgradeChoiceView;
    private Button myButton;
    void Awake()
    {
        myButton = GetComponent<Button>();
    }

    void Update()
    {
        myButton.interactable = deckUpgradeChoiceView.CanProceed;
    }

    public void Proceed()
    {
        deckUpgradeChoiceView.Proceed();
    }
}