FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY . .

FROM build AS publish
WORKDIR /app
RUN dotnet publish app/PaymentGateway/PaymentGateway.csproj --framework netcoreapp3.1 -c Release -o out -p:PublishWithAspNetCoreTargetManifest=false


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS runtime
WORKDIR /app
COPY --from=publish /app/out ./

EXPOSE 80/tcp
EXPOSE 443/tcp

ENV ASPNETCORE_ENVIRONMENT="DOCKER"
ENV ASPNETCORE_URLS="http://+:80"

ENTRYPOINT ["dotnet", "PaymentGateway.dll"]