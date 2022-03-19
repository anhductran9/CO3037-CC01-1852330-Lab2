print("IoT Gateway")
import paho.mqtt.client as mqttclient
import time
import json

BROKER_ADDRESS = "mqttserver.tk"
PORT = 1883
#THINGS_BOARD_ACCESS_TOKEN = "YpklfSJL5TwL6h1bjLId"


def subscribed(client, userdata, mid, granted_qos):
    print("Subscribed...")


def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))
    temp_data = {'value': True}
    try:
        jsonobj = json.loads(message.payload)
        if jsonobj['method'] == "setValue":
            temp_data['value'] = jsonobj['params']
            client.publish('/bkiot/1852330/status', json.dumps(temp_data), qos=0, retain=True)
    except:
        pass


def connected(client, usedata, flags, rc):
    if rc == 0:
        print("Connected successfully")
        client.subscribe("/bkiot/1852330/status", qos=0)
    else:
        print("Connection is failed")


client = mqttclient.Client("1852330")
client.username_pw_set(username="bkiot",password="12345678")

client.on_connect = connected
client.connect(BROKER_ADDRESS, 1883)
client.loop_start()

client.on_subscribe = subscribed
client.on_message = recv_message

temp = 30
humi = 50


while True:
    collect_data = {'temperature': temp, 'humidity': humi}
    temp += 1
    humi += 1
    client.publish('/bkiot/1852330/status', json.dumps(collect_data), qos=0, retain=True)
    time.sleep(10)
