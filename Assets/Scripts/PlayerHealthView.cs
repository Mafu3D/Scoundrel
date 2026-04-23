using TMPro;
using UnityEngine;

public class PlayerHealthView : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] TMP_Text health_TMPText;

    void OnEnable()
    {
        Player.OnHealthChanged += OnPlayerHealthChanged;
    }

    void OnDisable()
    {
        Player.OnHealthChanged -= OnPlayerHealthChanged;
    }

    private void OnPlayerHealthChanged(int amount)
    {
        health_TMPText.text = $"{amount} / {Player.MaxHealth}";
    }

}