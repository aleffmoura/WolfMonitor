# WolfMonitor
# Link para defesa do TCC | NOTA DO TCC: 10
https://www.facebook.com/aleffmds/videos/1501831676823028

## TCC para faculdade

Sistema para monitoramento de serviços e arquivos em servidores linux e windows, com o intuito de melhorar na qualidade e na forma em como servidores são monitorados.

# Importante para fazer funcionar
*Instalar o docker*
na pasta "Docker Installer" existe um arquivo chamado "install.ps1"
executar o arquivo no power shell.

*migration*
necessario aplicar migration para o entity framework funcionar corretamente.
tela de login ja possui o usuario e senha colocados.

Deverá serem feitos cadastros dos agents(servidores) aos quais serão monitorados, na guia de agents do front end, onde conterá todas as informações, necessario tbm inserir serviços que serão monitorados, adicionando o nome e nome de visualização; por exemplo o windows update, o nome do serviço é: wuauserv, seu nome de visualização pode ser qualquer coisa que o dev achar necessario.
Em Situações com linux, é necessario colocar o nome do Daemon corretamente como por exemplo "mysqld.service" e o nome de visualização o que o dev julgar melhor, em caso de monitoramento de arquivos, o nome de visualização tbm é o que o dev julgar melhor, e o nome é o nome completo, por exemplo: "C:\users\arquivo.txt" ou no caso do linux "/home/user/arquivo.txt" 

## o sistema possui 3 vertentes.

### Primeira
Front end -  o front end foi feito em WPF onde é mostrada as informações salvas na base de dados do sistema, a qual fica em uma api

### Segunda
Serviço - o serviço é um serviço windows/linux, funciona em ambas as plataformas, a qual tem um arquivo de configuração que deve ser configurado com o usuario e senha criado na guia de agents no front end, onde dessa maneira estara apito a funcionar corretamente.

### Terceira
uma Api onde é enviada e recebidas informações tanto pelo front end quanto pelo serviço de monitoramento

### Para mais informações entre em contato
