using TMPro;
using UnityEngine;

public class PlayerGoldView : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] TMP_Text gold_TMPText;

    void OnEnable()
    {
        Player.OnGoldChanged += OnPlayerGoldChanged;
    }

    void OnDisable()
    {
        Player.OnGoldChanged -= OnPlayerGoldChanged;
    }

    private void OnPlayerGoldChanged(int amount)
    {
        gold_TMPText.text = $"Gold: {amount}";
    }

}