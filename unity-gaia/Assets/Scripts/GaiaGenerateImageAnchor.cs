using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using TMPro;
using Zenject;

public class GaiaGenerateImageAnchor : MonoBehaviour
{


    [SerializeField]
    private ARReferenceImage referenceImage;

    [SerializeField]
    private ARReferenceImage sensorReferenceImage;

    [SerializeField]
    private ARReferenceImage anchorImage;
    [SerializeField]
    private ARReferenceImage anchorImageCopy;

    public GameObject prefabToGenerate;
    public GameObject sensorPrefab;

    private GameObject sensorGeneratedPrefab;

    public GameObject axisPrefab;

    private GameObject imageAnchorGO;

    public BarcodeCanvas barcodeCanvas;

    [Inject]
    GaiaNetworkManager networkManager;

    // Use this for initialization
    void Start()
    {
        UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
        UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpadteImageAnchor;
        //UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
        //Instantiate(prefabToGenerate, newObjectPosition, Quaternion.identity);
    }

    void AddImageAnchor(ARImageAnchor arImageAnchor)
    {
        Debug.Log("image anchor added: " + arImageAnchor.referenceImageName);

        Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
        Quaternion rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

        if (arImageAnchor.referenceImageName == referenceImage.imageName)
        {
            //imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, position, rotation);
            //CreateBarChart(position, rotation);
            //CreateGaiaPlane(arImageAnchor, position, rotation);
        }
        //else {
        //    CreateGaiaPlane(arImageAnchor, position, rotation);
        //}
        else if (arImageAnchor.referenceImageName == sensorReferenceImage.imageName)
        {
            if (barcodeCanvas != null) {
                Debug.Log("Barcode canvas is not null");
                barcodeCanvas.SetScanType(BarcodeCamera.Type.InfoSensor);
                barcodeCanvas.Show();
            }
            CreateSensorPlane(arImageAnchor, position, rotation);
        }
        else if (arImageAnchor.referenceImageName == anchorImage.imageName)
        {
            Debug.Log("Top-left anchor found at location: " + position.ToString());
            firstAnchorPosition = position;
        }
        else if (arImageAnchor.referenceImageName == anchorImageCopy.imageName)
        {
            Debug.Log("Bottom-right anchor found at location: " + position.ToString());

            Vector3 size = position - firstAnchorPosition;
            Vector3 centerPos = firstAnchorPosition + size / 2f;
            //GenerateMesh(size.x, size.z);

            CreateGaiaPlane(null, centerPos, rotation);
        }
    }

