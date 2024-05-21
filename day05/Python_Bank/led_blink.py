# led_blink.py
# Red led

import RPi.GPIO as GPIO
import time


# RGP LED PinNumber setting
red_pin = 4
green_pin = 6
blue_pin = 5

GPIO.setmode(GPIO.BCM) # GPIO.BORD(1~40)
GPIO.setup(red_pin, GPIO.OUT) # 4번 pin으로 출력
GPIO.setup(green_pin, GPIO.OUT)
GPIO.setup(blue_pin, GPIO.OUT)

try:
    while(True):
        GPIO.output(red_pin, True)
        GPIO.output(green_pin, False)
        GPIO.output(blue_pin, False)
        time.sleep(0.5) # 0.5sec

        # GPIO.output(red_pin, False)
        # GPIO.output(green_pin, True)
        # GPIO.output(blue_pin, False)        
        # time.sleep(0.5)

        # GPIO.output(red_pin, False)
        # GPIO.output(green_pin, False)
        # GPIO.output(blue_pin, True)        
        # time.sleep(0.5)

        GPIO.output(red_pin, True)
        GPIO.output(green_pin, True)
        GPIO.output(blue_pin, True)        
        time.sleep(0.5)

except KeyboardInterrupt:
    GPIO.cleanup()