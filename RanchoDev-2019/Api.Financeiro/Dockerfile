FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Api.Financeiro/Api.Financeiro.csproj", "Api.Financeiro/"]
RUN dotnet restore "Api.Financeiro/Api.Financeiro.csproj"
COPY . .
WORKDIR "/src/Api.Financeiro"
RUN dotnet build "Api.Financeiro.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.Financeiro.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.Financeiro.dll"]