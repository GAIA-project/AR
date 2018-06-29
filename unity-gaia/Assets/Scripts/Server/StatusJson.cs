using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusJson {

    public float Temperature;
    public string Luminosity;
    public string Motion;
    public string Led1;
    public string Led2;
    public string Led3;
    public string Motor;

    public static StatusJson CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<StatusJson>(jsonString);
    }
}
