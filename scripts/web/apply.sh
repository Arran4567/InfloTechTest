#!/bin/bash -e
. ./scripts/web/set-env.sh $1

echo "Setting deployment variables"

cp $PLAN_FILE_PATH $TERRAGRUNT_DIRECTORY

terragrunt apply -auto-approve \
    $PLAN_FILE_PATH \
    --terragrunt-non-interactive \
    --terragrunt-working-dir $TERRAGRUNT_DIRECTORY