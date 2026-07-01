using TMPro;
using UnityEngine;

public class FloorCounterView : MonoBehaviour
{
    [SerializeField] GameManager GameManager;
    [SerializeField] TMP_Text floorCounter_TMPText;

    void OnEnable()
    {
        GameManager.OnGoToNextFloor += OnGoToNextFloor;
    }

    void OnDisable()
    {
        GameManager.OnGoToNextFloor -= OnGoToNextFloor;
    }

    private void OnGoToNextFloor()
    {
        floorCounter_TMPText.text = "Floor: " + GameManager.FloorNumber.ToString();
    }

}