FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY ["src/TaskManagement.Listener/TaskManagement.Listener/TaskManagement.Listener.csproj", "src/TaskManagement.Listener/TaskManagement.Listener/"]
COPY ["src/TaskManagement.Listener/TaskManagement.Application/TaskManagement.Application.csproj", "src/TaskManagement.Listener/TaskManagement.Application/"]
COPY ["src/TaskManagement.Listener/TaskManagement.Infrastructure/TaskManagement.Infrastructure.csproj", "src/TaskManagement.Listener/TaskManagement.Infrastructure/"]
COPY ["src/TaskManagement.Shared/TaskManagement.Contracts/TaskManagement.Contracts.csproj", "src/TaskManagement.Shared/TaskManagement.Contracts/"]

RUN dotnet restore "src/TaskManagement.Listener/TaskManagement.Listener/TaskManagement.Listener.csproj"

COPY . .

WORKDIR "/source/src/TaskManagement.Listener/TaskManagement.Listener"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
WORKDIR /source/src/TaskManagement.Listener/TaskManagement.Listener
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
EXPOSE 8081

RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:8081/ || exit 1

ENTRYPOINT ["dotnet", "TaskManagement.Listener.dll"]