using UnityEngine;

[RequireComponent(typeof(CardView))]
public abstract class CardViewExtender : MonoBehaviour
{
    public CardView CardView { get; private set; }

    protected GlobalCardData globalCardData;

    public abstract void OnClicked(MousePositionContext mousePositionContext);
    public abstract void OnMouseEnter();
    public abstract void OnMouseExit();
    public abstract void OnMouseStay(MousePositionContext mousePositionContext);

    protected virtual void Awake()
    {
        CardView = GetComponent<CardView>();
        globalCardData = CardView.GlobalCardData;
    }
}
