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

@description('The name of the SQL Server instance')
param sqlServerName string = 'sqldb${applicationName}'

@description('The name of the SQL Database')
param sqlDatabaseName string = 'Movie'

@description('The container image used by the Catalog API')
param catalogApiImage string

@description('The administrator username of the SQL logical server')
param adminLoginUserName string

@description('The SQL Admin Password')
param sqlAdmin string

var catalogApiName = 'movie-catalog'
var catalogApiCpu = '0.5'
var catalogApiMemory = '1'
var catlogApiEnv = [
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
    name: 'AZURE_SQL_CONNECTIONSTRING'
    value: 'Server=tcp:${sql.name}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=${sqlDatabaseName};Persist Security Info=False;User ID=${adminLoginUserName};Password=${sqlAdmin};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
  }
]

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

resource sql 'Microsoft.Sql/servers@2022-02-01-preview' existing = {
  name: sqlServerName
}

module catalogApp 'br:acr5e7hkjc24cx2u.azurecr.io/modules/http-container-app:e643e49796bac2b13f11df32514aba5b04c4da4b' = {
  name: 'catalog-app'
  params: {
    acrPasswordSecret: keyVault.getSecret('acr-primary-password') 
    acrServerName: containerRegistry.properties.loginServer
    acrUsername: keyVault.getSecret('acr-username')
    containerAppEnvId: env.id
    containerAppName: catalogApiName
    containerImage: catalogApiImage
    cpuCore: catalogApiCpu
    isExternal: false
    location: location
    memorySize: catalogApiMemory
    envVariables: catlogApiEnv
    healthProbes: [
      {
        type: 'liveness'
        httpGet: {
          path: '/healthz/liveness'
          port: 80
        }
        initialDelaySeconds: 15
        periodSeconds: 30
        failureThreshold: 3
        timeoutSeconds: 1
      }
    ]
  }
}
