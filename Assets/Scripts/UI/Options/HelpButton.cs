using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    [SerializeField] GameObject helpTextObject;

    // Start is called before the first frame update
    void Start()
    {
        helpTextObject.SetActive(false);
    }

    public void ToggleHelp()
    {
        if (helpTextObject.activeSelf)
        {
            helpTextObject.SetActive(false);
        }
        else
        {
            helpTextObject.SetActive(true);
        }
    }
}
