using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="NewGlobalCardData", menuName="Card/Global Card Data")]
public class GlobalCardData : ScriptableObject
{
    [Header("Card Colors")]
    [SerializeField] public Color Red;
    [SerializeField] public Color Black;
    [SerializeField] public Color ValueIncreasedColor;
    [SerializeField] public Color ValueDecreasedColor;

    [Header("Suit Sprites")]
    [SerializeField] public Sprite UnknownSprite;
    [SerializeField] public Sprite DiamondsSprite;
    [SerializeField] public Sprite HeartsSprite;
    [SerializeField] public Sprite ClubsSprite;
    [SerializeField] public Sprite SpadesSprite;
    [SerializeField] public Sprite DoorsSprite;
    [SerializeField] public Sprite TreasuresSprite;

    [Header("Weapon Context")]
    [SerializeField] public string AttackWeaponText;
    [SerializeField] public Color AttackWeaponColor;

    [Header("Unarmed Context")]
    [SerializeField] public string AttackUnarmedText;
    [SerializeField] public Color AttackUnarmedColor;

    [Header("Weapon (Equip) Context")]
    [SerializeField] public string EquipWeaponText;
    [SerializeField] public Color EquipWeaponColor;

    [Header("Weapon (Discard) Context")]
    [SerializeField] public string DiscardWeaponText;
    [SerializeField] public Color DiscardWeaponColor;

    [Header("Potion (Drink) Context")]
    [SerializeField] public string DrinkPotionText;
    [SerializeField] public Color DrinkPotionColor;

    [Header("Potion (Discard) Context")]
    [SerializeField] public string DiscardPotionText;
    [SerializeField] public Color DiscardPotionColor;

    [Header("Treasure Context")]
    [SerializeField] public string TreasureText;
    [SerializeField] public Color TreasureColor;

    [Header("Blocked by Taunt")]
    [SerializeField] public string BlockedByTauntText;
    [SerializeField] public Color BlockedByTauntColor;

    [Header("DoorContext")]

    [Header("Door (Unlocked) Context")]
    [SerializeField] public string DoorUnlockedText;
    [SerializeField] public Color DoorLockedColor;
    [SerializeField] public Color DoorUnlockedColor;

    public Sprite GetSuitSprite(Suit suit)
    {
        return suit switch
        {
            Suit.SPADES => SpadesSprite,
            Suit.HEARTS => HeartsSprite,
            Suit.DIAMONDS => DiamondsSprite,
            Suit.CLUBS => ClubsSprite,
            Suit.SKULLS => UnknownSprite,
            Suit.GOBLINS => UnknownSprite,
            Suit.POTIONS => UnknownSprite,
            Suit.WEAPONS => UnknownSprite,
            Suit.DOORS => DoorsSprite,
            Suit.TREASURES => TreasuresSprite,
            Suit.QUESTS => UnknownSprite,
            _ => UnknownSprite,
        };
    }
}
