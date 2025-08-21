variable "environment" {
  type = string
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