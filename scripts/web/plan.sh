#!/bin/bash -e
. scripts/web/set-env.sh $1

echo $PLAN_FILE_PATH
echo $TERRAGRUNT_DIRECTORY

terragrunt plan -input=false \
    -out=$PLAN_FILE_PATH \
    --terragrunt-non-interactive \
    --terragrunt-working-dir $TERRAGRUNT_DIRECTORY