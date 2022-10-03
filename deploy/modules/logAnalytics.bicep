@description('The name of the Log Analytics workspace')
param logAnalyticsWorkspaceName string

@description('The location to deploy our resources to. Default is location of resource group')
param location string

@description('The name of the Key Vault')
param keyVaultName string

@description('The tags to apply to this Log Analytics resource')
param tags object

var sharedKeySecretName = 'log-analytics-shared-key'

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  name: logAnalyticsWorkspaceName
  location: location
  tags: tags
  properties: {
   retentionInDays: 30
   features: {
    searchVersion: 1
   }
   sku: {
    name: 'PerGB2018'
   } 
  }
}

resource sharedKeySecret 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  name: sharedKeySecretName
  parent: keyVault
  properties: {
    value: logAnalytics.listKeys().primarySharedKey
  }
}

output logAnalyticsId string = logAnalytics.id
output customerId string = logAnalytics.properties.customerId
