# FROM mcr.microsoft.com/dotnet/runtime:3.1.12-alpine3.12
# arm32v7/alpine:3.12
# FROM mcr.microsoft.com/dotnet/runtime-deps:3.1-bionic-arm32v7

# это дебиан
# FROM mcr.microsoft.com/dotnet/runtime:3.1
# https://devandchill.com/posts/2019/06/net-core-3.0-publish-single-file-binary-on-alpine-container/


FROM mcr.microsoft.com/dotnet/runtime:3.1-bionic-arm32v7

# RUN apt-get -y update && apt-get -y install systemd

COPY app/ app/

WORKDIR /app

ENTRYPOINT ["dotnet", "nostromo-muthur.dll"]
