#!/bin/bash -e
. ./scripts/web/set-env.sh $1

# Publish the function app to a folder
dotnet publish UserManagement.UI/UserManagement.UI.csproj \
    -c Release \
    -o ./publish

# Create a ZIP from the publish folder
cd ./publish/wwwroot
zip -r ../../techtest-web.zip .
cd ../..
cp techtest-web.zip $TERRAGRUNT_DIRECTORY/