FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY ["src/TaskManagement.Api/TaskManagement.Api/TaskManagement.Api.csproj", "src/TaskManagement.Api/TaskManagement.Api/"]
COPY ["src/TaskManagement.Api/TaskManagement.Application/TaskManagement.Application.csproj", "src/TaskManagement.Api/TaskManagement.Application/"]
COPY ["src/TaskManagement.Api/TaskManagement.Domain/TaskManagement.Domain.csproj", "src/TaskManagement.Api/TaskManagement.Domain/"]
COPY ["src/TaskManagement.Api/TaskManagement.Infrastructure/TaskManagement.Infrastructure.csproj", "src/TaskManagement.Api/TaskManagement.Infrastructure/"]
COPY ["src/TaskManagement.Shared/TaskManagement.Contracts/TaskManagement.Contracts.csproj", "src/TaskManagement.Shared/TaskManagement.Contracts/"]

RUN dotnet restore "src/TaskManagement.Api/TaskManagement.Api/TaskManagement.Api.csproj"

COPY . .

WORKDIR "/source/src/TaskManagement.Api/TaskManagement.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
WORKDIR /source/src/TaskManagement.Api/TaskManagement.Api
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
EXPOSE 8080

RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:8080/ || exit 1

ENTRYPOINT ["dotnet", "TaskManagement.Api.dll"]