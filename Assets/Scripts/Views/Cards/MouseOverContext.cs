using TMPro;
using UnityEngine;

public class MouseOverContextView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backgroundSprite;
    [SerializeField] private TMP_Text text;
    // Icon/animation goes here eventually?

    public void Show(string text, Color color)
    {
        this.text.text = text;
        backgroundSprite.color = color;
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        backgroundSprite.color = Color.gray;
        text.text = "None";
    }
}
