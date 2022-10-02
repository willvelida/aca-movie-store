@description('The location to deploy our resources to. Default is location of resource group')
param location string = resourceGroup().location

@description('The name of our application.')
param applicationName string = uniqueString(resourceGroup().id)

@description('The name of the Azure Container Registry')
param containerRegistryName string = 'acr${applicationName}'

@description('The name of the Container App Environment')
param containerEnvironmentName string = 'env${applicationName}'

@description('The name of the Key Vault')
param keyVaultName string = 'kv${applicationName}'

@description('The name of the App Insights workspace')
param appInsightsName string = 'appins${applicationName}'

@description('The container image used by the web app')
param webAppImage string

// Movie Web App variables
var movieWebAppName = 'movie-web'
var movieWebAppCpu = '0.5'
var movieWebAppMemory = '1'
var movieWebAppEnv = [
  {
    name: 'ASPNETCORE_ENVIRONMENT'
    value: 'Development'
  }
  {
    name: 'APPINSIGHTS_CONNECTION_STRING'
    value: appInsights.properties.ConnectionString
  }
  {
    name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
    value: appInsights.properties.InstrumentationKey
  }
  {
    name: 'CatalogApi'
    value: 'https://${catalogApp.properties.configuration.ingress.fqdn}'
  }
]

var catalogApiName = 'movie-catalog'

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2022-02-01-preview' existing = {
  name: containerRegistryName
}

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

resource env 'Microsoft.App/managedEnvironments@2022-03-01' existing = {
  name: containerEnvironmentName
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: appInsightsName
}

resource catalogApp 'Microsoft.App/containerApps@2022-03-01' existing = {
  name: catalogApiName
} 

module storeApp 'br:acr5e7hkjc24cx2u.azurecr.io/modules/http-container-app:e643e49796bac2b13f11df32514aba5b04c4da4b' = {
  name: 'store-app'
  params: {
    acrPasswordSecret: keyVault.getSecret('acr-primary-password') 
    acrServerName: containerRegistry.properties.loginServer
    acrUsername: keyVault.getSecret('acr-username')
    containerAppEnvId: env.id
    containerAppName: movieWebAppName
    containerImage: webAppImage
    isExternal: true
    location: location
    cpuCore: movieWebAppCpu
    memorySize: movieWebAppMemory
    envVariables: movieWebAppEnv
  }
}
