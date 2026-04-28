<h1 align="center">⚖️ ParallAI</h1>

<p align="center">
  <a href="https://dotnet.microsoft.com/"><img alt=".NET" src="https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet"></a>
  <a href="https://github.com/feytox/ParallAI/pkgs/container/parallai"><img alt="Docker" src="https://img.shields.io/badge/Docker-GHCR-2496ED?logo=docker&logoColor=white"></a>
  <a href="https://t.me/parallaibot"><img alt="Telegram" src="https://img.shields.io/badge/Telegram-Bot-26A5E4?logo=telegram&logoColor=white"></a>
</p>

ParallAI — это self-hosted Telegram-бот для работы с несколькими AI-провайдерами из одного чата. 🤖 Бот позволяет настраивать модели и пресеты, отправлять одиночные или непрерывные запросы, прикреплять файлы и сравнивать ответы нескольких моделей через отдельную модель-оркестратор.

## ✨ Возможности

- 📱 **Telegram-бот** с главным меню, командами и inline-настройками.
- ⚡ **Одиночные запросы** к модели.
- 🔄 **Непрерывный режим** запросов.
- ⚖️ **Режим сравнения**: один промпт отправляется нескольким моделям, после чего модель-оркестратор объединяет и анализирует ответы.
- 🛠️ **Пользовательские модели и пресеты**.
- 🌐 **Поддержка провайдеров**:
  - Google Gemini;
  - OpenRouter;
  - OpenAI-compatible API: OpenAI, DeepSeek, Qwen, Z.AI, Ollama-compatible gateway и другие.
- 📎 **Поддержка мультимедиа**: текст, фотографии и документы из Telegram.
- 📝 **Конвертация Markdown** в Telegram MarkdownV2.
- 🛑 **Отмена** активной генерации.
- 💾 **Хранение данных** в MongoDB (пользователи, модели, пресеты, конфигурации и метрики).
- 📦 **Docker-образ** с публикацией в GitHub Container Registry.

## ⌨️ Команды бота

| Команда | Назначение |
| --- | --- |
| `/start`, `/help` | 🏠 Открыть главное меню. |
| `/ask` | ❓ Выбрать режим запроса и отправить промпт. |
| `/compare` | ⚖️ Запустить сравнение ответов нескольких моделей. |
| `/models` | ⚙️ Создать, выбрать и настроить AI-модели. |
| `/presets` | 📝 Создать, выбрать и настроить пресеты. |
| `/config` | 🔍 Показать текущую модель и пресет. |
| `/providerguide` | 📖 Открыть гайд по получению токенов провайдеров. |
| `/cancel` | ❌ Выйти из текущего пошагового сценария. |

## 🛠️ Стек

- **Runtime:** .NET 8
- **Bot API:** Telegram.Bot
- **Database:** MongoDB.Driver
- **Markdown:** Markdig
- **Hosting:** Microsoft.Extensions.Hosting
- **DI:** Scrutor
- **Testing:** NUnit, FluentAssertions, FakeItEasy, Moq
- **DevOps:** Docker и Docker Compose

## 📂 Структура проекта

```text
ParallAI.Core/                 ⚙️ Доменные сущности, состояния, сервисы, провайдеры, репозитории
ParallAI.Infrastructure/       🗄️ MongoDB, конфигурация окружения, HTTP-обработчики AI-провайдеров
ParallAI.TeleBot/              📱 Команды, callbacks, экраны настроек, Telegram-сервисы
ParallAI.TeleBot.Core/         🛠️ Общий фреймворк команд, callbacks, настроек и state actions
ParallAI.MarkdownV2/           📝 Конвертер Markdown в Telegram MarkdownV2
ParallAI.Metrics/              📊 Сервис метрик и типы запросов
ParallAI.Host/                 🚀 Точка входа приложения и Dockerfile
*.Tests/                       🧪 NUnit-тесты основных модулей
ProviderGuide/                 📖 Гайд по настройке провайдеров со скриншотами
```

## 🐳 Self-hosting через Docker

### 📋 Требования

