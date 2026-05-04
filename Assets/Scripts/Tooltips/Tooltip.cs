using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Tooltips
{

    [ExecuteInEditMode()]
    public class Tooltip : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] int characterWrapLimit;

        [Header("Mouse Position")]
        [SerializeField] bool positionWithMouse = false;
        [SerializeField] float yOffset = -20f;
        [SerializeField] float xOffset = 0f;

        [Header("Components")]
        [SerializeField] TextMeshProUGUI headerText;
        [SerializeField] TextMeshProUGUI contentText;
        [SerializeField] LayoutElement layoutElement;

        private RectTransform rectTransform;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetText(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header))
            {
                headerText.gameObject.SetActive(false);
            }
            else
            {
                headerText.gameObject.SetActive(true);
                headerText.text = header;
            }
            contentText.text = content;

            int headerLength = headerText.text.Length;
            int contentLength = contentText.text.Length;
            layoutElement.enabled = (headerLength > characterWrapLimit) || (contentLength > characterWrapLimit) ? true : false;
        }

        void Update()
        {
            int headerLength = headerText.text.Length;
            int contentLength = contentText.text.Length;
            layoutElement.enabled = (headerLength > characterWrapLimit) || (contentLength > characterWrapLimit) ? true : false;
            if (positionWithMouse)
            {
                Vector2 position = Input.mousePosition;
                float pivotX = position.x / Screen.width;
                float pivotY = position.y / Screen.height;
                rectTransform.pivot = new Vector2(pivotX, pivotY);
                rectTransform.position = position + new Vector2(xOffset, yOffset);
            }

            // I had this check at some point... idk why?
            // if (Application.isEditor)
            // {
            // }
            // else
            // {
            // }
        }
    }
}
