using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpScreen : MonoBehaviour
{
    [SerializeField] List<GameObject> pages;

    [SerializeField] Button nextButton;
    [SerializeField] Button lastButton;

    private int currentIndex = 0;

    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            nextButton.interactable = currentIndex < pages.Count - 1;
            lastButton.interactable = currentIndex > 0;
        }
    }

    public void Next()
    {
        currentIndex = Math.Clamp(currentIndex + 1, 0, pages.Count-1);
        UpdatePages();

    }

    public void Back()
    {
        currentIndex = Math.Clamp(currentIndex - 1, 0, pages.Count-1);
        UpdatePages();
    }

    private void UpdatePages()
    {
                for (int i = 0; i < pages.Count; i++)
        {
            if (i == currentIndex)
            {
                pages[i].SetActive(true);
            }
            else
            {
                pages[i].SetActive(false);
            }
        }
    }

}
