using TMPro;
using UnityEngine;

public class BlacksmithBuffItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text titleTMP_Text;
    [SerializeField] private TMP_Text descriptionTMP_Text;
    [SerializeField] private TMP_Text costTMP_Text;

    private BlacksmithBuffItem buffItem;
    private Blacksmith blacksmithInstance;

    private BoxCollider2D myCollider;

    void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        HandleMousePosition();
    }

    public void OnClicked()
    {
        blacksmithInstance.SelectBuffItem(buffItem);
    }

    private void OnMouseDown()
    {
        blacksmithInstance.SelectBuffItem(buffItem);
        Debug.Log(blacksmithInstance.CurrentlySelectedBuffItem.Buff.Name);
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

        titleTMP_Text.text = buffItem.Buff.Name;
        descriptionTMP_Text.text = buffItem.Buff.Description;
    }

    public void DeregisterBuffItem()
    {
        buffItem = null;
        titleTMP_Text.text = "N/A";
        descriptionTMP_Text.text = "N/A";
    }

       private void HandleMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (myCollider.OverlapPoint(worldPos))
        {
        }
    }
}
