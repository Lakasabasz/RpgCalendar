import requests
from requests.auth import HTTPBasicAuth
import json
import os
graylog_base_uri = os.environ['GRAYLOG_HTTP_EXTERNAL_URI']
graylog_inputs_uri = graylog_base_uri + "/api/system/inputs"
username = os.environ['GRAYLOG_ROOT_USERNAME']
password = os.environ['GRAYLOG_ROOT_PASSWORD']

headers = {
    "Content-Type": "application/json",
    "X-Requested-By": "python-script"
}

input_data = {
    "title": "default",
    "type": "org.graylog2.inputs.gelf.tcp.GELFTCPInput",
    "configuration": {
        "bind_address": "0.0.0.0",
        "port": 12201,
        "recv_buffer_size": 1048576,
        "charset_name": "UTF-8",
        "force_rdns": False,
        "allow_override_date": True
    },
    "global": True

}

checkInputs = requests.get(
    graylog_inputs_uri,
    auth=HTTPBasicAuth(username, password),
    headers=headers
)
if checkInputs.status_code == 200 and checkInputs.json()['total'] == 0:
    response = requests.post(
        graylog_inputs_uri,
        auth=HTTPBasicAuth(username, password),
        headers=headers,
        data=json.dumps(input_data)
    )

    # Sprawdzenie odpowiedzi
    if response.status_code == 201:
        print("Input zostało pomyślnie utworzone!")
    else:
        print(f"Coś poszło nie tak: {response.status_code}")
        print(response.text)
elif checkInputs.json()['total'] != 0:
    print("Inputs already exists.")
else:
    print(f"GET inputs: Something went wrong. {checkInputs.status_code} \n {checkInputs.text()}")
    exit(1)