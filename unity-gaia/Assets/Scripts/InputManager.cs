using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputManager : MonoBehaviour {

    private float startingDistanceFromCamera = 0.5f;

    private GameObject _modelsCanvas;

    [Inject]
    private void Construct([Inject(Id = "ModelsCanvas")] RectTransform modelsCanvas)
    {
        _modelsCanvas = modelsCanvas.gameObject;
    }

	// Use this for initialization
	void Start () {

	}

    // Update is called once per frame
    void Update()
    {
        if (_modelsCanvas.activeSelf) return;

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            LEDHitTest();
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
            LEDHitTest();
        }
#endif
    }

    void LEDHitTest()
    {
        //use center of screen for focusing
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, startingDistanceFromCamera);

        //#if UNITY_EDITOR
        Ray ray = Camera.main.ScreenPointToRay(center);
        RaycastHit hit;

        //we'll try to hit one of the plane collider gameobjects
        if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Interactable")))
        {
            GameObject hitObject = hit.collider.gameObject;

            //we're going to get the position from the contact point
            Debug.Log(hitObject.name + " was hit");

            if (hitObject.tag.Equals("Switch"))
            {
                Debug.Log(hitObject.name + " light on: " + hitObject.GetComponent<LED>().ToggleLight());
            }

            return;
        }

    }
}
