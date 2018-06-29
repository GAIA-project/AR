using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class GenerateImageAnchor : MonoBehaviour {


	[SerializeField]
	private ARReferenceImage referenceImage;

	[SerializeField]
	private GameObject prefabToGenerate;

	private GameObject imageAnchorGO;

    [SerializeField]
    private GameObject ledPrefab;
    private List<GameObject> leds = new List<GameObject>();

    float TimeInterval;
    public float UpdateInterval;

	// Use this for initialization
	void Start () {
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;

	}

	void AddImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.Log ("image anchor added");
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			Vector3 position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
			Quaternion rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);

			//imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, position, rotation);

            CreateLeds(position, rotation);
		}
	}

	void UpdateImageAnchor(ARImageAnchor arImageAnchor)
	{
		//Debug.Log ("image anchor updated");
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
            Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
            Quaternion rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

            //imageAnchorGO.transform.position = position;
            //imageAnchorGO.transform.rotation = rotation;

            CreateOrUpdateLeds(position, rotation);
		}

	}

	void RemoveImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.Log ("image anchor removed");
        if (leds.Count > 0) {
            DestroyLeds();
		}

	}

	void OnDestroy()
	{
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;

	}

	// Update is called once per frame
	void Update () {
		
	}

    private void CreateLeds(Vector3 position, Quaternion rotation)
    {
        DestroyLeds();
        CreateOrUpdateLeds(position, rotation);
    }

    private void CreateOrUpdateLeds(Vector3 position, Quaternion rotation)
    {
        Vector3 ledPosition;
        GameObject led;

        //Debug.Log("Anchor original position: " + position);
        //Debug.Log("Anchor rotation: " + rotation);

        //Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0.1f);
        //Ray ray = Camera.main.ScreenPointToRay(center);
        //RaycastHit hit;

        ////we'll try to hit one of the plane collider gameobjects
        //if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("ARKitPlane")))
        //{
        //    GameObject hitObject = hit.collider.gameObject;

        //    //we're going to get the position from the contact point
        //    Debug.Log(hitObject.name + " was hit");

        //    position = new Vector3(position.x, hitObject.transform.position.y, position.z);
        //}

        TimeInterval += Time.deltaTime;

        for (int i = -1; i < 2; i++)
        {
            ledPosition = position + rotation * new Vector3(i * 0.05f, 0f, 0f);
            //Debug.Log("Led converted position: " + ledPosition);

            if (i + 1 < leds.Count)
            {
                if (TimeInterval < UpdateInterval) return;

                // updates LED position
                led = leds[i + 1];
                led.transform.position = ledPosition;
                led.transform.rotation = rotation;
            }
            else
            {
                // create new LED
                led = Instantiate<GameObject>(ledPrefab, ledPosition, rotation);
                led.name = "LED " + (i + 2);
                leds.Add(led);
            }
        }

        if (TimeInterval > UpdateInterval) TimeInterval = 0.0f;
    }

    private void DestroyLeds()
    {
        foreach (GameObject led in leds)
        {
            Destroy(led);
        }

        leds.Clear();
    }
}
