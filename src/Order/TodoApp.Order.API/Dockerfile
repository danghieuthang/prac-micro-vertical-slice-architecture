#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 15002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Order/TodoApp.Order.API/TodoApp.Order.API.csproj", "src/Order/TodoApp.Order.API/"]
RUN dotnet restore "src/Order/TodoApp.Order.API/TodoApp.Order.API.csproj"
COPY . .
WORKDIR "/src/src/Order/TodoApp.Order.API"
RUN dotnet build "TodoApp.Order.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoApp.Order.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApp.Order.API.dll"]