# Используем официальный образ .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Копируем все файлы проекта в контейнер
COPY . /app
WORKDIR /app

# Устанавливаем переменные окружения для .NET и NuGet
ENV DOTNET_CLI_HOME=/tmp/.dotnet
ENV NUGET_PACKAGES=/app/.nuget

# Изменяем права доступа
RUN chmod -R 777 /app/.dotnet /app/.nuget /app

# Восстанавливаем и строим серверную часть
RUN dotnet restore /app/Server/Server.csproj
RUN dotnet build /app/Server/Server.csproj

# Восстанавливаем и строим клиентскую часть
RUN dotnet restore /app/Client/Client.csproj
RUN dotnet build /app/Client/Client.csproj

# Восстанавливаем и строим проект тестов
RUN dotnet restore /app/Tests/Tests.csproj
RUN dotnet build /app/Tests/Tests.csproj

# Запускаем тесты
RUN dotnet test /app/Tests/Tests.csproj

# Копируем скрипт для запуска
COPY start.sh /app/start.sh

# Делаем скрипт исполнимым
RUN chmod +x /app/start.sh

# Запускаем сервер и клиент с помощью скрипта
CMD ["/app/start.sh"]
