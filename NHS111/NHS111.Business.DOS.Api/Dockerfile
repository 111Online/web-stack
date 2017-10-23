FROM microsoft/aspnet:4.7

WORKDIR /inetpub/wwwroot

ARG source
COPY ${source:-obj/Docker/publish/_PublishedWebsites/nhs111-business-dos-api} .
COPY ./start.ps1 .

RUN Set-WebBinding 'Default Web Site' -BindingInformation "*:80:" -PropertyName "Port" -Value "84"
CMD powershell ./start.ps1
