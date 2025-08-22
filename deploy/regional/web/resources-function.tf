resource "azurerm_resource_group" "main" {
  name     = "rg-techtestweb-${var.environment}"
  location = var.azure_location
}

resource "azurerm_service_plan" "main" {
  name                = "asp-techtestweb-${var.environment}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  os_type             = "Windows"
  sku_name            = "B1"
}

resource "azurerm_windows_web_app" "main" {
  name                = "${var.environment}-techtest-web"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  service_plan_id     = azurerm_service_plan.main.id

  site_config {
    application_stack {
      dotnet_version = "v9.0"
      current_stack  = "dotnet"
    }
  }

  app_settings = {
    "ASPNETCORE_ENVIRONMENT" = var.environment
    "DOTNET_VERSION"         = "v9.0"
  }

  zip_deploy_file = "techtest-web.zip"
}

output "app_service_url" {
  value = "https://${azurerm_windows_web_app.main.default_hostname}/"
}