@description('The name of the logical SQL Server')
param sqlServerName string

@description('The name of the SQL Database')
param sqlDatabaseName string

@description('The location to deploy our resource')
param location string

@description('The administrator username of the SQL logical server')
param adminLoginUserName string

@description('The admin password of the SQL logical server')
param adminLogin string

@description('The name of the Key Vault to store the SQL secrets')
param keyVaultName string

@description('The tags to apply to the SQL Server')
param tags object

var sqlConnectionString = 'sql-connection-string'

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

resource sqlServer 'Microsoft.Sql/servers@2022-02-01-preview' = {
  name: sqlServerName
  tags: tags
  location: location
  properties: {
   administratorLogin: adminLoginUserName
   administratorLoginPassword: adminLogin 
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource firewallRules 'Microsoft.Sql/servers/firewallRules@2022-02-01-preview' = {
  name: 'AllowAllIps'
  parent: sqlServer
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'   
  }
}

resource sqlDb 'Microsoft.Sql/servers/databases@2022-02-01-preview' = {
  name: sqlDatabaseName
  parent: sqlServer
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
}

resource sqlConnectionStringSecret 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  name: sqlConnectionString
  parent: keyVault
  properties: {
    value: 'Server=tcp:${sqlServer.name}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=${sqlDb.name};Persist Security Info=False;User ID=${sqlServer.properties.administratorLogin};Password=${adminLogin};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
  }
}

output sqlServerName string = sqlServer.name
output sqlDbName string = sqlDb.name
output sqlAdminLogin string = sqlServer.properties.administratorLogin
