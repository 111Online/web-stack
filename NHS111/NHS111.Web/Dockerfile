FROM microsoft/aspnet:4.7

WORKDIR /inetpub/wwwroot

ARG source
COPY ./obj/octopacked/ .
COPY ./start.ps1 .

ENTRYPOINT powershell ./start.ps1