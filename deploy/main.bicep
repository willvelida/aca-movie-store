@description('The location to deploy our resources to. Default is location of resource group')
param location string = resourceGroup().location

@description('The name of our application.')
param applicationName string = uniqueString(resourceGroup().id)

@description('The name of the Azure Container Registry')
param containerRegistryName string = 'acr${applicationName}'

@description('The name of the Key Vault')
param keyVaultName string = 'kv${applicationName}'

@description('The name of the Log Analytics workspace')
param logAnalyticsWorkspaceName string = 'law${applicationName}'

@description('The name of the App Insights workspace')
param appInsightsName string = 'appins${applicationName}'

@description('The name of the Container App Environment')
param containerEnvironmentName string = 'env${applicationName}'

@description('The name of the SQL Server instance')
param sqlServerName string = 'sqldb${applicationName}'

@description('The SQL Admin Login username')
param sqlAdminUsername string

@description('The SQL Admin Password')
param sqlAdmin string

@description('The container image used by the web app')
param webAppImage string

@description('The container image used by the Catalog API')
param catalogApiImage string

// General Variables
var movieDatabaseName = 'Movie'

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
    value: 'https://${catalogApp.outputs.fqdn}'
  }
]

// Catalog API variables
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
    value: 'Server=tcp:${sql.outputs.sqlServerName}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=${sql.outputs.sqlDbName};Persist Security Info=False;User ID=${sql.outputs.sqlAdminLogin};Password=${sqlAdmin};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
  }
]

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: keyVaultName
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: tenant().tenantId
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enableSoftDelete: false
    accessPolicies: [
    ]
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalytics.outputs.logAnalyticsId
  }
}

module logAnalytics 'modules/logAnalytics.bicep' = {
  name: 'log-analytics'
  params: {
    keyVaultName: keyVault.name
    location: location
    logAnalyticsWorkspaceName: logAnalyticsWorkspaceName
  }
}

module containerRegistry 'modules/containerRegistry.bicep' = {
  name: 'acr'
  params: {
    containerRegistryName: containerRegistryName
    keyVaultName: keyVault.name
    location: location
  }
}

module sql 'modules/sqlServer.bicep' = {
  name: 'sql'
  params: {
    adminLogin: sqlAdmin
    adminLoginUserName: sqlAdminUsername
    keyVaultName: keyVault.name
    location: location
    sqlDatabaseName: movieDatabaseName
    sqlServerName: sqlServerName
  }
}

module env 'modules/containerAppEnvironment.bicep' = {
  name: 'container-app-env'
  params: {
    containerEnvironmentName: containerEnvironmentName
    location: location
    logAnalyticsCustomerId: logAnalytics.outputs.customerId 
    logAnalyticsSharedKey: keyVault.getSecret('log-analytics-shared-key')
  }
}

module storeApp 'modules/httpContainerApp.bicep' = {
  name: 'store-app'
  params: {
    acrPasswordSecret: keyVault.getSecret('acr-primary-password') 
    acrServerName: containerRegistry.outputs.loginServer
    acrUsername: keyVault.getSecret('acr-username')
    containerAppEnvId: env.outputs.containerAppEnvId
    containerAppName: movieWebAppName
    containerImage: webAppImage
    isExternal: true
    location: location
    cpuCore: movieWebAppCpu
    memorySize: movieWebAppMemory
    envVariables: movieWebAppEnv
  }
}

module catalogApp 'modules/httpContainerApp.bicep' = {
  name: 'catalog-app'
  params: {
    acrPasswordSecret: keyVault.getSecret('acr-primary-password') 
    acrServerName: containerRegistry.outputs.loginServer
    acrUsername: keyVault.getSecret('acr-username')
    containerAppEnvId: env.outputs.containerAppEnvId
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
          path: '/healthz'
          port: 8080
        }
        initialDelaySeconds: 7
        periodSeconds: 3
      }
    ]
  }
}
