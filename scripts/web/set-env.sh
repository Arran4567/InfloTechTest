#!/bin/bash -e

echo "Setting deployment variables"

export ENVIRONMENT_NAME=$1
export PROJECT_NAME="techtest"

export TERRAGRUNT_DIRECTORY="deploy/regional/web"
export PLAN_FILE_PATH="/tmp/${AWS_DEFAULT_REGION}-${PROJECT_NAME}.tfplan"
export ARTIFACTS_DIR="${PWD}/build"
export TF_VAR_environment_name=${ENVIRONMENT_NAME}
export TF_VAR_environment=$1
mkdir -p $ARTIFACTS_DIR