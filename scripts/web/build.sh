#!/bin/bash -e
. ./scripts/web/set-env.sh $1

# Publish the function app to a folder
dotnet publish UserManagement.sln \
    -c Release \
    -o ./publish

# Create a ZIP from the publish folder
cd ./publish
zip -r ../techtest-web.zip .
cd ..
cp techtest-web.zip $TERRAGRUNT_DIRECTORY/