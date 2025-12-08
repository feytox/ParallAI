FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["ParallAI.Host/ParallAI.Host.csproj", "ParallAI.Host/"]
COPY ["ParallAI.Infrastructure/ParallAI.Infrastructure.csproj", "ParallAI.Infrastructure/"]
COPY ["ParallAI.Core/ParallAI.Core.csproj", "ParallAI.Core/"]
COPY ["ParallAI.TeleBot/ParallAI.TeleBot.csproj", "ParallAI.TeleBot/"]
COPY ["ParallAI.TeleBot.Core/ParallAI.TeleBot.Core.csproj", "ParallAI.TeleBot.Core/"]
COPY ["ParallAI.MarkdownV2/ParallAI.MarkdownV2.csproj", "ParallAI.MarkdownV2/"]

RUN dotnet restore "ParallAI.Host/ParallAI.Host.csproj"

COPY . .
WORKDIR "/src/ParallAI.Host"
RUN dotnet build "./ParallAI.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ParallAI.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParallAI.Host.dll"]
