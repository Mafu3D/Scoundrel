using TMPro;
using UnityEngine;

public class ScorekeeperView : MonoBehaviour
{
    [SerializeField] GameManager GameManager;
    [SerializeField] TMP_Text score_TMPText;

    bool subscribed = false;

    void OnEnable()
    {
        if (!subscribed && GameManager.ScoreKeeper != null)
        {
            GameManager.ScoreKeeper.OnScoreUpdated += OnScoreUpdated;
            subscribed = true;
        }
    }

    void OnDisable()
    {
        if (subscribed)
        {
            GameManager.ScoreKeeper.OnScoreUpdated -= OnScoreUpdated;
        }
    }

    void Start()
    {
        score_TMPText.text = "0";

        // Fix this, its running in the same loop as the scorekeeper is created
        // this may break when you build it
        if (!subscribed && GameManager.ScoreKeeper != null)
        {
            GameManager.ScoreKeeper.OnScoreUpdated += OnScoreUpdated;
            subscribed = true;
        }
    }

    private void OnScoreUpdated(int score)
    {
        score_TMPText.text = score.ToString();
    }

}