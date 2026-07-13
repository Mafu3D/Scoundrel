using System;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[RequireComponent(typeof(MouseOverContextManager))]
public class DungeonInteractableCardView : CardViewExtender
{
    GameManager gameManager;

    private MouseOverContextManager mouseOverContextManager;

    protected override void Awake()
    {
        base.Awake();
        mouseOverContextManager = GetComponent<MouseOverContextManager>();
    }

    void Start()
    {
        ServiceLocator.Global.Get(out gameManager);
    }

    public override void OnClicked(MousePositionContext mousePositionContext)
    {
        //TOOD: This is just temporary
        gameManager.OnCardClicked(CardView.Card, mousePositionContext);
    }

    public override void OnMouseEnter() { }

    public override void OnMouseStay(MousePositionContext mousePositionContext)
    {
        DispatchMouseOver(mousePositionContext);
    }

    public override void OnMouseExit()
    {
        mouseOverContextManager.HideAllMouseOverContextViews();
    }

    private void DispatchMouseOver(MousePositionContext mousePositionContext)
    {
        if (mousePositionContext == MousePositionContext.NONE)
        {
            return;
        }

        switch (CardView.Card.CardType)
        {
            case CardType.MONSTER:
                ShowMonsterContext(mousePositionContext);
                break;
            case CardType.WEAPON:
                ShowWeaponContext();
                break;
            case CardType.POTION:
                ShowPotionContext();
                break;
            case CardType.DOOR:
                ShowDoorContext();
                break;
            case CardType.TREASURE:
                ShowTreasureContext();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void ShowMonsterContext(MousePositionContext mousePositionContext)
    {
        GameManager gameManager;
        ServiceLocator.Global.Get(out gameManager);
        if (!gameManager.CombatController.CheckForTaunt((this.CardView.Card as MonsterCardModel)))
        {
            mouseOverContextManager.ShowFull(globalCardData.BlockedByTauntText,
                                             globalCardData.BlockedByTauntColor);
            return;
        }

        if (mousePositionContext == MousePositionContext.TOP)
        {
            mouseOverContextManager.ShowTop(globalCardData.AttackWeaponText,
                                            globalCardData.AttackWeaponColor);
        }
        else if (mousePositionContext == MousePositionContext.BOT)
        {
            mouseOverContextManager.ShowBot(globalCardData.AttackUnarmedText,
                                            globalCardData.AttackUnarmedColor);
        }
    }

    private void ShowWeaponContext()
    {
        mouseOverContextManager.ShowFull(globalCardData.EquipWeaponText,
                                         globalCardData.EquipWeaponColor);
    }

    private void ShowPotionContext()
    {
        ServiceLocator.Global.Get(out GameManager gameManager);
        // Gross, clean up later!
        if ((gameManager.Player.HasDrankPotionThisRoom || gameManager.Player.IsAtMaxHealth) && (!(CardView.Card as PotionCardModel).IsArmor || !(CardView.Card as PotionCardModel).IsWhetstone))
        {
            mouseOverContextManager.ShowFull(globalCardData.DiscardPotionText,
                                             globalCardData.DiscardPotionColor);
        }
        else
        {
            mouseOverContextManager.ShowFull(globalCardData.DrinkPotionText,
                                             globalCardData.DrinkPotionColor);
        }
    }

    private void ShowDoorContext()
    {
        ServiceLocator.Global.Get(out GameManager gameManager);
        int scoreNeeded = gameManager.GetScoreToGoToNextFloor();
        if (gameManager.ScoreKeeper.GetScore() >= scoreNeeded)
        {
            mouseOverContextManager.ShowFull(globalCardData.DoorUnlockedText,
                                             globalCardData.DoorUnlockedColor);
        }
        else
        {
            mouseOverContextManager.ShowFull($"Locked:\nScore: {scoreNeeded}",
                                             globalCardData.DoorLockedColor);
        }
    }

    private void ShowTreasureContext()
    {
        // TODO: Treasure should get text from the actual treasure somehow?
        mouseOverContextManager.ShowFull(globalCardData.TreasureText,
                                         globalCardData.TreasureColor);
    }
}
