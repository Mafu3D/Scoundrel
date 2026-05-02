using TMPro;
using UnityEngine;

public class RoundCounterView : MonoBehaviour
{
    [SerializeField] GameManager GameManager;
    [SerializeField] TMP_Text roundCounter_TMPText;

    void OnEnable()
    {
        GameManager.OnOpenNewRoom += OnOpenNewRoom;
    }

    void OnDisable()
    {
        GameManager.OnOpenNewRoom -= OnOpenNewRoom;
    }

    private void OnOpenNewRoom()
    {
        roundCounter_TMPText.text = "Round: " + GameManager.RoomNumber.ToString();
    }

}