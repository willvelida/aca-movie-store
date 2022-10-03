@description('The name of the Azure Container Registry')
param containerRegistryName string

@description('The location to deploy our resources to. Default is location of resource group')
param location string

@description('The name of the Key Vault')
param keyVaultName string

@description('Tags applied to this resource')
param tags object

var primaryPasswordSecret = 'acr-primary-password'
var secondaryPasswordSecret = 'acr-secondary-password'
var usernameSecret = 'acr-username'

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2022-02-01-preview' = {
  name: containerRegistryName
  location: location
  tags: tags
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource acrPasswordSecret1 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  name: primaryPasswordSecret
  parent: keyVault
  properties: {
    value: containerRegistry.listCredentials().passwords[0].value
  }
}

resource acrPasswordSecret2 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  name: secondaryPasswordSecret
  parent: keyVault
  properties: {
    value: containerRegistry.listCredentials().passwords[1].value
  }
}

resource acrUsername 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  name: usernameSecret
  parent: keyVault
  properties: {
    value: containerRegistry.listCredentials().username
  }
}

output loginServer string = containerRegistry.properties.loginServer
output containerRegistryPrincipalId string = containerRegistry.identity.principalId
