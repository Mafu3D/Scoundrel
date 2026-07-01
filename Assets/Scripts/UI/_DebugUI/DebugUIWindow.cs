using TMPro;
using UnityEngine;

namespace Project.UI.DebugUI
{
    public class DebugUIWindow : MonoBehaviour
    {
        [SerializeField] GameObject windowContainer;
        [SerializeField] TMP_Text windowTitle;
        [SerializeField] TMP_Text windowContent;

        public DebugUIWindowModel Model;

        public void OnToggleWindow()
        {
            if (windowContainer.activeSelf)
            {
                windowContainer.SetActive(false);
            }
            else
            {
                windowContainer.SetActive(true);
            }
        }

        void Update()
        {
            windowTitle.text = Model.Title;
            windowContent.text = Model.Content;
        }
    }

    public class DebugUIWindowModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
