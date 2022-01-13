FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80
ENV mongodbConnection="mongodb://root:w5ePt4oSvPlJ6r9WDM@cloud-mongo:27017/"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Infotecs.MobileMonitoring/Infotecs.MobileMonitoring.csproj", "Infotecs.MobileMonitoring/"]
RUN dotnet restore "Infotecs.MobileMonitoring/Infotecs.MobileMonitoring.csproj"
COPY . .
WORKDIR "/src/Infotecs.MobileMonitoring"
RUN dotnet build "Infotecs.MobileMonitoring.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Infotecs.MobileMonitoring.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Infotecs.MobileMonitoring.dll"]
