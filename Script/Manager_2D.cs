using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class Manager_2D : MonoBehaviour
{

    public InputField brokerInputField;
    public InputField userInputField;
    public InputField passInputField;

    public static string username;
    public static string password;
    public static string broker;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkSubmit()
    {
        if(userInputField.text == "bkiot" && passInputField.text == "12345678" && brokerInputField.text == "mqttserver.tk")
        {
            username = "bkiot";
            password = "12345678";
            broker = "mqttserver.tk";
            SceneManager.LoadScene("Screen2");
        }
    }


}
