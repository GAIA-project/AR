using UnityEngine;
using TMPro;

public class SensorInfoVisual : MonoBehaviour
{

    public TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void UpdateText(string value)
    {
        textMesh.text = value;
    }
}
