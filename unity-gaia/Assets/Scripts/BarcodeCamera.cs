using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;
using ZXing;

public class BarcodeCamera : MonoBehaviour
{

    [Inject]
    GaiaNetworkManager networkManager;

    public RenderTexture tex;

    // Use this for initialization
    void Start()
    {
        tex = new RenderTexture(512, 512, 16);
        GetComponent<Camera>().targetTexture = tex;
    }

    public void SaveBarcodeCamera()
    {
        StartCoroutine(SaveBarcodeCoroutine());
    }

    IEnumerator SaveBarcodeCoroutine()
    {
        var texture = toTexture2D(tex);
        byte[] bytes = texture.EncodeToPNG();

        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(texture.GetPixels32(), texture.width, texture.height);
            if (result != null)
            {
                Debug.Log("DECODED TEXT FROM QR: " +result.Text);

                networkManager.ParseAndSubscribe(result.Text);
            }

        }
        catch (Exception ex) { Debug.LogWarning(ex.Message); }

        //var folder = Directory.CreateDirectory(Application.persistentDataPath + "/Images");

        //File.WriteAllBytes(Application.persistentDataPath + "/Images/barcode.png", bytes);

        yield return null;
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
