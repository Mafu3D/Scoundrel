using UnityEngine;

[RequireComponent(typeof(MouseOverContextManager))]
public class BlacksmithWeaponItemView : CardViewExtender
{
    // TODO: Move into globals?
    [SerializeField] private Color upgradeColor;
    [SerializeField] private string upgradeText;
    [SerializeField] private Color buffNotSelectedColor;
    [SerializeField] private string buffNotSelectedText;

    CardView cardView;

    private BlacksmithWeaponItem weaponItem;
    private Blacksmith blacksmithInstance;
    private MouseOverContextManager mouseOverContextManager;

    protected override void Awake()
    {
        base.Awake();
        mouseOverContextManager = GetComponent<MouseOverContextManager>();
    }

    public override void OnClicked(MousePositionContext mousePositionContext)
    {
        blacksmithInstance.SelectWeapon(weaponItem);
        if (!blacksmithInstance.TryApplyUpgrade())
        {
            blacksmithInstance.DeselectAll();
        }
    }

    public override void OnMouseEnter() { }

    public override void OnMouseStay(MousePositionContext mousePositionContext)
    {
        if (blacksmithInstance.CurrentlySelectedBuffItem == null)
        {
            mouseOverContextManager.ShowFull(buffNotSelectedText,
                                             buffNotSelectedColor);
        }
        else
        {
            mouseOverContextManager.ShowFull(upgradeText,
                                             upgradeColor);
        }
    }

    public override void OnMouseExit()
    {
        mouseOverContextManager.HideAllMouseOverContextViews();
    }

    public void RegisterWeaponItem(BlacksmithWeaponItem weaponItem, Blacksmith blacksmithInstance)
    {
        if (this.weaponItem != null)
        {
            Debug.LogWarning("Instance already registed! Deregister the instance first.");
            return;
        }

        if (cardView == null)
        {
            cardView = GetComponent<CardView>();
        }

        this.weaponItem = weaponItem;
        this.blacksmithInstance = blacksmithInstance;
        Debug.Log(cardView);
        Debug.Log(weaponItem);
        Debug.Log(weaponItem.Card);
        cardView.RegisterCard(weaponItem.Card);
    }

    public void DeregisterWeaponItem()
    {
        weaponItem = null;
        cardView.DeregisterCard();
    }

}
