import os

import requests
import json

# Ustawienia Keycloak
keycloak_url = os.environ["KEYCLOAK_URL"]
realm_name = os.environ["KEYCLOAK_REALM_NAME"]
admin_username = os.environ["KEYCLOAK_ADMIN"]
admin_password = os.environ["KEYCLOAK_ADMIN_PASSWORD"]
client_id = "admin-cli"

# Endpointy API
token_url = f"{keycloak_url}/realms/master/protocol/openid-connect/token"
realm_url = f"{keycloak_url}/admin/realms"
realm_check_url = f"{realm_url}/{realm_name}"

# Pobranie tokenu dostępu
token_response = requests.post(
    token_url,
    data={
        "grant_type": "password",
        "client_id": client_id,
        "username": admin_username,
        "password": admin_password
    }
)

token_response.raise_for_status()
access_token = token_response.json()["access_token"]

# Nagłówki z tokenem
headers = {
    "Authorization": f"Bearer {access_token}",
    "Content-Type": "application/json"
}

# Sprawdzenie, czy realm już istnieje
realm_check_response = requests.get(realm_check_url, headers=headers)

if realm_check_response.status_code == 200:
    print(f"Realm '{realm_name}' już istnieje.")
elif realm_check_response.status_code == 404:
    print(f"Realm '{realm_name}' nie istnieje. Tworzenie nowego realm...")

    # Konfiguracja nowego realm
    realm_data = {
        "realm": realm_name,
        "enabled": True,
        "displayName": "My New Realm",
        "userManagedAccessAllowed": True,
        "sslRequired": "external",  # Inne opcje: "none", "all"
        # Dodatkowe opcje konfiguracji:
        "accessTokenLifespan": 300,  # Czas życia access tokenu w sekundach
        "ssoSessionIdleTimeout": 1800,  # Czas życia sesji SSO
        "ssoSessionMaxLifespan": 36000,  # Maksymalny czas życia sesji SSO
        "registrationAllowed": True,
        "loginWithEmailAllowed": True,
        "duplicateEmailsAllowed": False,
        "resetPasswordAllowed": True,
        "editUsernameAllowed": False,
        # Inne ustawienia według potrzeb
    }

    # Wysłanie żądania do Keycloak API w celu utworzenia realm
    create_realm_response = requests.post(
        realm_url,
        headers=headers,
        data=json.dumps(realm_data)
    )

    # Sprawdzenie odpowiedzi
    if create_realm_response.status_code == 201:
        print(f"Realm '{realm_name}' został pomyślnie utworzony!")
    else:
        print(f"Coś poszło nie tak podczas tworzenia realm: {create_realm_response.status_code}")
        print(create_realm_response.text)
else:
    print(f"Wystąpił błąd podczas sprawdzania stanu realm: {realm_check_response.status_code}")
    print(realm_check_response.text)
