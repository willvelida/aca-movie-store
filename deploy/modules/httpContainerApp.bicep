@description('The name of the container app')
param containerAppName string

@description('The location to deploy the Container App')
param location string

@description('The ContainerApp Environment Id')
param containerAppEnvId string

@description('The container image that this app will use')
param containerImage string

@description('The ACR server name')
param acrServerName string

@description('The ACR username')
@secure()
param acrUsername string

@description('The ACR password secret')
@secure()
param acrPasswordSecret string

@description('Is this app external')
param isExternal bool

@description('The environment variables for this container app')
param envVariables array = []

@description('The amount of CPU cores the container can use. Can be with a maximum of two decimals')
param cpuCore string

@description('The amount of memory (in gibibytes, GiB) allocated to the container app')
param memorySize string

@description('The probes to apply in this Container App')
param healthProbes array = []

@description('The tags to apply to this Container App')
param tags object

resource containerApp 'Microsoft.App/containerApps@2022-03-01' = {
  name: containerAppName
  location: location
  tags: tags
  properties: {
   managedEnvironmentId: containerAppEnvId
   configuration: {
    activeRevisionsMode: 'Multiple'
    ingress: {
      external: isExternal
      transport: 'http'
      targetPort: 80
      allowInsecure: false
      traffic: [
        {
          latestRevision: true
          weight: 100
        }
      ]
    }
    secrets: [
      {
        name: 'container-registry-password'
        value: acrPasswordSecret
      }
    ]
    registries: [
      {
        server: acrServerName
        username: acrUsername
        passwordSecretRef: 'container-registry-password'
      }
    ]
   }
   template: {
    containers: [
      {
        name: containerAppName
        image: containerImage
        env: envVariables
        probes: healthProbes
        resources: {
          cpu: json(cpuCore)
          memory: '${memorySize}Gi'
        }
      }
    ]
    scale: {
      minReplicas: 1
      maxReplicas: 30
      rules: [
        {
          name: 'http-rule'
          http: {
            metadata: {
              concurrentRequests: '100'
            }
          }
        }
      ]
    }
   } 
  }
  identity: {
    type: 'SystemAssigned'
  }
}

output fqdn string = containerApp.properties.configuration.ingress.fqdn
