using TMPro;
using UnityEngine;

public class PlayerArmorView : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] TMP_Text armor_TMPText;

    void OnEnable()
    {
        Player.OnArmorChanged += OnPlayerArmorChanged;
    }

    void OnDisable()
    {
        Player.OnArmorChanged -= OnPlayerArmorChanged;
    }

    private void OnPlayerArmorChanged(int amount)
    {
        armor_TMPText.text = Player.Armor.ToString();;
    }

}