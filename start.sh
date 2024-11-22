#!/bin/bash

# Запуск сервера в фоновом режиме
dotnet /app/Server/bin/Debug/net6.0/Server.dll &

# Запуск клиента
dotnet /app/Client/bin/Debug/net6.0/Client.dll
