using TMPro;
using UnityEngine;

public class RoundCounterView : MonoBehaviour
{
    [SerializeField] GameManager GameManager;
    [SerializeField] TMP_Text roundCounter_TMPText;

    void OnEnable()
    {
        GameManager.OnEnterNewRoom += OnEnterNewRoom;
    }

    void OnDisable()
    {
        GameManager.OnEnterNewRoom -= OnEnterNewRoom;
    }

    private void OnEnterNewRoom()
    {
        roundCounter_TMPText.text = "Round: " + GameManager.RoomNumber.ToString();
    }

}