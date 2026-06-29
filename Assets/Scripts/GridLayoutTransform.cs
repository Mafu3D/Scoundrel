using UnityEngine;

[ExecuteInEditMode()]
public class GridLayoutTransform : MonoBehaviour
{
    [SerializeField] float padding = 0f;
    [SerializeField] float cellSize = 10f;
    [SerializeField] int columns = 2;

    private void Update()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            child.localPosition = new Vector3((i % columns) * (cellSize + padding), -(i / columns) * (cellSize + padding), 0f);
        }
    }
}
