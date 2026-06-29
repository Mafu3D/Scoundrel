using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI.Tooltips
{
    [ExecuteInEditMode()]
    [RequireComponent(typeof(RectTransform))]
    public class TooltipCollectionView : MonoBehaviour
    {
        [SerializeField] GameObject tooltipViewPrefab;

        [Header("Mouse Position")]
        [SerializeField] bool positionWithMouse = false;
        [SerializeField] float yOffset = -20f;
        [SerializeField] float xOffset = 0f;

        private List<KeyValuePair<GameObject, TooltipView>> activeTooltipViews = new();
        private RectTransform myRectTransform;
        private CanvasGroup myCanvasGroup;
        private Transform anchor;

        void Awake()
        {
            myRectTransform = GetComponent<RectTransform>();
            myCanvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }

        public void RegisterTooltips(TooltipCollection tooltipCollection)
        {
            DeregisterTooltips();

            foreach (TooltipData tooltipData in tooltipCollection.TooltipDatas)
            {
                GameObject tooltipGameObject = Instantiate(tooltipViewPrefab, this.transform.position, Quaternion.identity, this.transform);
                TooltipView tooltipView = tooltipGameObject.GetComponent<TooltipView>();
                tooltipView.RegisterTooltipData(tooltipData);

                activeTooltipViews.Add(new(tooltipGameObject, tooltipView));
            }
        }

        public void DeregisterTooltips()
        {
            foreach(KeyValuePair<GameObject, TooltipView> tooltipView in activeTooltipViews)
            {
                Destroy(tooltipView.Key);
            }
            activeTooltipViews = new();
        }

        public void Show() {
            UpdatePosition();
            StartCoroutine(BufferShowRoutine());
        }

        private IEnumerator BufferShowRoutine()
        {
            yield return new WaitForEndOfFrame();
            myCanvasGroup.alpha = 1;
        }

        public void Hide() => myCanvasGroup.alpha = 0;

        public void SetAnchor(Transform anchor)
        {
            this.anchor = anchor;
        }

        private void Update()
        {
            if (myCanvasGroup.alpha > 0)
            {
                UpdatePosition();

                // Continue to check if a triggerable object is beneath the pointer in case the object is hidden or destroyed
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                TooltipTrigger currentTrigger = null;
                if (hit.collider != null)
                {
                    hit.transform.TryGetComponent<TooltipTrigger>(out currentTrigger);
                }
                if (currentTrigger == null)
                {
                    Hide();
                }
            }



            // I had this check at some point... idk why?
            // if (Application.isEditor)
            // {
            // }
            // else
            // {
            // }
        }

        private void UpdatePosition()
        {
            Vector2 position = myRectTransform.position;
            Vector2 pivot = myRectTransform.pivot;
            if (anchor != null)
            {
                position = Camera.main.WorldToScreenPoint(anchor.position);
                pivot = new Vector2(0, 1);
            }
            else if (positionWithMouse)
            {
                position = Input.mousePosition;
                float pivotX = position.x / Screen.width;
                float pivotY = position.y / Screen.height;
                pivot = new Vector2(pivotX, pivotY);
                position += new Vector2(xOffset, yOffset);
            }

            myRectTransform.pivot = pivot;
            myRectTransform.position = position;
        }
    }
}
