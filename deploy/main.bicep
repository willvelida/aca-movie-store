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

// General Variables
var movieDatabaseName = 'Movie'

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
