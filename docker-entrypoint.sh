#!/bin/sh
set -e

# Persistent data (SQLite db, product uploads, data-protection keys) lives on the mounted disk.
DATA_DIR="${DATA_DIR:-/var/data}"
mkdir -p "$DATA_DIR/uploads" "$DATA_DIR/keys"

# Serve uploaded product images from the persistent disk.
rm -rf /app/wwwroot/uploads
ln -s "$DATA_DIR/uploads" /app/wwwroot/uploads

# Render provides PORT (default 10000); listen on plain http behind its TLS proxy.
export ASPNETCORE_URLS="http://+:${PORT:-10000}"

exec dotnet TextileShop.Web.dll
