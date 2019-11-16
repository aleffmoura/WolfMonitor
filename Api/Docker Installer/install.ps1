# Valida se o script está rodando como administrador.
if (-Not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] 'Administrator')) {
 if ([int](Get-CimInstance -Class Win32_OperatingSystem | Select-Object -ExpandProperty BuildNumber) -ge 6000) {
  $CommandLine = "-File `"" + $MyInvocation.MyCommand.Path + "`" " + $MyInvocation.UnboundArguments
  Write-Output "Script deve ser executado como Administrador!"
  #Write-Output "Uma nova janela será aberta automaticamente no diretório: "(Split-Path $MyInvocation.MyCommand.Path)
  Start-Process -FilePath PowerShell.exe -Verb Runas -ArgumentList $CommandLine -WorkingDirectory (Split-Path $MyInvocation.MyCommand.Path)
  Write-Output "Essa janela será encerrada em 10 segundos..."
  Start-Sleep -s 10
  Exit
 }
}

clear
#Altera para o diretorio que contem o arquivo de configuracao do docker
cd (Split-Path $MyInvocation.MyCommand.Path)

#Autenticar com repositorio privado de imagens da UDS aqui!
#Write-Output "Autenticando com udscr.azurecr.io..."
#docker login 'udscr.azurecr.io' -u 'user' -p 'pass'


Write-Output "Instalando e iniciando containers..."
docker-compose up -d

Write-Output "Aguardando 30 segundos por warm up dos containers..."
Start-Sleep -s 30

$urlConsul = "http://localhost:8500"
& .\conf-consul.ps1 $urlConsul"/v1/kv/"

Write-Output ""
Write-Output "Instalação concluída com sucesso!"
Write-Output ""

Start-Sleep -s 4
docker stats