- Docker и Docker Compose.
- Токен Telegram-бота от [BotFather](https://t.me/BotFather).

### 🚀 Быстрый старт

Создайте `compose.yaml`:

```yaml
services:
  parallai.host:
    image: ghcr.io/feytox/parallai:latest
    environment:
      - BOT_TOKEN=${BOT_TOKEN}
      - MONGO_CONNECTION_STRING=mongodb://${MONGO_USER}:${MONGO_PASS}@mongo-db:27017/ParallAIDB?authSource=admin
    depends_on:
      - mongo-db

  mongo-db:
    image: mongo:latest
    restart: always
    environment:
      - MONGO_INITDB_ROOT_USERNAME=${MONGO_USER}
      - MONGO_INITDB_ROOT_PASSWORD=${MONGO_PASS}
    volumes:
      - mongo_data:/data/db

volumes:
  mongo_data:
```

Создайте `.env`:

```ini
BOT_TOKEN=YOUR_TELEGRAM_BOT_TOKEN
MONGO_USER=admin
MONGO_PASS=passwd
```

Запустите бота:

```bash
docker compose up -d
```

Посмотрите логи:

```bash
docker compose logs -f parallai.host
```

### 🔄 Обновление

```bash
docker compose down
docker compose pull
docker compose up -d
```

### 🛑 Остановка

```bash
docker compose down
```

Остановить контейнеры и удалить данные MongoDB:

```bash
docker compose down -v
```

## 💻 Локальная разработка

### 📋 Требования

- .NET SDK 8.0.
- MongoDB локально или в Docker.
- Токен Telegram-бота.

### ⚙️ Переменные окружения

Приложение читает переменные из окружения и локального `.env` файла.

| Переменная | Обязательна | Значение по умолчанию | Описание |
| --- | --- | --- | --- |
| `BOT_TOKEN` | ✅ Да | нет | Токен Telegram-бота. |
| `MONGO_CONNECTION_STRING` | ❌ Нет | `mongodb://localhost:27017` | Строка подключения к MongoDB. |
| `USERS_COLLECTION` | ❌ Нет | `Users` | Коллекция MongoDB для пользовательского состояния. |

Пример локального `.env`:

```ini
BOT_TOKEN=YOUR_TELEGRAM_BOT_TOKEN
MONGO_CONNECTION_STRING=mongodb://localhost:27017
USERS_COLLECTION=Users
```

### 🏃 Запуск из исходников

Восстановите зависимости и соберите решение:

```bash
dotnet restore ParallAI.sln
dotnet build ParallAI.sln
```

Запустите MongoDB через compose-файлы репозитория:

```bash
docker compose up -d mongo-db
```

Запустите host-проект:

```bash
dotnet run --project ParallAI.Host/ParallAI.Host.csproj
```

### 🧪 Тесты

```bash
dotnet test ParallAI.sln
```

## 🔑 Настройка AI-провайдеров

Ключи провайдеров настраиваются внутри бота отдельно для каждого Telegram-пользователя. В репозитории есть пошаговый гайд по получению API-ключей:

👉 [ProviderGuide/PROVIDERGUIDE.md](ProviderGuide/PROVIDERGUIDE.md)

Поддерживаемые типы провайдеров:

- `Gemini`: доступ к Google Gemini по API-ключу. ♊
- `OpenRouter`: доступ к моделям OpenRouter по API-ключу. 🌐
- `OpenAI Compatible`: произвольный endpoint URL и API-ключ для OpenAI-style Chat Completions API. 🔌

## ⚖️ Как работает режим сравнения

Режим сравнения использует двухэтапный сценарий:

1. Один и тот же пользовательский промпт параллельно отправляется выбранным моделям. 📤
2. Ответы моделей объединяются в формате `<response_1>`, `<response_2>` и так далее. 🖇️
3. Модель-оркестратор получает исходный промпт и все собранные ответы. 🧠
4. Бот отправляет пользователю ответы каждой модели, а затем итоговый анализ оркестратора. ✅

Такой режим помогает находить расхождения между моделями, собирать консенсусный ответ и сравнивать качество разных провайдеров на одной задаче.

## 📦 Docker-образ

Docker-образ публикуется в GitHub Container Registry через workflow `.github/workflows/publish.yml`.

Публикуемые теги:
- 🏷️ semantic version tags, например `v1.2.3`;
- 🌿 названия веток;
- 🔢 SHA коммита;
- 🌟 `latest` для default branch.

Основной runtime-образ:

```text
ghcr.io/feytox/parallai:latest
```
