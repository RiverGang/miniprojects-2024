# mqtt_realapp.py
# 온습도센서데이터 통신, RGB LED Setting
# MQTT -> json 형태로 전환

# mqtt_simple.py
import paho.mqtt.client as mqtt
import RPi.GPIO as GPIO
import adafruit_dht
import board
import time
import datetime as dt
import json

red_pin = 4
green_pin = 6
dht_pin = 18

dev_id = 'PKNU72'
loop_num = 0

## 초기화 시작
def onConnect(client, userdata, flags, reason_code, properties):
    print(f'연결성공 : {reason_code}')
    client.subscribe('pknu/rcv')

def onMessage(client, userdata, msg):
    print(f'{msg.topic} +{msg.payload}')

GPIO.cleanup()
GPIO.setmode(GPIO.BCM)
    ## LED 설정, 출력형태
GPIO.setup(red_pin, GPIO.OUT)
GPIO.setup(green_pin, GPIO.OUT)
    ## 온습도(dht) 설정, RPi로부터 오는 센서 값은 입력형태
GPIO.setup(dht_pin, GPIO.IN)
dhtDevice = adafruit_dht.DHT11(board.D18) # 보드의 D18번

## 초기화 끝

## 실행시작
mqttc = mqtt.Client(mqtt.CallbackAPIVersion.VERSION2) # 2023.09 이후 버전업
mqttc.on_connect = onConnect # 접속시 이벤트
mqttc.on_message = onMessage # 메세지 전송

# 192.168.5.2 window ip, 본인의 ip 입력
mqttc.connect('192.168.5.2', 1883, 60)

mqttc.loop_start()
while True:
    time.sleep(2) # DHT11 2초 마다 갱신이 잘됨

    try:
        ## 온습도 값을 받아서 MQTT로 전송
        temp = dhtDevice.temperature
        humid = dhtDevice.humidity

        print(f'{loop_num} - Temp:{temp} / humid:{humid}')

        origin_data = { 'DEV_ID' : dev_id,
                        'CURR_DT' : dt.datetime.now().strftime('%Y-%m-%d %H:%M:%S'),
                        'TYPE' : 'TEMPHUMID',
                        'VALUE' : f'{temp}{humid}'
                        } # dictionary data
        pub_data = json.dumps(origin_data, ensure_ascii=False)

        mqttc.publish('pknu/data', pub_data)
        loop_num += 1
    
    except RuntimeError as ex:
        print(ex.args[0])
    except KeyboardInterrupt:
        break
mqttc.loop_stop()
dhtDevice.exit()
## 실행 끝