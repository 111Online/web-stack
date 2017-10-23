FROM microsoft/aspnet:4.7

WORKDIR /inetpub/wwwroot

ARG source
COPY ${source:-obj/Docker/publish/_PublishedWebsites/nhs111-integration-dos-api} .
COPY ./start.ps1 .

RUN Set-WebBinding 'Default Web Site' -BindingInformation "*:80:" -PropertyName "Port" -Value "86"
CMD powershell ./start.ps1