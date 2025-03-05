# Events

Веб-приложение для управления событиями с аутентификацией пользователей и административной панелью.

---
## 🚀 Быстрый старт
### Необходимые компоненты
- [PostgreSQL](https://www.postgresql.org/download/) (версия 14+)
### ⚙️ Настройка базы данных
1. Создайте новую БД в PostgreSQL
2. Обновите строку подключения в `appsettings.json` проекта API и Persistence:

```json
"ConnectionStrings": {
  "EventsDatabase": "Host=localhost;Port=5432;Database=EventsDB;Username=postgres;Password=12345;"
}
```
### 🛠 Применение миграций
Выберите подходящий способ:
#### Через Package Manager Console:
```Package Manager
Update-Database -Project Events_WEB_APP.Persistence -StartupProject Events_WEB_APP.API
```
#### Через .NET CLI:
```.NET CLI
dotnet ef database update --project Events_WEB_APP.Persistence --startup-project Events_WEB_APP.API
```
### ▶️ Запуск приложения
1. Запустите проект в Visual Studio
2. Приложение будет доступно по адресу: https://localhost:7271

### 🔍 Доступ к API
Документация Swagger доступна после запуска: 📚 [Swagger UI](https://localhost:7271/swagger/index.html "Swagger UI")

### 🔑 Учётные данные администратора
Для доступа к административным функциям:

+ Email: ```admin@events.com```
+ Password: ```Admin```