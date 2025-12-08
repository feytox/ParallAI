# ParallAI

## Self-hosting через Docker
1. Убедитесь, что у вас установлен Docker

2. Создайте файл `compose.yml` и вставьте в него следующее:

`compose.yml`
```yml
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
    ports:
      - "27017:27017"
    environment:
      - MONGO_INITDB_ROOT_USERNAME=${MONGO_USER}
      - MONGO_INITDB_ROOT_PASSWORD=${MONGO_PASS}
    volumes:
      - mongo_data:/data/db

volumes:
  mongo_data:
```

3. Создайте файл `.env`, вставьте в него следующее и вставьте токен вашего тг-бота:

`.env`
```ini
BOT_TOKEN=ВАШ_ТОКЕН_ТГ_БОТА
MONGO_USER=admin
MONGO_PASS=passwd
```

4. Для запуска используйте:

```
docker compose up -d
```