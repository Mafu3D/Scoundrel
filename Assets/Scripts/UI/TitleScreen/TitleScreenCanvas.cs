using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenCanvas : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public void StartNewRun() => gameManager.StartNewRun();
}
