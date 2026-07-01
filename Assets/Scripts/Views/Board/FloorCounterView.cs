using TMPro;
using UnityEngine;

public class FloorCounterView : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TMP_Text floorCounter_TMPText;

    void OnEnable()
    {
        gameManager.DungeonController.OnGoToNextFloor += OnGoToNextFloor;
    }

    void OnDisable()
    {
        gameManager.DungeonController.OnGoToNextFloor -= OnGoToNextFloor;
    }

    private void OnGoToNextFloor()
    {
        floorCounter_TMPText.text = "Floor: " + gameManager.DungeonController.GetFloorNumber().ToString();
    }

}