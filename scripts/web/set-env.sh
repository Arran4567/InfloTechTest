#!/bin/bash -e

echo "Setting deployment variables"

export ENVIRONMENT_NAME=$1
export PROJECT_NAME="techtestweb"

export TERRAGRUNT_DIRECTORY="deploy/regional/web"
export PLAN_FILE_PATH="/tmp/${PROJECT_NAME}.tfplan"
export ARTIFACTS_DIR="${PWD}/build"
export TF_VAR_environment_name=${ENVIRONMENT_NAME}
export TF_VAR_environment=$1
export TF_VAR_azure_subscription_id=${AZURE_SUBSCRIPTION_ID}
export TF_VAR_azure_client_id=${AZURE_CLIENT_ID}
export TF_VAR_azure_client_secret=${AZURE_CLIENT_SECRET}
export TF_VAR_azure_tenant_id=${AZURE_TENANT_ID}
export TF_VAR_azure_location="ukwest"
mkdir -p $ARTIFACTS_DIR