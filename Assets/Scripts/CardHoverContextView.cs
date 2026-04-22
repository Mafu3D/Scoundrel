using TMPro;
using UnityEngine;

public class CardHoverContextView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bgSprite;
    [SerializeField] private TMP_Text TMP_Text;

    public void SetBGColor(Color color) => bgSprite.color = color;

    public void SetText(string text) => TMP_Text.text = text;

    public void Clear()
    {
        bgSprite.color = Color.gray;
        TMP_Text.text = "None";
    }
}
