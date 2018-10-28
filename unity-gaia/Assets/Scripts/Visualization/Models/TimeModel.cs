using System;

[Serializable]
public class TimeModel
{
    [Serializable]
    public enum Action
    {
        PlaneDiscovery,
        QRScan,
        ServerComm,
        MQTTServer
    }

    public String timestamp;
    public Action action;
    public float executionTime;

    public TimeModel(DateTime timestamp, Action action, float executionTime)
    {
        this.timestamp = timestamp.ToString("R");
        this.action = action;
        this.executionTime = executionTime;
    }
}
