FROM microsoft/aspnet:4.7

WORKDIR /inetpub/wwwroot

ARG source
COPY ${source:-obj/Docker/publish/_PublishedWebsites/nhs111-domain-dos-api} .

RUN Set-WebBinding 'Default Web Site' -BindingInformation "*:80:" -PropertyName "Port" -Value "85"
CMD powershell ./start.ps1
