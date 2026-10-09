FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["AppointmentApp/AppointmentApp.csproj", "AppointmentApp/"]
RUN dotnet restore "AppointmentApp/AppointmentApp.csproj"

COPY . .
WORKDIR /src/AppointmentApp

RUN dotnet publish "AppointmentApp.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AppointmentApp.dll"]