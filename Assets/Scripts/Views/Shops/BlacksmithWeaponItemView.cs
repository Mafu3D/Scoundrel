using UnityEngine;

[RequireComponent(typeof(MouseOverContextManager))]
public class BlacksmithWeaponItemView : CardViewExtender
{
    // TODO: Move into globals?
    [SerializeField] private Color upgradeColor;
    [SerializeField] private string upgradeText;
    [SerializeField] private Color cannotUpgradeColor;
    [SerializeField] private string buffNotSelectedText;
    [SerializeField] private string noUpgradeRemainingText;
    [SerializeField] private string notEnoughGoldText;

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
            blacksmithInstance.DeselectWeapon();
        }
    }

    public override void OnMouseEnter() { }

    public override void OnMouseStay(MousePositionContext mousePositionContext)
    {
        if (!blacksmithInstance.ValidateSelectedWeaponCanBeUpgrade(weaponItem))
        {
            mouseOverContextManager.ShowFull(noUpgradeRemainingText,
                                             cannotUpgradeColor);
        }
        else if (blacksmithInstance.CurrentlySelectedBuffItem == null)
        {
            mouseOverContextManager.ShowFull(buffNotSelectedText,
                                             cannotUpgradeColor);
        }
        else if (!blacksmithInstance.ValidatePlayerHasEnoughGold())
        {
            mouseOverContextManager.ShowFull(notEnoughGoldText,
                                             cannotUpgradeColor);
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

        this.weaponItem = weaponItem;
        this.blacksmithInstance = blacksmithInstance;
        CardView.RegisterCard(weaponItem.Card);
    }

    public void DeregisterWeaponItem()
    {
        weaponItem = null;
        if (CardView != null)
        {
            CardView.DeregisterCard();
        }
    }

}
