using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Zenject;

public class GaiaNetworkManager : MonoBehaviour
{

    class Response
    {
        public string access_token;
        public string token_type;
        public string refresh_type;
        public string expires_in;
        public string scope;
    }

    public InputField ipInputField;

    private String IP_ADDRESS = "http://150.140.5.100:5000/";

    //private MqttClient client;
    //[Inject]
    //GenerateImageAnchor imageAnchor;

    MqttClient client;

    private StatusJson lastServerStatus;

    // Use this for initialization
    void Start()
    {
        // Connect to sparworks server
        StartCoroutine(GetAccessToken());
        StartCoroutine(PingSensors(null));
    }

    public void SetIP()
    {
        IP_ADDRESS = ipInputField.text;
    }

    public IEnumerator PingSensors(Action<StatusJson> finished)
    {
        Debug.Log("Pinging " + IP_ADDRESS);
        using (UnityWebRequest www = UnityWebRequest.Get(IP_ADDRESS))
        {
            yield return www.SendWebRequest();

            lastServerStatus = StatusJson.CreateFromJSON(www.downloadHandler.text);

            Debug.Log(www.downloadHandler.text);

            if (finished != null)
            {
                finished(lastServerStatus);
            }
        }
        Debug.Log("Ping finished");
    }

    public IEnumerator ChangeLedStatus(int led, int status)
    {
        Debug.Log("Changing " + IP_ADDRESS + "Led" + led.ToString() + " to " + '"' + status.ToString() + '"');
        UnityWebRequest www = UnityWebRequest.Put(IP_ADDRESS + "Led" + led.ToString(), status.ToString());
        www.SetRequestHeader("Content-Type", "text/plain");


        Debug.Log(www.GetRequestHeader("Content-Type"));
        using (www)
        {
            www.chunkedTransfer = false;
            yield return www.SendWebRequest();
            Debug.Log("Server response: LED light" + led + " -> " + www.downloadHandler.text);
        }
    }

    public IEnumerator RefreshTemperature(Action<string> finished)
    {
        Debug.Log("Refreshing temperature...");
        using (UnityWebRequest www = UnityWebRequest.Get(IP_ADDRESS + "temperature"))
        {
            yield return www.SendWebRequest();

            var newTemperature = www.downloadHandler.text;
            Debug.Log("New temperature: " + newTemperature);

            if (finished != null)
            {
                finished(newTemperature);
            }
        }
    }

    public IEnumerator RefreshHumidity(Action<string> finished)
    {
        Debug.Log("Refreshing humidity...");
        using (UnityWebRequest www = UnityWebRequest.Get(IP_ADDRESS + "humidity"))
        {
            yield return www.SendWebRequest();

            var newHumidity = www.downloadHandler.text;
            Debug.Log("New humidity: " + newHumidity);

            if (finished != null)
            {
                finished(newHumidity);
            }
        }
    }

    public void Refresh(Action<StatusJson> finished)
    {
        StartCoroutine(PingSensors((statusJson) =>
        {
            if (finished != null)
            {
                finished(statusJson);
            }
            else
            {
                List<ISelfUpdate> selfUpdateComponents = (List<ISelfUpdate>)InterfaceHelper.FindObjects<ISelfUpdate>();
                foreach (ISelfUpdate component in selfUpdateComponents)
                {
                    component.SelfUpdate(statusJson);
                }
            }
        }));
    }

    IEnumerator GetAccessToken()
    {
        // create client instance 
        client = new MqttClient(IPAddress.Parse("150.140.5.20"), 80, false, null);
        yield return client;

        // register to message received 
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        ParseAndSubscribe("http://0013a200409c168b/0xd13/temp,0013a200409c168b/0xd13/sound,0013a200409c168b/0xd13/pir,0013a200409c168b/0xd13/light,0013a200409c168b/0xd13/humid");

        // subscribe to the topic
        //client.Subscribe(new string[] { "#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Message received, topic: " + e.Topic + " , message: " + e.Message);
    }

    public void ParseAndSubscribe(String text)
    {
        if (client == null)
        {
            Debug.Log("Mqqt client is null, cannot parse and subscribe");
            return;
        }

        text = text.Replace("http://", "");
        string[] sensors = text.Split(',');

        HashSet<string> topics = new HashSet<string>();

        foreach (string sensor in sensors)
        {
            var topic = sensor.Split('/')[0];
            Debug.Log(topic);
            topics.Add(topic.Trim());
        }

        String[] stringArray = new String[topics.Count];
        topics.CopyTo(stringArray);

        client.Subscribe(stringArray, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
    }
}
