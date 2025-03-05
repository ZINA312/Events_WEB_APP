#!/bin/bash

# Проверка доступности PostgreSQL через pg_isready
until pg_isready -h postgres -p 5432 -U postgres -d EventsDB -t 1
do
  echo "Ожидание PostgreSQL..."
  sleep 2
done

# Применение миграций
dotnet ef database update --project Events_WEB_APP.Persistence --startup-project Events_WEB_APP.API

# Запуск приложения
exec dotnet Events_WEB_APP.API.dll