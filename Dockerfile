FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY App_practical.csproj ./
RUN dotnet restore App_practical.csproj
COPY . .
RUN dotnet publish App_practical.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:5009
ENTRYPOINT ["dotnet", "App_practical.dll"]
