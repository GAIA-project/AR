using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;

public class PlaceModelManager : MonoBehaviour {

    public GameObject selectedModel;

    private bool moveObject;

	// Use this for initialization
	void Start () {
		
	}

    public void StartMovingObject(GameObject selectedModel)
    {
        this.selectedModel = Instantiate(selectedModel, new Vector3(-100f, -100f, -100f), Quaternion.identity);
        this.selectedModel.GetComponent<Rotate>().enabled = false;
        this.selectedModel.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        this.selectedModel.GetComponent<OnePlaneCuttingController>().enabled = false;
        this.moveObject = true;

        Debug.Log(this.selectedModel.name);
    }

    public void StopMovingObject()
    {
        this.selectedModel.GetComponent<Rotate>().enabled = true;
        this.selectedModel.GetComponent<Slicer>().enabled = true;
        this.selectedModel.GetComponent<Slicer>().AddSlicer();

        this.moveObject = false;
        this.selectedModel = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (!this.moveObject || this.selectedModel == null) return;

        var screenPosition = Camera.main.ScreenToViewportPoint(new Vector2(Screen.width / 2, Screen.height / 2));

        ARPoint point = new ARPoint
        {
            x = screenPosition.x,
            y = screenPosition.y
        };

        // prioritize reults types
        ARHitTestResultType[] resultTypes = {
                         ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                         // if you want to use infinite planes use this:
                         //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        // ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
                         //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                     };

        foreach (ARHitTestResultType resultType in resultTypes)
        {
            if (HitTestWithResultType(point, resultType))
            {
                return;
            }
        }
	}

    bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
    {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
        if (hitResults.Count > 0)
        {
            foreach (var hitResult in hitResults)
            {
                selectedModel.transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                Debug.Log("Hit position: " + selectedModel.transform.position);
                selectedModel.transform.rotation = Quaternion.identity;
                return true;
            }
        }
        return false;
    }
}
