#!/bin/bash -e
. ./scripts/api/set-env.sh $1

# Publish the function app to a folder
dotnet publish UserManagement.Api/UserManagement.Api.csproj \
    -c Release \
    -o ./publish

# Create a ZIP from the publish folder
cd ./publish
zip -r ../techtest-api.zip .
cd ..
cp techtest-api.zip $TERRAGRUNT_DIRECTORY/