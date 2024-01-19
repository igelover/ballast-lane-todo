#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ballast.Todo.API/Ballast.Todo.API.csproj", "Ballast.Todo.API/"]
COPY ["Ballast.Todo.Application/Ballast.Todo.Application.csproj", "Ballast.Todo.Application/"]
COPY ["Ballast.Todo.Domain/Ballast.Todo.Domain.csproj", "Ballast.Todo.Domain/"]
COPY ["Ballast.Todo.Infrastructure/Ballast.Todo.Infrastructure.csproj", "Ballast.Todo.Infrastructure/"]
RUN dotnet restore "./Ballast.Todo.API/./Ballast.Todo.API.csproj"
COPY . .
WORKDIR "/src/Ballast.Todo.API"
RUN dotnet build "./Ballast.Todo.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ballast.Todo.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ballast.Todo.API.dll"]