FROM microsoft/aspnet:4.7

WORKDIR /inetpub/wwwroot

ARG source
COPY ${source:-obj/Docker/publish/_PublishedWebsites/nhs111-domain-api} .

RUN Set-WebBinding 'Default Web Site' -BindingInformation "*:80:" -PropertyName "Port" -Value "83"
CMD powershell ./start.ps1