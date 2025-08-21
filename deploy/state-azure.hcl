remote_state {
    backend = "azurerm"
    config = {
        resource_group_name = "rg-terraform-state"
        storage_account_name = "techtesttfstate"
        container_name = "tfstate"
        key = "${path_relative_to_include()}/terraform.tfstate"
    }
}