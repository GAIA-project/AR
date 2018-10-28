using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;
using ZXing;

public class BarcodeCamera : MonoBehaviour
{

    public enum Type
    {
        SubscribeSensor,
        InfoSensor
    }

    [Inject]
    GaiaNetworkManager networkManager;

    public RenderTexture tex;

    private float startTime;

    private IBarcodeReader barcodeReader;

    // Use this for initialization
    void Start()
    {
        tex = new RenderTexture(Screen.width, Screen.height, 16);
        GetComponent<Camera>().targetTexture = tex;
        barcodeReader = new BarcodeReader();
    }

    public void SaveBarcodeCamera(Type type, Action<bool> foundBarcode)
    {
        startTime = Time.realtimeSinceStartup;
        StartCoroutine(SaveBarcodeCoroutine(type, foundBarcode));
    }

    IEnumerator SaveBarcodeCoroutine(Type type, Action<bool> foundBarcode)
    {
        var texture = toTexture2D(tex);
        byte[] bytes = texture.EncodeToPNG();

        try
        {
            // decode the current frame
            var result = barcodeReader.Decode(texture.GetPixels32(), texture.width, texture.height);
            Debug.Log("Result: " + result);
            if (result != null)
            {
                Debug.Log("DECODED TEXT FROM QR: " + result.Text);
                foundBarcode(true);

                float scanTime = Time.realtimeSinceStartup - startTime;
                TimeProfiler.Instance.AddLog(System.DateTime.Now, TimeModel.Action.QRScan, scanTime);

                Debug.Log("Type: " + type.ToString());
                if (type == Type.SubscribeSensor)
                {
                    networkManager.ParseAndSubscribe(result.Text);
                }
                else if (type == Type.InfoSensor)
                {
                    Debug.Log("Barcode: " + result.Text);
                    networkManager.ParseAndSubscribeForInfo(result.Text);
                }
            }
            else
            {
                foundBarcode(false);
            }

        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
            foundBarcode(false);
        }

        //var folder = Directory.CreateDirectory(Application.persistentDataPath + "/Images");
        Debug.Log(Application.persistentDataPath);

        File.WriteAllBytes(Application.persistentDataPath + "/Images/barcode.png", bytes);

        yield return null;
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
