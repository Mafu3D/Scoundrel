using TMPro;
using UnityEngine;

public class PlayerRunTokensView : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] TMP_Text tokens_TMPText;

    void OnEnable()
    {
        Player.OnRunTokensChanged += OnPlayerRunTokensChanged;
    }

    void OnDisable()
    {
        Player.OnRunTokensChanged -= OnPlayerRunTokensChanged;
    }

    private void OnPlayerRunTokensChanged(int amount)
    {
        tokens_TMPText.text = $"Run Tokens: {amount}";
    }

}