using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Logger : MonoBehaviour {

    public TextMeshProUGUI textComponent;

    private ScrollRect scrollRect; 

    static string myLog;
    private string output = "";

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // Remove callback when object goes out of scope
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        myLog += "\n" + output;

        textComponent.text = myLog;
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    public void ToggleLogger()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
