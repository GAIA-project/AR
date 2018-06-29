using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip3D : MonoBehaviour {

    private const float CUBE_SIZE = 0.05f;
    private const float CUBE_OFFSET = 0.025f; 

    public bool hasCompareAction;
    public bool hasSelectAction;
    public bool hasRotateAction;

    public Transform buttonPrefab;

    // Use this for initialization
    void Start () {
        Bounds modelMeshBounds = GetComponent<MeshRenderer>().bounds;

        int index = -1;
        Vector3 position;

        if (hasCompareAction)
        {
            index++;

            position = new Vector3(transform.position.x + modelMeshBounds.size.x / 2f + index * CUBE_SIZE + CUBE_OFFSET, transform.position.y + modelMeshBounds.size.z, transform.position.z);
            var button = Instantiate(buttonPrefab, position, Quaternion.identity);
            //button.transform.parent = transform;

            var textComponent = button.GetComponentInChildren<TextMeshPro>();
            textComponent.text = "Compare";
        }

        if (hasSelectAction)
        {
            index++;
            position = new Vector3(transform.position.x + modelMeshBounds.size.x / 2f + index * CUBE_SIZE + CUBE_OFFSET, transform.position.y + modelMeshBounds.size.z, transform.position.z);
            var button = Instantiate(buttonPrefab, position, Quaternion.identity);
            //button.transform.parent = transform;

            var textComponent = button.GetComponentInChildren<TextMeshPro>();
            textComponent.text = "Select";
        }

        if (hasRotateAction)
        {
            index++;
            position = new Vector3(transform.position.x + modelMeshBounds.size.x / 2f + index * CUBE_SIZE + CUBE_OFFSET, transform.position.y + modelMeshBounds.size.z, transform.position.z);
            var button = Instantiate(buttonPrefab, position, Quaternion.identity);
            //button.transform.parent = transform;

            var textComponent = button.GetComponentInChildren<TextMeshPro>();
            textComponent.text = "Rotate";
        }

	}
	
	// Update is called once per frame
	void Update () {

	}

    void ToggleTooltip()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }


}
