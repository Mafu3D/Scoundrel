using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextRoomButton : MonoBehaviour
{
    [SerializeField] GameManager GameManager;
    [SerializeField] Button button;

    void Update()
    {
        if (GameManager.CanGoToNextRoom)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
