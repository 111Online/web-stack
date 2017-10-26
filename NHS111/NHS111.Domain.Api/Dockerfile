FROM microsoft/aspnet:4.7

WORKDIR /inetpub/wwwroot

ARG source
COPY ./obj/octopacked/ .
COPY ./start.ps1 .

RUN Set-WebBinding 'Default Web Site' -BindingInformation "*:80:" -PropertyName "Port" -Value "83"
EXPOSE 83

ENTRYPOINT powershell ./start.ps1