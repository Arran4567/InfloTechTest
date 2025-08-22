resource "azurerm_resource_group" "main" {
  name     = "rg-techtest-${var.environment}"
  location = var.azure_location
}

resource "azurerm_storage_account" "function" {
  name                     = "techtestfunc${var.environment}"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_service_plan" "main" {
  name                = "asp-techtest-${var.environment}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  os_type             = "Windows"
  sku_name            = "B1"
}

resource "azurerm_windows_function_app" "main" {
  name                = "${var.environment}-techtest-api"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location

  storage_account_name       = azurerm_storage_account.function.name
  storage_account_access_key = azurerm_storage_account.function.primary_access_key
  service_plan_id            = azurerm_service_plan.main.id

  site_config {
    application_stack {
      dotnet_version = "v9.0"
    }
  }

  app_settings = {
    "DOTNET_VERSION"           = "v9.0"
    "FUNCTIONS_WORKER_RUNTIME" = "dotnet-isolated"
    "ENVIRONMENT"              = var.environment
  }

  zip_deploy_file = "techtest-api.zip"
}

output "function_app_url" {
  value = "https://${azurerm_windows_function_app.main.default_hostname}/"
}
