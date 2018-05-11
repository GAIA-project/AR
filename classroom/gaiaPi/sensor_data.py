from flask import Flask
from flask import jsonify
from flask import request
import random

app = Flask(__name__)

_motor = 0
_led_1 = 0
_led_2 = 0
_led_3 = 0


@app.route("/ping")
def ping():
    return "pong"


@app.route("/")
def hello():
    data = {}
    data["Temperature"] = random.randint(10, 40)
    data["Luminosity"] = random.randint(0, 200)
    data["Motion"] = random.randint(0, 1)
    data["Humidity"] = random.randint(0, 100)
    data["Led/1"] = _led_1
    data["Led/2"] = _led_2
    data["Led/3"] = _led_3
    data["Motor"] = _motor
    return jsonify(data)


@app.route("/Led/1", methods=['PUT'])
def put_Led_1():
    global _led_1
    print "got:", request.data
    _led_1 = int(request.data)
    return request.data


@app.route("/Led/2", methods=['PUT'])
def put_Led_2():
    global _led_2
    print "got:", request.data
    _led_2 = int(request.data)
    return request.data


@app.route("/Led/3", methods=['PUT'])
def put_Led_3():
    global _led_3
    print "got:", request.data
    _led_3 = int(request.data)
    return request.data


@app.route("/Motor", methods=['PUT'])
def put_Motor():
    global _motor
    print "got:", request.data
    _motor = int(request.data)
    return request.data


app.run(debug=True)
