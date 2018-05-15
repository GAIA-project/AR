from flask import Flask
from flask import jsonify
from flask import request
import random
import grovepi
import light
import math
import time
from threading import Thread
import threading
exitapp=False

app = Flask(__name__)

_motor = 0
_led_1 = 0
_led_2 = 0
_led_3 = 0

#sensor value variables
luminosity=0
temperature=0
humidity=0
motion=0

#Pins declaration
pir_sensor = 7
temHum_sensor = 8
pin1 = [2, 4, 6]
pin2 = [3, 5, 7]

grovepi.pinMode(pir_sensor,"INPUT")
for i in [0, 1]:
    grovepi.pinMode(pin1[i], "OUTPUT")
    grovepi.pinMode(pin2[i], "OUTPUT")



#def pir(arg):
# 	global motion, exitapp
#	while not exitapp:
#		print "Pir"
#		try:
        		# Sense motion, usually human, within the target range
#			value=grovepi.digitalRead(pir_sensor)
#                	if value==0 or value==1:      # check if reads were 0 or 1 it$
#                        	if value==1:
#					print value
#					motion=1
#                       	 # if your hold time is less than this, you might not se$
#                	time.sleep(.2)

#       	 	except IOError:
#	                print ("Error")
#	print "The thread finish"



def getData():
	global luminosity,humidity,temperature
	#temperature & humidity v1.2 blue grovepi sensor
	[temp,hum] = grovepi.dht(temHum_sensor,0)
	if math.isnan(temp) == False and math.isnan(hum) == False:
		temperature=temp
		humidity=hum
	#Digital light grovepi sensor
	luminosity=light.readVisibleLux()


@app.route("/ping")
def ping():
    return "pong"


@app.route("/")
def hello():
    global luminosity,humidity,temperature,motion	
    getData()

    data = {}
    data["Temperature"] = temperature
    data["Luminosity"] = luminosity
    data["Motion"] = motion
    data["Humidity"] = humidity
    data["Led1"] = _led_1
    data["Led2"] = _led_2
    data["Led3"] = _led_3
    data["Motor"] = _motor
    
    motion=0	
    return jsonify(data)


@app.route("/Led1", methods=['PUT'])
def put_Led_1():
    global _led_1
    print "got:", request.data
    _led_1 = int(request.data)
    if _led_1==1:
	grovepi.digitalWrite(pin1[0], 1)
        grovepi.digitalWrite(pin2[0], 0)
    else:
	grovepi.digitalWrite(pin1[0], 0)
        grovepi.digitalWrite(pin2[0], 0)		
    return request.data


@app.route("/Led2", methods=['PUT'])
def put_Led_2():
    global _led_2
    print "got:", request.data
    _led_2 = int(request.data)
    if _led_2==1:
	grovepi.digitalWrite(pin1[1], 1)
        grovepi.digitalWrite(pin2[1], 0)
    else:
	grovepi.digitalWrite(pin1[1], 0)
        grovepi.digitalWrite(pin2[1], 0)		

    return request.data


@app.route("/Led3", methods=['PUT'])
def put_Led_3():
    global _led_3
    print "got:", request.data
    _led_3 = int(request.data)
    if _led_3==1:
	grovepi.digitalWrite(pin1[2], 1)
        grovepi.digitalWrite(pin2[2], 0)
    else:
	grovepi.digitalWrite(pin1[2], 0)
        grovepi.digitalWrite(pin2[2], 0)		

    return request.data


@app.route("/Motor", methods=['PUT'])
def put_Motor():
    global _motor
    print "got:", request.data
    _motor = int(request.data)
    return request.data


#def main():
#	thread = Thread(target=pir, args=(10,))
#	thread.start()
app.run(host='0.0.0.0', port=5000, debug=True)

#try:
#    main()
#except KeyboardInterrupt:
#    exitapp = True
#    raise



