using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using TMPro;

public class GaiaGenerateImageAnchor : MonoBehaviour
{


    [SerializeField]
    private ARReferenceImage referenceImage;

    [SerializeField]
    private GameObject prefabToGenerate;
    public GameObject cubePrefab;
    public GameObject axisPrefab;

    private GameObject imageAnchorGO;

    //private List<GameObject> cubes = new List<GameObject>();

    //private float cubeWidth = 0f;

    // Use this for initialization
    void Start()
    {
        UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
        UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpadteImageAnchor;
        UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
        //Instantiate(prefabToGenerate, newObjectPosition, Quaternion.identity);
    }

    void AddImageAnchor(ARImageAnchor arImageAnchor)
    {
        Debug.Log("image anchor added: " + arImageAnchor.referenceImageName);

        Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
        Quaternion rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

        if (arImageAnchor.referenceImageName == referenceImage.imageName) {
            //imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, position, rotation);
            CreateBarChart(position, rotation);
        }
        //else {
        //    CreateGaiaPlane(arImageAnchor, position, rotation);
        //}
    }

    private void CreateGaiaPlane(ARImageAnchor arImageAnchor, Vector3 position, Quaternion rotation)
    {
        GameObject cube = Instantiate<GameObject>(axisPrefab, position, rotation);

        position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
        rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

        Vector3 size = cube.GetComponent<BoxCollider>().bounds.size;

        //position = new Vector3(position.x - arImageAnchor.referenceImagePhysicalSize / 2f, position.y + size.y, position.z);

        Instantiate<GameObject>(axisPrefab, position, rotation);

        //cube.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        //PlaneHitResult result = PlaneHitTest();
        //if (result != null)
        //{
        //    cube.transform.position = result.position;
        //    //cube.transform.rotation = result.rotation;
        //}
    }

    void UpadteImageAnchor(ARImageAnchor arImageAnchor)
    {
        //Debug.Log ("image anchor updated");
        if (arImageAnchor.referenceImageName == referenceImage.imageName) {
        //	imageAnchorGO.transform.position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
        //	imageAnchorGO.transform.rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);

            Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);

            textMesh.transform.position = position;
            textMesh.transform.rotation = Quaternion.AngleAxis(90, Vector3.right);
        }

        //Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
        //Quaternion rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

        //for (int i = 0; i < 5; i++)
        //{
        //    Vector3 newPosition = new Vector3(position.x + i * cubeWidth, cubes[i].transform.position.y, position.z);
        //    cubes[i].transform.position = newPosition;
        //}
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
        //Quaternion newRotation = new Quaternion(rotation.x, rotation.y - 90f, rotation.z, rotation.w);

        //for (int i = 0; i < 5; i++)
        //{
        //    float heightScale = 0.05f * Random.Range(1f, 5f);
        //    Vector3 newPosition = new Vector3(position.x + i * cubeWidth, position.y + heightScale / 2f, position.z);

        //    GameObject cube = Instantiate<GameObject>(prefabToGenerate, newPosition, newRotation);
        //    cube.transform.localScale = new Vector3(0.05f, heightScale, 0.05f);

        //    cubeWidth = cube.transform.localScale.x;

        //    cubes.Add(cube);
        //}

        //float heightScale = 0.05f * Random.Range(1f, 5f);
        //Vector3 newPosition = new Vector3(position.x, position.y + 0.05f / 2f, position.z);

        //cube = Instantiate<GameObject>(cubePrefab, position, rotation);
        //cube.transform.localScale = new Vector3(0.05f, heightScale, 0.05f);

        //newPosition = new Vector3(position.x, position.y, position.z);

        // text gameobject
        textObject = Instantiate<GameObject>(prefabToGenerate, position, rotation);
        textObject.GetComponent<TextComponent>().enabled = true;

        textMesh = textObject.GetComponent<TextMeshPro>();
        textMesh.text = temperatures[tempIndex];

        textMesh.transform.position = position;
        textMesh.transform.rotation = Quaternion.AngleAxis(90, Vector3.right);

        //temperatureText.transform.rotation = rotation;

        //PlaneHitResult result = PlaneHitTest();
        //if (result != null)
        //{
        //    textObject.transform.position = result.position;
        //    textObject.transform.rotation = result.rotation;
        //}

        //StartCoroutine(ChangeTemperature());
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

}
