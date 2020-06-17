Write-Output "Criando parâmetros de configuração no Consul..."
$Url = "http://192.168.0.102:8500/v1/kv/"
if($Args[0]) {
	$Url = $Args[0]
}
Write-Output "Usando API do consul em $Url" 
Write-Output ""

$Global = @'
{
  "loggingLevel": "Warning",
  "databaseLogging": "User ID=postgres;Password=W01fM0n1t0r;Host=10.0.75.1;Port=5432;Database=logs;",
  "logConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=LogContext;Persist Security Info=True;Integrated Security=True;",
  "healthCheckIntervalSegs": "10",
  "deregisterCriticalServiceAfterSegs": "10",
  "cors": "192.168.0.102:4200",
  "broker": {
    "hostname": "192.168.0.102",
    "exchangeName": "tottem"
  },
  "identityServerAddress": "http://192.168.0.102:16000"
}
'@

$Jobs = @'
{
  "tags": "Recurring jobs",
  "connectionString": "User ID=postgres;Password=W01fM0n1t0r;Host=10.0.75.1;Port=5433;Database=hangfire;",
  "jobs": [
    {
      "intervalMinutes": "15",      
      "targetServiceName": "Sample",
      "messageType": "InfoMessage"
    }
  ]
}
'@

$Gateway = @'
{
  "Tags": "Gateway,Hub,Services",
    "ReRoutes": [],
    "Aggregates": [],
    "GlobalConfiguration": {
        "RequestIdKey": null,
        "ServiceDiscoveryProvider": {
            "Host": "192.168.0.102",
            "Port": 8500,
            "Type": "Consul",
            "Token": null,
            "ConfigurationKey": null
        },
        "RateLimitOptions": {
            "ClientIdHeader": "ClientId",
            "QuotaExceededMessage": null,
            "RateLimitCounterPrefix": "ocelot",
            "DisableRateLimitHeaders": false,
            "HttpStatusCode": 429
        },
        "QoSOptions": {
            "ExceptionsAllowedBeforeBreaking": 0,
            "DurationOfBreak": 0,
            "TimeoutValue": 0
        },
        "BaseUrl": null,
        "LoadBalancerOptions": {
            "Type": "LeastConnection",
            "Key": null,
            "Expiry": 0
        },
        "DownstreamScheme": "http",
        "HttpHandlerOptions": {
            "AllowAutoRedirect": false,
            "UseCookieContainer": false,
            "UseTracing": false
        }
    }
}
'@

$IdentityServer = @'
{
  "tags": "Authtentication,JWT,Token",
  "connectionString": "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AuthContext;Persist Security Info=True;Integrated Security=True;",
  "wolfMonitorConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=WolfMonitorContext;Persist Security Info=True;Integrated Security=True;",
  "issuerUri": "http://192.168.0.102",
  "agentsApiSecret":"agentSuperSecret",
  "monitoringApiSecret":"monitoringSuperSecret",
  "usersApiSecret":"usersSuperSecret",
  "companiesApiSecret":"companiesSuperSecret",
  "clients": [
    {
      "id": "agentService",
      "secret": "agentServiceSecret",
      "name": "Agent Service",
      "scopes": [
      		"Agents", "Monitoring"
      ]
    },
    {
      "id": "application",
      "secret": "applicationSecret",
      "name": "Application WPF",
      "scopes": [
      		"Agents", "Monitoring", "Users", "Companies"
      ]
    }
  ]
}
'@

$Register = @'
{
	"Tags": "Register",
	"connectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=WolfMonitorContext;Persist Security Info=True;Integrated Security=True;",
	"authConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AuthContext;Persist Security Info=True;Integrated Security=True;",
	"monitoringConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=WolfMonitoringContext;Persist Security Info=True;Integrated Security=True;",
	"apiSecret":"usersSuperSecret",
	"authSettings": {
		"secret": "IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw",
		"issuer": "https://192.168.0.102:9001/",
		"clientId": "099153c2625149bc8ecb3e85e03f0022"
	}
}
'@
$Agents = @'
{
  "Tags": "Agents",
  "connectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=WolfMonitorContext;Persist Security Info=True;Integrated Security=True;",
  "authConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AuthContext;Persist Security Info=True;Integrated Security=True;",
  "monitoringConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=WolfMonitoringContext;Persist Security Info=True;Integrated Security=True;",
  "apiName":"Agents",
  "apiSecret":"agentsSuperSecret"
}
'@

$Users = @'
{
  "Tags": "Users",
  "connectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=WolfMonitorContext;Persist Security Info=True;Integrated Security=True;",
  "authConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AuthContext;Persist Security Info=True;Integrated Security=True;",
  "monitoringConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=WolfMonitoringContext;Persist Security Info=True;Integrated Security=True;",
  "apiName":"Users",
  "apiSecret":"usersSuperSecret"
}
'@
$Monitoring = @'
{
  "Tags": "Monitoring",
  "connectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=WolfMonitorContext;Persist Security Info=True;Integrated Security=True;",
  "authConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AuthContext;Persist Security Info=True;Integrated Security=True;",
  "monitoringConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=WolfMonitoringContext;Persist Security Info=True;Integrated Security=True;",
  "apiName":"Monitoring",
  "apiSecret":"monitoringSuperSecret"
}
'@

$Companies = @'
{
  "Tags": "Companies",
  "connectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=WolfMonitorContext;Persist Security Info=True;Integrated Security=True;",
  "authConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AuthContext;Persist Security Info=True;Integrated Security=True;",
  "monitoringConnectionString":"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=WolfMonitoringContext;Persist Security Info=True;Integrated Security=True;",
  "apiName":"Companies",
  "apiSecret":"companiesSuperSecret"
}
'@

$response = Invoke-RestMethod -Method 'Put' -Uri $url"Global" -Body $Global
if($response -eq 'true') {
	Write-Output "Configuração Global criada com sucesso!"
}

$response = Invoke-RestMethod -Method 'Put' -Uri $url"Jobs" -Body $Jobs
if($response -eq 'true') {
	Write-Output "Configuração Jobs criada com sucesso!"
}

$response = Invoke-RestMethod -Method 'Put' -Uri $url"Gateway" -Body $Gateway
if($response -eq 'true') {
	Write-Output "Configuração Gateway criada com sucesso!"
}

$response = Invoke-RestMethod -Method 'Put' -Uri $url"IdentityServer" -Body $IdentityServer
if($response -eq 'true') {
	Write-Output "Configuração IdentityServer criada com sucesso!"
}


$response = Invoke-RestMethod -Method 'Put' -Uri $url"Register" -Body $Register
if($response -eq 'true') {
	Write-Output "Configuração Register criada com sucesso!"
}
$response = Invoke-RestMethod -Method 'Put' -Uri $url"Agents" -Body $Agents
if($response -eq 'true') {
	Write-Output "Configuração Agents criada com sucesso!"
}
$response = Invoke-RestMethod -Method 'Put' -Uri $url"Monitoring" -Body $Monitoring
if($response -eq 'true') {
	Write-Output "Configuração Monitoring criada com sucesso!"
}
$response = Invoke-RestMethod -Method 'Put' -Uri $url"Users" -Body $Users
if($response -eq 'true') {
	Write-Output "Configuração Users criada com sucesso!"
}
$response = Invoke-RestMethod -Method 'Put' -Uri $url"Companies" -Body $Companies
if($response -eq 'true') {
	Write-Output "Configuração Companies criada com sucesso!"
}
Write-Output ""
Write-Output "Configuração concluída com sucesso!"
Write-Output ""