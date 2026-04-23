using TMPro;
using UnityEngine;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text endMessage_TMPText;
    [SerializeField] private TMP_Text score_TMPText;

    public void UpdateText(bool playerWon, int score)
    {
        endMessage_TMPText.text = playerWon ? "You Win!" : "You Lose!";
        score_TMPText.text = $"Score: {score.ToString()}";
    }
}