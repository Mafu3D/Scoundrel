using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    [SerializeField] GameObject helpObject;

    // Start is called before the first frame update
    void Start()
    {
        helpObject.SetActive(false);
    }

    public void ToggleHelp()
    {
        if (helpObject.activeSelf)
        {
            helpObject.SetActive(false);
        }
        else
        {
            helpObject.SetActive(true);
        }
    }
}