    private Vector3 firstAnchorPosition;
    private void GenerateMesh(float width, float height)
    {
        GameObject plane = new GameObject("Plane");
        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = CreateMesh(width, height);
        MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material.shader = Shader.Find("Particles/Additive");
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.green);
        tex.Apply();
        renderer.material.mainTexture = tex;
        renderer.material.color = Color.green;
    }

    Mesh CreateMesh(float width, float height)
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] {
            firstAnchorPosition,
            firstAnchorPosition + new Vector3(width, 0f, 0f),
            firstAnchorPosition + new Vector3(0f, 0f, height),
            firstAnchorPosition + new Vector3(width, 0f, height)
        };
        m.uv = new Vector2[] {
            new Vector2 (0, 0),
            new Vector2 (0, 1),
            new Vector2(1, 1),
            new Vector2 (1, 0)
        };
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();

        return m;
    }

    public void instantiate() {
        CreateGaiaPlane(null, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
    }

    private void CreateSensorPlane(ARImageAnchor arImageAnchor, Vector3 position, Quaternion rotation)
    {
        sensorGeneratedPrefab = Instantiate<GameObject>(sensorPrefab, position, rotation);
        sensorGeneratedPrefab.SetActive(true);
    }


    GameObject generatedPrefab;
    GameObject led2;
    private void CreateGaiaPlane(ARImageAnchor arImageAnchor, Vector3 position, Quaternion rotation)
    {
        generatedPrefab = Instantiate<GameObject>(prefabToGenerate, position, rotation);
        generatedPrefab.SetActive(true);
    }

    //float total = 0.1f;
    //float aspect = 0.29f / 0.21f;
    public void increasePostion() {
        //if (generatedPrefab != null) {
        //    total += 0.01f;
        //    generatedPrefab.transform.localScale = new Vector3(total, 1f, total / aspect);
        //}
        //Debug.Log("scale " + total.ToString());
    }

    //float totalScale = 0.05f;
    public void descreasePostion()
    {
        //if(generatedPrefab != null) {
        //    total -= 0.005f;
        //    generatedPrefab.transform.localScale = new Vector3(total, 1f, total / aspect);
        //}
        //Debug.Log("scale " + total.ToString());
    }

    void UpadteImageAnchor(ARImageAnchor arImageAnchor)
    {
        if (arImageAnchor.referenceImageName == referenceImage.imageName) {
            Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);

            generatedPrefab.transform.position = position + new Vector3(0.02f, 0f, 0.005f);
        }
    }

    void RemoveImageAnchor(ARImageAnchor arImageAnchor)
    {
        Debug.Log("image anchor removed");
        //foreach (GameObject cube in cubes)
        //{
        //    Destroy(cube);
        //}

    }

    void OnDestroy()
    {
        UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
        UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpadteImageAnchor;
        UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;

    }

    string[] temperatures = new string[] { "25", "27", "19", "30", "38" };
    int tempIndex = 0;
    TextMeshPro textMesh;
    GameObject textObject;

    private void CreateBarChart(Vector3 position, Quaternion rotation)
    {
        // text gameobject
        textObject = Instantiate<GameObject>(prefabToGenerate, position, rotation);
        textObject.GetComponent<TextComponent>().enabled = true;

        textMesh = textObject.GetComponent<TextMeshPro>();
        textMesh.text = temperatures[tempIndex];

        textMesh.transform.position = position;
        textMesh.transform.rotation = Quaternion.AngleAxis(90, Vector3.right);
    }

    public PlaneHitResult PlaneHitTest()
    {
        ARHitTestResultType[] resultTypes = {
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane,
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane,
                        //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    };

        Touch touch = new Touch();
        touch.position = new Vector2(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f);
        var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);

        ARPoint point = new ARPoint
        {
            x = screenPosition.x,
            y = screenPosition.y
        };

        foreach (ARHitTestResultType resultType in resultTypes)
        {
            PlaneHitResult result = HitTestWithResultType(point, resultType);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    IEnumerator ChangeTemperature()
    {
        yield return new WaitForSeconds(2);
        tempIndex++;

        if (tempIndex == temperatures.Length)
        {
            tempIndex = 0;
        }

        textMesh.text = temperatures[tempIndex];
        StartCoroutine(ChangeTemperature());
    }

    public void OnTemperatureReceived(string temperature)
    {
        int tempInt = int.Parse(temperature);

        if (tempInt > 25)
        {
            textMesh.color = Color.red;
        }
        else if (tempInt < 12)
        {
            textMesh.color = Color.blue;
        }
        else
        {
            textMesh.color = Color.white;
        }

        textMesh.text = temperature;
    }

    public float maxRayDistance = 30.0f;
    public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

    PlaneHitResult HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
    {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
        if (hitResults.Count > 0)
        {
            foreach (var hitResult in hitResults)
            {
                Debug.Log("Got hit!");

                var planeHitResult = new PlaneHitResult();
                planeHitResult.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                planeHitResult.rotation = Quaternion.AngleAxis(90, Vector3.right);
                planeHitResult.hitResult = hitResult;

                //Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", textObject.transform.position.x, textObject.transform.position.y, textObject.transform.position.z));
                //Debug.Log(string.Format("rotation x:{0:0.######} y:{1:0.######} z:{2:0.######}", textObject.transform.rotation.x, textObject.transform.rotation.y, textObject.transform.rotation.z));
                return planeHitResult;
            }
        }
        return null;
    }

    int angle = 90;

    public void RotateHorizontal()
    {
        textObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.right);
        angle += 90;
    }

    private void Update()
    {
        if (Input.touchCount == 1 && Input.touches[0].phase ==TouchPhase.Began) {
            HitTestNextPreviousButtons(Input.touches[0].position);
        }
    }

    private void HitTestNextPreviousButtons(Vector2 position)
    {
        //use center of screen for focusing
        Vector3 center = new Vector3(position.x, position.y, 0.2f);

        //#if UNITY_EDITOR
        Ray ray = Camera.main.ScreenPointToRay(center);
        RaycastHit hit;

        //we'll try to hit one of the plane collider gameobjects
        if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("LabControls")))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.name.Equals("Next"))
            {
                generatedPrefab.GetComponent<InstructionsController>().ShowNext();
            }
            else if (hitObject.name.Equals("Previous"))
            {
                generatedPrefab.GetComponent<InstructionsController>().ShowPrevious();
            }

            return;
        }
    }

}
