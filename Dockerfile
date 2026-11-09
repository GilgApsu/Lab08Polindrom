# ЭТАП 1: сборка
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Копируем только csproj и восстанавливаем зависимости — для кэширования слоёв
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы и публикуем релизную сборку
COPY . ./
RUN dotnet publish -c Release -o out

# ЭТАП 2: запуск в лёгком runtime-образе
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Копируем только результат публикации из этапа сборки
COPY --from=build-env /app/out .

# Приложение слушает порт 8080
EXPOSE 8080

# Запуск приложения
ENTRYPOINT ["dotnet", "Lab08Polindrom.dll"]