#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Movie.WebHost/Movie.WebHost.csproj", "Movie.WebHost/"]
RUN dotnet restore "Movie.WebHost/Movie.WebHost.csproj"
COPY . .
WORKDIR "/src/Movie.WebHost"
RUN dotnet build "Movie.WebHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Movie.WebHost.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movie.WebHost.dll"]