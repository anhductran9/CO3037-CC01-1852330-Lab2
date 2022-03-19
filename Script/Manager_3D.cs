using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class Manager_3D : MonoBehaviour
{
    public Text celciusText;
    public Text percentText;
    public Toggle ledToggle;
    public Toggle pumpToggle;
    public MqttClient client;

    public string username;
    public string password;
    public string broker;

    private List<MqttMsgPublishEventArgs> messageList = new List<MqttMsgPublishEventArgs>();

    // Start is called before the first frame update
    void Start()
    {
        username = Manager_2D.username;
        password = Manager_2D.password;
        broker = Manager_2D.broker;
        client = new MqttClient("mqttserver.tk",1883, false, null, null, MqttSslProtocols.None);
        StartCoroutine(Connecting());
    }

    // Update is called once per frame
    void Update()
    {
        if(messageList.Count > 0)
        {
            DecodeMessage(messageList[0].Topic, messageList[0].Message);
            messageList.RemoveAt(0);
        }
    }

    private IEnumerator Connecting()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId, username, password);
        client.MqttMsgPublishReceived += OnMqttMessageReceived;
        client.Subscribe(new string[] { "/bkiot/1852330/status" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    private void OnMqttMessageReceived(object sender, MqttMsgPublishEventArgs msg)
    {
        messageList.Add(msg);
    }

    void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        Todo todo = JsonConvert.DeserializeObject<Todo>(msg);
        celciusText.text = todo.temperature;
        percentText.text = todo.humidity;
    }

    class Todo
    {
        public string temperature { get; set; }
        public string humidity { get; set; }
    }

    public void ToggleLed()
    {
        if(ledToggle.isOn)
        {
            Debug.Log("{\"device\":\"LED\",\"status\":\"ON\"}");
            client.Publish("/bkiot/1852330/led", System.Text.Encoding.UTF8.GetBytes("{\"device\":\"LED\",\"status\":\"ON\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }
        else
        {
            Debug.Log("{\"device\":\"LED\",\"status\":\"OFF\"}");
            client.Publish("/bkiot/1852330/led", System.Text.Encoding.UTF8.GetBytes("{\"device\":\"LED\",\"status\":\"OFF\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }
    }

    public void TogglePump()
    {
        if(pumpToggle.isOn)
        {
            Debug.Log("{\"device\":\"PUMP\",\"status\":\"ON\"}");
            client.Publish("/bkiot/1852330/pump", System.Text.Encoding.UTF8.GetBytes("{\"device\":\"PUMP\",\"status\":\"ON\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }
        else
        {
            Debug.Log("{\"device\":\"PUMP\",\"status\":\"OFF\"}");
            client.Publish("/bkiot/1852330/pump", System.Text.Encoding.UTF8.GetBytes("{\"device\":\"PUMP\",\"status\":\"OFF\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }
    }
}