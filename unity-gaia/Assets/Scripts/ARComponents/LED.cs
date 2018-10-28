using UnityEngine;
using Zenject;

public class LED : MonoBehaviour, ISelfUpdate {

    public Material defaultMaterial;
    public Material inactiveMaterial;

    GameObject led;
    MeshRenderer meshRenderer;

    [Inject]
    GaiaNetworkManager networkManager;

    int id;
    string statusKey;

    private void Awake()
    {
        led = transform.GetChild(0).gameObject;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Use this for initialization
    void Start () {
        int.TryParse(gameObject.name.Split()[1], out id);
        statusKey = "Led" + id;
	}
	
	public bool ToggleLight()
    {
        led.SetActive(!led.activeSelf);

        if (led.activeSelf)
        {
            meshRenderer.material = defaultMaterial;
        }
        else {
            meshRenderer.material = inactiveMaterial;
        }

        int status = led.activeSelf ? 1 : 0;
        StartCoroutine(networkManager.ChangeLedStatus(id, status));

        return led.activeSelf;
    }

    public void SelfUpdate(StatusJson status)
    {
        int newStatus = int.Parse(status.GetType().GetField(statusKey).GetValue(status).ToString());
        Debug.Log(gameObject.name + " SelfUpdate: new status -> " + newStatus);

        led.SetActive(newStatus == 1);
    }
}
