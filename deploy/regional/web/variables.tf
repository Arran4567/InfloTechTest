variable "environment" {
  type = string
}

variable "azure_location" {
  type = string
}

variable "azure_subscription_id" {
  type        = string
  description = "Azure subscription ID to use for the provider"
}

variable "azure_client_id" {
  type        = string
  description = "Azure Service Principal Client ID"
}

variable "azure_client_secret" {
  type        = string
  sensitive   = true
  description = "Azure Service Principal Client Secret"
}

variable "azure_tenant_id" {
  type        = string
  description = "Azure Tenant ID"
}