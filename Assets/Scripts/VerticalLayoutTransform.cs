using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class VerticalLayoutTransform : MonoBehaviour
{
    [SerializeField] float offset = 0f;
    private void Update()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            child.localPosition = new Vector3(0f, offset * i, 0f);
        }
    }
}
