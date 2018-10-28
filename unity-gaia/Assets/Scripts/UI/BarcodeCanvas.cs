using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BarcodeCanvas : MonoBehaviour {

    BarcodeCamera barcodeCamera;

    public float TIMEOUT_VALUE = 1.0f;

    BarcodeCamera.Type scanType;

    [Inject(Id = "MainCanvas")]
    RectTransform mainCanvas;

	// Use this for initialization
	void Start () {
        barcodeCamera = FindObjectOfType<BarcodeCamera>();
	}

    public void ScanAndSubscribeToSensor()
    {
        Scan(scanType);
    }

    public void Scan(BarcodeCamera.Type type)
    {
        if (barcodeCamera == null)
        {
            barcodeCamera = FindObjectOfType<BarcodeCamera>();
        }

        barcodeCamera.SaveBarcodeCamera(type, (foundBarcode) => {
            Debug.Log("Scanning image for barcode, found: " + foundBarcode);
            if (foundBarcode)
            {
                gameObject.SetActive(false);
                mainCanvas.gameObject.SetActive(true);
            }
        });
    }

    public void Show()
    {
        mainCanvas.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        mainCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
        this.scanType = BarcodeCamera.Type.SubscribeSensor;
    }

    public void SetScanType(BarcodeCamera.Type type)
    {
        this.scanType = type;  
    }
}
