#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["DevWeek-20200/DevWeek-20200.csproj", "DevWeek-20200/"]
RUN dotnet restore "DevWeek-20200/DevWeek-20200.csproj"
COPY . .
WORKDIR "/src/DevWeek-20200"
RUN dotnet build "DevWeek-20200.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevWeek-20200.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevWeek-20200.dll"]