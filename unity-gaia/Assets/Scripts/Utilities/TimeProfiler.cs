using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System;

public class TimeProfiler
{
    private static TimeProfiler instance;

    private List<TimeModel> logs;

    public static TimeProfiler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TimeProfiler();
            }

            return instance;
        }
    }

    public TimeProfiler()
    {
        logs = new List<TimeModel>();
    }

    public void SaveLogs()
    {
        SerializedLogs serializedLogs = new SerializedLogs();
        serializedLogs.logs = logs;

        string json = JsonUtility.ToJson(serializedLogs);
        string path = Application.persistentDataPath + "/execution-times-" + System.DateTime.Now.ToString("U") + ".log";

        Debug.Log("JSON logs path: " + path);

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(json);
            }
        }
    }

    public void AddLog(DateTime timestamp, TimeModel.Action action, float time)
    {
        logs.Add(new TimeModel(timestamp, action, time));
    }

    class SerializedLogs {
        public List<TimeModel> logs;
    }
}
