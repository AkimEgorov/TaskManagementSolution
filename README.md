# TaskManagement Solution

API-сервис, предназначенный для простого управления задачами пользователя (Task Management Service)

## Описание

Решение состоит из двух микросервисов:
- **TaskManagement.Api** — REST API для CRUD-операций над задачами (Producer).
- **TaskManagement.Listener** — сервис-слушатель, который асинхронно обрабатывает события жизненного цикла задач и ведёт аудит (Consumer).

Связь между сервисами реализована через брокер сообщений **RabbitMQ** с использованием библиотеки **MassTransit**.

## Быстрый старт

### Предварительные требования
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (с включённым Docker Compose)

### Запуск через Docker Compose (рекомендуется)

```powershell
# Клонируем репозиторий
git clone <your-repo-url>
cd TaskManagementSolution

# Собираем образы и запускаем все сервисы
docker compose up --build -d

# Проверяем статус контейнеров
docker compose ps
```

После успешного запуска доступны:
- **API Swagger UI**: http://localhost:5000/swagger
- **Listener Swagger UI**: http://localhost:5001/swagger
- **RabbitMQ Management UI**: http://localhost:15672 (логин: `guest`, пароль: `guest`)

### Локальный запуск (без Docker)

```powershell
# Терминал 1: Запуск API
dotnet run --project src/TaskManagement.Api/TaskManagement.Api/TaskManagement.Api.csproj

# Терминал 2: Запуск Listener
dotnet run --project src/TaskManagement.Listener/TaskManagement.Listener/TaskManagement.Listener.csproj
```

> ⚠️ Для локального запуска требуется доступная PostgreSQL (порт 5432) и RabbitMQ (порт 5672).


## Просмотр логов и трассировки

### Логи API
```powershell
docker compose logs -f api
```

### Логи Listener (с событиями)
```powershell
docker compose logs -f listener
```

### Текстовые файлы логов
Логи также пишутся в rolling-файлы:
- API: `src/TaskManagement.Api/TaskManagement.Api/logs/taskmanagement-*.txt`
- Listener: `src/TaskManagement.Listener/TaskManagement.Listener/logs/listener-*.txt`

## Запуск Unit-тестов

```powershell
dotnet test tests/TaskManagement.Tests/TaskManagement.Tests.csproj
```

### Генерация миграций EF Core
```powershell
# Для API
dotnet ef migrations add <MigrationName> --project src/TaskManagement.Api/TaskManagement.Infrastructure --startup-project src/TaskManagement.Api/TaskManagement.Api

# Для Listener
dotnet ef migrations add <MigrationName> --project src/TaskManagement.Listener/TaskManagement.Infrastructure --startup-project src/TaskManagement.Listener/TaskManagement.Listener
```

### Остановка сервисов
```powershell
docker compose down         # Остановить контейнеры
docker compose down -v      # Остановить и удалить volume с данными БД
```
---

## Задание 2: Табличная функция агрегации платежей (SQL)

В корне репозитория находится файл **`TableFunction.sql`**, содержащий реализацию табличной функции для получения поденных сумм платежей клиента за заданный интервал дат (включая дни без платежей, для которых возвращается `0`).