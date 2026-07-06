using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlacksmithBuffItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private TMP_Text titleTMP_Text;
    [SerializeField] private TMP_Text descriptionTMP_Text;
    [SerializeField] private TMP_Text costTMP_Text;
    [SerializeField] private Transform arrowTransform;

    [Header("Animation")]
    [SerializeField] private float onHoverGrowthSize = 1.1f;
    [SerializeField] private float onHoverGrowthSpeed = 0.1f;

    private BlacksmithBuffItem buffItem;
    private Blacksmith blacksmithInstance;

    private Transform myTransform;
    private BoxCollider2D myCollider;
    private Vector2 arrowStartingPosition;

    private bool isSelected => buffItem != null && blacksmithInstance != null && blacksmithInstance.CurrentlySelectedBuffItem == buffItem;

    void Awake()
    {
        myTransform = GetComponent<Transform>();
        myCollider = GetComponent<BoxCollider2D>();
        arrowStartingPosition = arrowTransform.localPosition;
    }

    private void Update()
    {
        HandleMousePosition();

        if (isSelected)
        {
            arrowTransform.gameObject.SetActive(true);

            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 direction = worldPos - (Vector2)arrowTransform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));

            arrowTransform.localPosition = arrowStartingPosition + (direction / 2);
        }
        else
        {
            arrowTransform.gameObject.SetActive(false);
        }

        if (isSelected)
        {
            myTransform.localScale = new Vector3(onHoverGrowthSize, onHoverGrowthSize, onHoverGrowthSize);
        }
    }

    private void OnMouseDown()
    {
        if (isSelected)
        {
            Debug.Log("deselect");
            blacksmithInstance.DeselectBuffItem();
        }
        else
        {
            blacksmithInstance.SelectBuffItem(buffItem);
        }
    }

    public void RegisterBuffItem(BlacksmithBuffItem buffItem, Blacksmith blacksmithInstance)
    {
        if (this.buffItem != null)
        {
            Debug.LogWarning("Instance already registed! Deregister the instance first.");
            return;
        }

        this.buffItem = buffItem;
        this.blacksmithInstance = blacksmithInstance;

        this.blacksmithInstance.OnBuffSelectionChanged += OnBuffSelectionChanged;

        titleTMP_Text.text = buffItem.Buff.Name;
        descriptionTMP_Text.text = buffItem.Buff.Description;
        this.gameObject.SetActive(true);
    }

    public void DeregisterBuffItem()
    {
        this.gameObject.SetActive(false);
        if (blacksmithInstance != null)
        {
            this.blacksmithInstance.OnBuffSelectionChanged -= OnBuffSelectionChanged;
        }
        buffItem = null;
        titleTMP_Text.text = "N/A";
        descriptionTMP_Text.text = "N/A";
    }

    private void OnBuffSelectionChanged(BlacksmithBuffItem item)
    {
        if (isSelected)
        {
            myTransform.localScale = new Vector3(onHoverGrowthSize, onHoverGrowthSize, onHoverGrowthSize);
        }
        else
        {
            myTransform.DOScale(1f, onHoverGrowthSpeed);
        }
    }

    private void HandleMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (myCollider.OverlapPoint(worldPos))
        {
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
        {
            myTransform.DOScale(onHoverGrowthSize, onHoverGrowthSpeed);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            myTransform.DOScale(1f, onHoverGrowthSpeed);
        }
    }
}
