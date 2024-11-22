# Используем официальный образ Rocky Linux
FROM rockylinux:8

# Используем официальный образ .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Копируем все файлы проекта в контейнер
COPY . /app
WORKDIR /app

USER root

# Устанавливаем переменные окружения для .NET и NuGet
ENV DOTNET_CLI_HOME=/app/.dotnet
ENV NUGET_PACKAGES=/app/.nuget

# Изменяем права доступа
RUN chmod -R 755 /app
RUN chmod -R 755 /tmp/.dotnet

# Сначала копируем и компилируем серверную часть
RUN dotnet restore /app/Server/Server.csproj
RUN dotnet build /app/Server/Server.csproj

# Теперь копируем и компилируем клиентскую часть
RUN dotnet restore /app/Client/Client.csproj
RUN dotnet build /app/Client/Client.csproj

# Копируем скрипт для запуска обоих процессов
COPY start.sh /app/start.sh

# Делаем скрипт исполнимым
RUN chmod +x /app/start.sh

# Запускаем сервер и клиент с помощью скрипта
CMD ["/app/start.sh"]
