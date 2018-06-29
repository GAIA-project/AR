using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CanvasModelsManager : MonoBehaviour {

    public List<GameObject> canvasModels = new List<GameObject>();

    private RectTransform rectTransform;

    PlaceModelManager _placeModelManager;
    GameObject _modelsCanvas;

    [Inject]
    private void Construct(PlaceModelManager placeModelManager, 
                           [Inject(Id = "ModelsCanvas")] RectTransform modelsCanvas)
    {
        _placeModelManager = placeModelManager;
        _modelsCanvas = modelsCanvas.gameObject;
    }

    public void ShowModels()
    {
        _modelsCanvas.SetActive(true);

        // already added items
        if (transform.childCount > 0)
        {
            return;
        }

        int index = 0;

        rectTransform = GetComponent<RectTransform>();
        Vector2 rectPosition = rectTransform.rect.center;

        foreach (GameObject model in canvasModels)
        {
            var canvasModel = Instantiate(model, transform);
            canvasModel.name = string.Format("{0}@{1}", model.name, index);

            canvasModel.AddComponent<RectTransform>();

            var layoutElement = canvasModel.AddComponent<LayoutElement>();
            layoutElement.flexibleWidth = 1f;
            layoutElement.flexibleHeight = 1f;

            canvasModel.transform.localScale = new Vector3(2f, 2f, 2f);
            canvasModel.GetComponent<OnePlaneCuttingController>().enabled = false;
            canvasModel.GetComponent<LayoutElement>().enabled = true;

            index++;
        }
    }

    public void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            Vector3 center = new Vector3(touch.position.x, touch.position.y, 0.1f);
            HitTestSchoolGrid(center);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 center = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.1f);
            HitTestSchoolGrid(center);
        }
    }

    public void HitTestSchoolGrid(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        //we'll try to hit one of the plane collider gameobjects
        if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Interactable")))
        {
            GameObject hitObject = hit.collider.gameObject;

            //we're going to get the position from the contact point
            Debug.Log(hitObject.name + " school was hit");

            //hitObject.transform.SetParent(_placeModelManager.transform.parent);

            int index = int.Parse(hitObject.name.Split('@')[1]);

            _placeModelManager.StartMovingObject(canvasModels[index]);
            _modelsCanvas.SetActive(false);
            return;
        }
    }
}
