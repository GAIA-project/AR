using UnityEngine;
using System.Collections;
using TMPro;

public class VisualTemperature : MonoBehaviour
{
    public TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void UpdateText(float value)
    {
        if (textMesh == null) return;

        if (value > 25)
        {
            textMesh.color = Color.red;
        }
        else if (value < 16)
        {
            textMesh.color = Color.blue;
        }
        else
        {
            textMesh.color = Color.white;
        }

        textMesh.text = value.ToString();
    }
}
