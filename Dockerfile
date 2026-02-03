# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

EXPOSE 80
EXPOSE 443

# copy csproj and restore as distinct layers
COPY . .
RUN find . -name '*.csproj' -exec dotnet restore {} \;

WORKDIR /source/YATM
RUN dotnet publish YATM.csproj -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "YATM.dll"]
