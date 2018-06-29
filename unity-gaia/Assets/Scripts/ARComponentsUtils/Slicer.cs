using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Slicer : MonoBehaviour {

    public GameObject slicerPrefab;
    public GameObject slicer;

    private MeshRenderer meshRenderer;
    private OnePlaneCuttingController planeController;

    private Slider _ySlider;

    private Vector3 originalPosition;

    [Inject]
    private void Construct([Inject(Id = "SlicerYSlider")] RectTransform ySlider)
    {
        _ySlider = ySlider.GetComponent<Slider>();
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        planeController = GetComponent<OnePlaneCuttingController>();
    }

    public void AddSlicer()
    {
        slicerPrefab = Resources.Load("Prefabs/SlicePanel") as GameObject;
        Debug.Log("Slicer prefab: " + slicerPrefab);

        Vector3 bounds = Vector3.Scale(meshRenderer.bounds.size, new Vector3(1.4f, 1.4f, 1f));
        Vector3 scale = new Vector3(bounds.x, bounds.z, 1f);
        Debug.Log("Slicer scale: " + scale);

        Vector3 position = new Vector3(transform.position.x + bounds.x / 2f, transform.position.y + bounds.y, transform.position.y);

        slicer = Instantiate(slicerPrefab);
        slicer.transform.position = position;
        slicer.transform.localScale = scale;
        slicer.name = gameObject.name + "@slicer";

        planeController.plane = slicer;
        planeController.enabled = true;

        _ySlider = FindObjectOfType<Slider>();
        _ySlider.minValue = -bounds.y * 1.6f;
        _ySlider.onValueChanged.AddListener(OnYValueChanged);

        originalPosition = slicer.transform.position;
    }

    private void OnYValueChanged(float value)
    {
        Vector3 currentPosition = slicer.transform.position;

        Vector3 newPosition = new Vector3(currentPosition.x, originalPosition.y + value, currentPosition.z);
        slicer.transform.localPosition = newPosition;
    }
}
