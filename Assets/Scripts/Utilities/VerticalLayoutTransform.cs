using System.Collections.Generic;
using UnityEngine;

public enum TransformLayoutDirection
{
    UP,
    DOWN
}

[ExecuteInEditMode()]
public class VerticalLayoutTransform : MonoBehaviour
{
    [SerializeField] float offset = 0f;
    [SerializeField] TransformLayoutDirection layoutDirection = TransformLayoutDirection.UP;

    private void Update()
    {
        int directionMult = layoutDirection switch
        {
            TransformLayoutDirection.UP => 1,
            TransformLayoutDirection.DOWN => -1,
            _ => 1,
        };

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            float zPos = child.transform.localPosition.z;
            child.localPosition = new Vector3(0f, offset * i * directionMult, zPos);
        }
    }
}
