@description('The name of the Container App Environment')
param containerEnvironmentName string

@description('The location to deploy our resources to. Default is location of resource group')
param location string

@description('The Log Analytics Customer Id')
param logAnalyticsCustomerId string

@description('The Log Analytics Shared Key')
@secure()
param logAnalyticsSharedKey string

@description('Tags applied to this resource')
param tags object

resource env 'Microsoft.App/managedEnvironments@2022-03-01' = {
  name: containerEnvironmentName
  location: location
  tags: tags
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsCustomerId
        sharedKey: logAnalyticsSharedKey
      }
    }
  }
}

output containerAppEnvId string = env.id
