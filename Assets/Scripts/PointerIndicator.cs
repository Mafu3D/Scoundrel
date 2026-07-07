using System;
using System.Collections.Generic;
using UnityEngine;

public class PointerIndicator : MonoBehaviour
{
    [SerializeField] Transform baseTransform;
    [SerializeField] Transform pointerTransform;

    [SerializeField] float cursorOffset = 1f;

    [Header("Intermediate Objects")]
    [SerializeField] int maxObjects = 5;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float minDistance = 1f;
    [SerializeField] Sprite intermediateRepeatingSprite;
    [SerializeField] List<Transform> intermediateRepeatingObjects;

    void Start()
    {
        Hide();
    }

    public void UpdatePointerPosition()
    {
        pointerTransform.gameObject.SetActive(true);

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the offset and angle towards
        Vector2 pointerOffset = worldPos - (Vector2) baseTransform.position;
        Vector2 direction = pointerOffset.normalized;
        float angle = Mathf.Atan2(pointerOffset.y, pointerOffset.x) * Mathf.Rad2Deg;
        angle -= 90;
        pointerTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Offset the cursor towards the base
        float distance = Math.Clamp(pointerOffset.magnitude - cursorOffset, 0, 9999999f);
        Vector2 pointerPosition = (Vector2) baseTransform.localPosition + (direction * distance);
        pointerTransform.localPosition = pointerPosition;

        // Get the distance between the base and pointer position
        int amount = Math.Min((int) Math.Floor(distance / minDistance), maxObjects);

        float interval = distance / amount;

        for (int i = 0; i < intermediateRepeatingObjects.Count; i++)
        {
            if (i < amount - 1)
            {
                intermediateRepeatingObjects[i].gameObject.SetActive(true);
                Vector2 offset = new (direction.x * (interval * (i+1)), direction.y * (interval * (i+1)));
                intermediateRepeatingObjects[i].localPosition = (Vector2) baseTransform.localPosition + offset;
            }
            else
            {
                intermediateRepeatingObjects[i].gameObject.SetActive(false);
                intermediateRepeatingObjects[i].localPosition = baseTransform.localPosition;
            }
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
