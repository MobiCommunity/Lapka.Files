FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY Lapka.Files.Api/Lapka.Files.Api.csproj Lapka.Files.Api/Lapka.Files.Api.csproj
COPY Lapka.Files.Application/Lapka.Files.Application.csproj Lapka.Files.Application/Lapka.Files.Application.csproj
COPY Lapka.Files.Core/Lapka.Files.Core.csproj Lapka.Files.Core/Lapka.Files.Core.csproj
COPY Lapka.Files.Infrastructure/Lapka.Files.Infrastructure.csproj Lapka.Files.Infrastructure/Lapka.Files.Infrastructure.csproj
RUN dotnet restore Lapka.Files.Api

COPY . .
RUN dotnet publish Lapka.Files.Api -c release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS http://*:5003
ENV ASPNETCORE_ENVIRONMENT docker

EXPOSE 5003

ENTRYPOINT dotnet Lapka.Files.Api.dll