FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ClientImplicit.MVC/ClientImplicit.MVC.csproj", "ClientImplicit.MVC/"]
RUN dotnet restore "ClientImplicit.MVC/ClientImplicit.MVC.csproj"
COPY . .
WORKDIR "/src/ClientImplicit.MVC"
RUN dotnet build "ClientImplicit.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClientImplicit.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClientImplicit.MVC.dll"]