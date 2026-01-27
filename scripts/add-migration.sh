#!/usr/bin/env bash
# Usage: ./scripts/add-migration.sh <NAME>

if [ -z "$1" ]; then
  echo "Migration name required"
  exit 1
fi

MIGRATION_NAME=$1

dotnet ef migrations add "$MIGRATION_NAME" \
  --project ./src/eShop.Infrastructure/eShop.Infrastructure.csproj \
  --startup-project ./src/eShop.Web/eShop.Web.csproj \
  --context ApplicationDbContext \
  --output-dir Presistence/Migrations \
  --namespace eShop.Infrastructure.Presistence.Migrations
