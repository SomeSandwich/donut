#! /bin/sh

set -e

export VAULT_ADDR=http://vault:8200

# give some time for Vault to start and be ready
sleep 3

# login with root token at $VAULT_ADDR
vault login root-token

vault kv put -mount=secret "ConnectionStrings" "LinkDB=mongodb://admin:admin@nosqldata/LinkDB?authSource=admin" "Cache=cachedata:6379,abortConnect=false"
vault kv put -mount=secret "MessageQueue" "Host=messagequeue" "Port=5672" "VHost=/" "Username=guest" "Password=guest"
