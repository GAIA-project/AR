using UnityEngine;
using TMPro;
using Zenject;

public class TextComponent : MonoBehaviour {

    public float UPDATE_TIMEOUT = 1.0f;

    private float _updateTimeout;

    GaiaNetworkManager networkManager;

    public TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        _updateTimeout = UPDATE_TIMEOUT;
    }

    private void OnEnable()
    {
        networkManager = FindObjectOfType<GaiaNetworkManager>();
    }

    // Update is called once per frame
    void Update () {
        _updateTimeout -= Time.deltaTime;

        if (_updateTimeout < 0)
        {
            _updateTimeout = UPDATE_TIMEOUT;
            if (networkManager == null) return;

            networkManager.Refresh( (newStatus) => 
            {
                UpdateText(newStatus);
            });
        }
	}

    void UpdateText(StatusJson data)
    {
        if (textMesh == null) return;

        float tempInt = data.Temperature;

        if (tempInt > 25)
        {
            textMesh.color = Color.red;
        }
        else if (tempInt < 16)
        {
            textMesh.color = Color.blue;
        }
        else
        {
            textMesh.color = Color.white;
        }

        textMesh.text = data.Temperature.ToString();
    }
}
