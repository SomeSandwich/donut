# Export Keycloak Realm to a File
- For Windows users: Please run this process in WSL (Windows Subsystem for Linux).

> Note: If your source directory is located at `C:\code\donut`, the corresponding path in WSL will be `/mnt/c/code/donut`.

## Steps
1. List all Docker networks and identify the network name used by Docker Compose
- Run the following command to list the networks:
```sh
docker network ls
```
2. Run the export command
- Replace `{{ enter the network on docker compose }}` with the actual Docker network name and execute the following command:
```sh
docker run --rm \
    --name keycloak_exporter \
    --network {{ enter the network on docker compose }} \
    -v ./keycloak/:/tmp/keycloak-export \
    -e KC_DB=postgres \
    -e KC_DB_PASSWORD=admin \
    -e KC_DB_USERNAME=admin \
    -e KC_DB_URL=jdbc:postgresql://postgres:5432/admin \
    quay.io/keycloak/keycloak:latest \
    export \
    --realm somesandwich-donut \
    --dir /tmp/keycloak-export \
    --users realm_file
```
